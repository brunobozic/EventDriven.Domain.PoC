using AutoMapper;
using AutoMapper.QueryableExtensions;
using IdentityService.Api.Controllers.BaseControllerType;
using IdentityService.Api.Extensions;
using IdentityService.Application.DomainServices.CountryService;
using IdentityService.Application.ViewModels;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using SharedKernel.Helpers.Configuration;
using SharedKernel.RequestResponsePattern;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using URF.Core.Abstractions;

namespace IdentityService.Api.Controllers;

[Route("api/country")]
[ApiController]
[Produces("application/json")]
[AllowAnonymous]
public class CountryController : BaseController
{
    #region Private props

    private readonly ICountryService _countryStateService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CountryController> _logger;

    #endregion Private props

    #region Ctor

    public CountryController(
        IMyUnitOfWork unitOfWork
        , ICountryService countryService
        , IMapper mapper
        , IHttpContextAccessor contextAccessor
        , IOptionsSnapshot<MyConfigurationValues> myConfigurationValues
        , IConfiguration configuration
        , ILogger<CountryController> logger
    ) : base(unitOfWork, mapper, myConfigurationValues, null, contextAccessor, configuration)
    {
        _countryStateService = countryService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    #endregion Ctor

    #region Public get

    [HttpGet("{countryId:long}")]
    [SwaggerOperation(Summary = "Retrieves details of a specific country by its ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CountryViewModel), Description = "Returns the country matching the specified ID.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid country ID supplied.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Country not found.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.")]
    [Produces(typeof(CountryViewModel))]
    public async Task<IActionResult> Get([FromRoute][Required] long countryId, CancellationToken ct)
    {
        if (countryId <= 0)
        {
            Log.Warning("Attempted to retrieve a country with an invalid Id: {CountryId}", countryId);
            return BadRequest("Invalid country Id.");
        }

        try
        {
            var country = await _countryStateService
                .Queryable()
                .AsNoTracking()
                .Where(c => c.Id == countryId)
                .ProjectTo<CountryViewModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(ct);

            if (country == null)
            {
                Log.Information("Country with Id: {CountryId} not found.", countryId);
                return NotFound($"Country with Id {countryId} not found.");
            }

            Log.Information("Retrieved country with Id: {CountryId} successfully.", countryId);
            return Ok(country);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to retrieve country with Id: {CountryId}", countryId);
            return new ObjectResult(HandleEnvironmentBasedResponse(ex, Configuration["ApplicationSettings:Environment"],
                nameof(Get), base.Configuration["ApplicationSettings:GenericErrorMessageForEndUser"],
                Request.Headers["X-Correlation-Id"], Request.Headers.Origin))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    #endregion Public get


    #region Paginated get

    [HttpPost("paginated")]
    [SwaggerOperation(Summary = "Retrieves a paginated list of countries based on search criteria.")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PagedResponse<CountryViewModel>), Description = "Successful response with paginated list of countries.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request parameters.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.")]
    [Produces(typeof(PagedResponse<CountryViewModel>))]
    public async Task<IActionResult> Paginated([FromBody][Required] GetPaginatedCountryRequest request,
        CancellationToken ct)
    {
        Log.Information("Starting paginated retrieval for countries.");

        if (request.PageSize <= 0) request.PageSize = ConfigurationValues.DefaultPageSize;
        if (request.PageNumber <= 0) request.PageNumber = ConfigurationValues.DefaultPageNumber;

        try
        {
            var query = _countryStateService
                .Queryable()
                .Where(u => EF.Functions.Like(u.Name, $"%{request.SearchCriteria}%") ||
                            EF.Functions.Like(u.ISO_3166_ALPHA_2, $"%{request.SearchCriteria}%") ||
                            EF.Functions.Like(u.ISO_3166_ALPHA_3, $"%{request.SearchCriteria}%") ||
                            EF.Functions.Like(u.LocalName, $"%{request.SearchCriteria}%") ||
                            EF.Functions.Like(u.ISO_3166_NUMERIC, $"%{request.SearchCriteria}%"));

            query = DynamicOrdering(query, request.OrderBy, request.SortOrder);

            var totalCount = await query.CountAsync(ct);

            var items = await query.AsNoTracking()
                .Paging(request.PageSize, request.PageNumber)
                .ProjectTo<CountryViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);

            var (deleted, inactive) = await CountInactiveOrDeletedItems(ct);

            var responseMessage = items.Any()
                ? $"Page [ {request.PageNumber} ] of [ {Math.Ceiling((double)totalCount / request.PageSize)} ], total records: [ {totalCount} ]. Additionally, there were related entities omitted due to being inactive or deleted: D: {deleted}, Inactive: {inactive}."
                : $"No results were found by your search criteria: [ {request?.SearchCriteria} ].";

            var response = new PagedResponse<CountryViewModel>
            {
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                ItemsCount = totalCount,
                Model = items,
                Message = responseMessage
            };

            Log.Information(
                "Paginated retrieval of countries successful. Page: {PageNumber}, PageSize: {PageSize}, TotalCount: {TotalCount}.",
                request.PageNumber, request.PageSize, totalCount);
            return Ok(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while processing paginated retrieval of countries. Request: {@Request}",
                request);
            return new ObjectResult(HandleEnvironmentBasedResponse(ex, Configuration["ApplicationSettings:Environment"],
                nameof(Paginated), Configuration["ApplicationSettings:GenericErrorMessageForEndUser"],
                Request.Headers["X-Correlation-Id"], Request.Headers.Origin))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    private IQueryable<CountryCodebook> DynamicOrdering(IQueryable<CountryCodebook> query, string orderBy,
        string sortOrder)
    {
        var isDescending = string.Equals(sortOrder?.Trim(), "desc", StringComparison.OrdinalIgnoreCase);

        return orderBy.Trim().ToLower() switch
        {
            "datecreated" => isDescending
                ? query.OrderByDescending(q => q.DateCreated)
                : query.OrderBy(q => q.DateCreated),
            "countryid" => isDescending ? query.OrderByDescending(q => q.Id) : query.OrderBy(q => q.Id),
            "name" => isDescending ? query.OrderByDescending(q => q.Name) : query.OrderBy(q => q.Name),
            "localname" => isDescending ? query.OrderByDescending(q => q.LocalName) : query.OrderBy(q => q.LocalName),
            "iso_3166_alpha_2" => isDescending
                ? query.OrderByDescending(q => q.ISO_3166_ALPHA_2)
                : query.OrderBy(q => q.ISO_3166_ALPHA_2),
            "iso_3166_alpha_3" => isDescending
                ? query.OrderByDescending(q => q.ISO_3166_ALPHA_3)
                : query.OrderBy(q => q.ISO_3166_ALPHA_3),
            _ => query.OrderBy(q => q.DateCreated) // Default ordering
        };
    }

    private async Task<(int deleted, int inactive)> CountInactiveOrDeletedItems(CancellationToken ct)
    {
        var countDeleted = await _countryStateService.Queryable()
            .CountAsync(c => c.Deleted, ct);

        var countInactive = await _countryStateService.Queryable()
            .CountAsync(c => !c.IsActive, ct);


        return (countDeleted, countInactive);
    }

    #endregion

    #region CUD

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new country entity.")]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(CountryViewModel), Description = "The country was successfully created.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data or country already exists.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.")]
    [Produces(typeof(CountryViewModel))]
    public async Task<IActionResult> Insert([FromBody][Required] CountryViewModel request, CancellationToken ct)
    {
        Log.Information("Starting country creation for {Name}", request.INT_NAME);

        if (request == null)
        {
            Log.Warning("Insert method received a null request.");
            return BadRequest("Request cannot be null.");
        }

        var existingCountry = await _countryStateService.FindAsync(request.INT_NAME, ct);
        if (existingCountry != null)
        {
            Log.Error("Country creation failed for {Name}. Error: Country already exists", request.INT_NAME);
            return BadRequest($"Country named {request.INT_NAME} already exists.");
        }

        try
        {
            var domain = _mapper.Map<CountryCodebook>(request);
            _countryStateService.Insert(domain);
            var res = await _unitOfWork.SaveChangesAsync(ct);

            if (res > 0)
            {
                Log.Information("Country {Name} created successfully.", request.INT_NAME);
                var createdCountry = _mapper.Map<CountryViewModel>(domain);
                return CreatedAtAction(nameof(Insert), new { id = domain.Id }, createdCountry);
            }

            Log.Error("Country {Name} not created. No changes were made during save.", request.INT_NAME);
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create the country.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while processing the creation of country {Name}.", request.INT_NAME);
            return new ObjectResult(HandleEnvironmentBasedResponse(ex, Configuration["ApplicationSettings:Environment"],
                nameof(Insert), Configuration["ApplicationSettings:GenericErrorMessageForEndUser"],
                Request.Headers["X-Correlation-Id"], Request.Headers.Origin))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Deletes a specific country by its ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Country successfully deleted.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid country ID supplied.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Country not found or already deleted.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.")]
    [Produces(typeof(string))]
    public async Task<IActionResult> Delete([Required] long id, CancellationToken ct)
    {
        if (id <= 0)
        {
            Log.Warning("Attempt to delete country with invalid Id: {CountryId}", id);
            return BadRequest("Invalid country Id.");
        }

        var existingCountry = await _countryStateService
            .Queryable()
            .Where(c => c.Id == id)
            .AsNoTracking()
            .SingleOrDefaultAsync(ct);

        if (existingCountry == null)
        {
            Log.Information("Attempt to delete non-existing or already deleted country with Id: {CountryId}", id);
            return NotFound("Country not found.");
        }

        try
        {
            _countryStateService.Delete(existingCountry);

            var saveResult = await _unitOfWork.SaveChangesAsync(ct);

            if (saveResult > 0)
            {
                Log.Information("Country with Id: {CountryId} was successfully deleted.", id);
                return Ok($"Country with Id {id} successfully deleted.");
            }

            Log.Error("Failed to delete country with Id: {CountryId}. Save changes resulted in no modifications.", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                "An error occurred while deleting the country.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Exception occurred while attempting to delete country with Id: {CountryId}.", id);
            return new ObjectResult(HandleEnvironmentBasedResponse(ex, Configuration["ApplicationSettings:Environment"],
                nameof(Delete), Configuration["ApplicationSettings:GenericErrorMessageForEndUser"],
                Request.Headers["X-Correlation-Id"], Request.Headers.Origin))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    [HttpPut("{countryId:long}")]
    [SwaggerOperation(Summary = "Updates a specific country by its ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CountryViewModel),
        Description = "Country successfully updated.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid country ID or mismatch between URL and body ID.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Country not found.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.")]
    [Produces(typeof(CountryViewModel))]
    public async Task<IActionResult> Edit([FromRoute][Required] long countryId,
        [FromBody][Required] UpdateCountryRequest request, CancellationToken ct)
    {
        if (countryId <= 0 || countryId != request.CountryId)
        {
            Log.Warning("Edit attempt with mismatched or invalid country Id: {CountryId}, Request: {@Request}",
                countryId, request);
            return BadRequest("Invalid request data.");
        }

        var existingCountry = await _countryStateService
            .Queryable()
            .Where(c => c.Id == countryId)
            .SingleOrDefaultAsync(ct);

        if (existingCountry == null)
        {
            Log.Information("Attempt to edit non-existing or deleted country with Id: {CountryId}", countryId);
            return NotFound("Country not found.");
        }

        try
        {
            _mapper.Map(request, existingCountry);
            _countryStateService.Update(existingCountry);

            var saveResult = await _unitOfWork.SaveChangesAsync(ct);

            if (saveResult > 0)
            {
                Log.Information("Successfully edited country with Id: {CountryId}", countryId);
                var updatedCountry = await _countryStateService
                    .Queryable()
                    .AsNoTracking()
                    .Where(c => c.Id == countryId)
                    .ProjectTo<CountryViewModel>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(ct);

                return Ok(updatedCountry);
            }

            Log.Error("Failed to edit country with Id: {CountryId}. No changes saved.", countryId);
            return new ObjectResult(HandleEnvironmentBasedResponse(new Exception("Failed to edit country"),
                Configuration["ApplicationSettings:Environment"],
                nameof(Edit), Configuration["ApplicationSettings:GenericErrorMessageForEndUser"],
                Request.Headers["X-Correlation-Id"], Request.Headers.Origin))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Exception occurred while attempting to edit country with Id: {CountryId}.", countryId);
            return new ObjectResult(HandleEnvironmentBasedResponse(ex, Configuration["ApplicationSettings:Environment"],
                nameof(Edit), Configuration["ApplicationSettings:GenericErrorMessageForEndUser"],
                Request.Headers["X-Correlation-Id"], Request.Headers.Origin))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    #endregion CUD
}