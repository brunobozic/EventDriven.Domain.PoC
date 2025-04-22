using AutoMapper;
using AutoMapper.QueryableExtensions;
using IdentityService.Api.Controllers.BaseControllerType;
using IdentityService.Api.Extensions;
using IdentityService.Application.DomainServices;
using IdentityService.Application.ViewModels;
using IdentityService.Application.ViewModels.ApplicationUsers.Request;
using IdentityService.Application.ViewModels.Permission;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using URF.Core.Abstractions;

namespace IdentityService.Api.Controllers;

[Route("api/permission")]
[ApiController]
[Produces("application/json")]
[AllowAnonymous]
public class PermissionController : BaseController
{
    #region Private props

    private readonly IPermissionService _permissionService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PermissionController> _logger;

    #endregion Private props

    #region Ctor

    public PermissionController(
        IMyUnitOfWork unitOfWork
        , IPermissionService permissionService
        , IMapper mapper
        , IHttpContextAccessor contextAccessor
        , IOptionsSnapshot<MyConfigurationValues> myConfigurationValues
        , IConfiguration configuration
        , ILogger<PermissionController> logger
    ) : base(unitOfWork, mapper, myConfigurationValues, null, contextAccessor, configuration)
    {
        _permissionService = permissionService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;

        _logger = logger;
    }

    #endregion Ctor

    #region Public Paginated get

    [HttpPost("paginated")]
    [SwaggerOperation(Summary = "Retrieves a paginated list of permissions based on search criteria.")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PagedResponse<PermissionViewModel>), Description = "Successful response with paginated list of permissions.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request parameters.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.")]
    [Produces(typeof(PagedResponse<PermissionViewModel>))]
    public async Task<IActionResult> Paginated([FromBody][Required] GetPaginatedPermissionRequest request,
        CancellationToken ct)
    {
        Log.Information("Starting paginated retrieval for permissions.");

        if (request.PageSize <= 0) request.PageSize = ConfigurationValues.DefaultPageSize;
        if (request.PageNumber <= 0) request.PageNumber = ConfigurationValues.DefaultPageNumber;

        try
        {
            var query = _permissionService.Queryable()
                .Where(u => EF.Functions.Like(u.Name, $"%{request.SearchCriteria}%") ||
                            u.Id.ToString().Contains(request.SearchCriteria) ||
                            EF.Functions.Like(u.Description, $"%{request.SearchCriteria}%"));

            // Apply dynamic ordering
            query = ApplyDynamicOrdering(query, request.OrderBy, request.SortOrder);

            var totalCount = await query.CountAsync(ct);

            var items = await query.AsNoTracking()
                .Paging(request.PageSize, request.PageNumber)
                .ProjectTo<PermissionViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);

            var (inactiveCount, deletedCount) = await CountInactiveOrDeletedPermissions(ct);

            var responseMessage = items.Any()
                ? $"Page [ {request.PageNumber} ] of [ {Math.Ceiling((double)totalCount / request.PageSize)} ], total records: [ {totalCount} ]. Inactive: {inactiveCount}, Deleted: {deletedCount}."
                : "No results were found by your search criteria.";

            var response = new PagedResponse<PermissionViewModel>
            {
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                ItemsCount = totalCount,
                Model = items,
                Message = responseMessage
            };

            Log.Information(
                "Paginated retrieval of permissions successful. Page: {PageNumber}, PageSize: {PageSize}, TotalCount: {TotalCount}, InactiveCount: {InactiveCount}, DeletedCount: {DeletedCount}.",
                request.PageNumber, request.PageSize, totalCount, inactiveCount, deletedCount);
            return Ok(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while processing paginated retrieval of permissions. Request: {@Request}",
                request);
            return new ObjectResult(HandleEnvironmentBasedResponse(ex, Configuration["ApplicationSettings:Environment"],
                nameof(Paginated), Configuration["ApplicationSettings:GenericErrorMessageForEndUser"],
                Request.Headers["X-Correlation-Id"], Request.Headers.Origin))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    private IQueryable<Permission> ApplyDynamicOrdering(IQueryable<Permission> query, string orderBy, string sortOrder)
    {
        var isDescending = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);

        return orderBy switch
        {
            "Name" => isDescending ? query.OrderByDescending(q => q.Name) : query.OrderBy(q => q.Name),
            "DateCreated" => isDescending
                ? query.OrderByDescending(q => q.DateCreated)
                : query.OrderBy(q => q.DateCreated),
            _ => query.OrderBy(q => q.Name)
        };
    }

    private async Task<(int inactiveCount, int deletedCount)> CountInactiveOrDeletedPermissions(CancellationToken ct)
    {
        var inactiveCount = await _permissionService.Queryable().CountAsync(p => !p.IsActive, ct);
        var deletedCount = await _permissionService.Queryable().CountAsync(p => p.Deleted, ct);

        return (inactiveCount, deletedCount);
    }

    #endregion Public Paginated get

    #region CUD

    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Deletes a specific permission by its ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Permission successfully deleted.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid permission ID supplied.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Permission not found or already deleted.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.")]
    [Produces(typeof(string))]
    public async Task<IActionResult> Delete([Required] long id, CancellationToken ct)
    {
        if (id <= 0)
        {
            Log.Warning("Attempt to delete permission with invalid Id: {PermissionId}", id);
            return BadRequest("Invalid permission Id.");
        }

        var existingPermission = await _permissionService
            .Queryable()
            .Where(p => p.Id == id)
            .AsNoTracking()
            .SingleOrDefaultAsync(ct);

        if (existingPermission == null)
        {
            Log.Information("Attempt to delete non-existing or already deleted permission with Id: {PermissionId}", id);
            return NotFound("Permission not found.");
        }

        try
        {
            _permissionService.Delete(existingPermission);
            var saveResult = await _unitOfWork.SaveChangesAsync(ct);

            if (saveResult > 0)
            {
                Log.Information("Permission with Id: {PermissionId} was successfully deleted.", id);
                return Ok($"Permission with Id {id} successfully deleted.");
            }

            Log.Error("Failed to delete permission with Id: {PermissionId}. Save changes resulted in no modifications.",
                id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                "An error occurred while deleting the permission.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Exception occurred while attempting to delete permission with Id: {PermissionId}.", id);
            return new ObjectResult(HandleEnvironmentBasedResponse(ex, Configuration["ApplicationSettings:Environment"],
                nameof(Delete), Configuration["ApplicationSettings:GenericErrorMessageForEndUser"],
                Request.Headers["X-Correlation-Id"], Request.Headers.Origin))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    [HttpPut("{id:long}")]
    [SwaggerOperation(Summary = "Updates a specific permission by its ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PermissionViewModel),
        Description = "Permission successfully updated.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid permission ID or mismatch between URL and body ID.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Permission not found.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.")]
    [Produces(typeof(PermissionViewModel))]
    public async Task<IActionResult> Edit([Required] long id, [FromBody][Required] UpdatePermissionRequest request)
    {
        if (id <= 0 || id != request.PermissionId)
        {
            Log.Warning("Edit attempt with mismatched or invalid permission Id: {PermissionId}, Request: {@Request}",
                id, request);
            return BadRequest("Invalid request data.");
        }

        var existingPermission = await _permissionService
            .Queryable()
            .Where(p => p.Id == id)
            .SingleOrDefaultAsync();

        if (existingPermission == null)
        {
            Log.Information("Attempt to edit non-existing or deleted permission with Id: {PermissionId}", id);
            return NotFound("Permission not found.");
        }

        try
        {
            _mapper.Map(request, existingPermission);
            _permissionService.Update(existingPermission);

            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (saveResult > 0)
            {
                Log.Information("Successfully edited permission with Id: {PermissionId}", id);
                var updatedPermission = await _permissionService
                    .Queryable()
                    .AsNoTracking()
                    .Where(p => p.Id == id)
                    .ProjectTo<PermissionViewModel>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync();

                return Ok(updatedPermission);
            }

            Log.Error("Failed to edit permission with Id: {PermissionId}. No changes saved.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update the permission.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Exception occurred while attempting to edit permission with Id: {PermissionId}.", id);
            return new ObjectResult(HandleEnvironmentBasedResponse(ex, Configuration["ApplicationSettings:Environment"],
                nameof(Edit), Configuration["ApplicationSettings:GenericErrorMessageForEndUser"],
                Request.Headers["X-Correlation-Id"], Request.Headers.Origin))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new permission entity.")]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(PermissionViewModel),
        Description = "The permission was successfully created.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.")]
    [Produces(typeof(PermissionViewModel))]
    public async Task<IActionResult> Insert([FromBody][Required] PermissionViewModel request)
    {
        Log.Information("Starting permission creation process.");

        if (request == null)
        {
            Log.Warning("Insert method received a null request.");
            return BadRequest("Request cannot be null.");
        }

        try
        {
            var domain = _mapper.Map<Permission>(request);
            _permissionService.Insert(domain);

            var result = await _unitOfWork.SaveChangesAsync();
            if (result > 0)
            {
                Log.Information("Permission created successfully.");
                var createdPermission = _mapper.Map<PermissionViewModel>(domain);
                return CreatedAtAction(nameof(Insert), new { id = domain.Id }, createdPermission);
            }

            Log.Error("Permission creation failed. No changes were made during save.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create the permission.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred during permission creation.");
            return new ObjectResult(HandleEnvironmentBasedResponse(ex, Configuration["ApplicationSettings:Environment"],
                nameof(Insert), Configuration["ApplicationSettings:GenericErrorMessageForEndUser"],
                Request.Headers["X-Correlation-Id"], Request.Headers.Origin))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    #endregion CUD

    #region Public get

    [HttpGet]
    [SwaggerOperation(Summary = "Retrieves all permissions.")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<PermissionViewModel>),
        Description = "Successfully retrieved all permissions.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.")]
    [Produces(typeof(IEnumerable<PermissionViewModel>))]
    public async Task<IActionResult> Get()
    {
        Log.Information("Retrieving all permissions.");

        try
        {
            var permissions = await _permissionService
                .Queryable()
                .AsNoTracking()
                .ProjectTo<PermissionViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            if (!permissions.Any())
            {
                Log.Information("No permissions found.");
                return Ok(new List<PermissionViewModel>());
            }

            return Ok(permissions);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving all permissions.");
            return new ObjectResult(HandleEnvironmentBasedResponse(ex, Configuration["ApplicationSettings:Environment"],
                nameof(Get), base.Configuration["ApplicationSettings:GenericErrorMessageForEndUser"],
                Request.Headers["X-Correlation-Id"], Request.Headers.Origin))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Retrieves a specific permission by its ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PermissionViewModel),
        Description = "Successfully retrieved the permission.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Permission not found.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.")]
    [Produces(typeof(PermissionViewModel))]
    public async Task<IActionResult> Get([Required] long id)
    {
        Log.Information("Retrieving permission with Id: {Id}", id);

        try
        {
            var permission = await _permissionService
                .Queryable()
                .AsNoTracking()
                .Where(p => p.Id == id)
                .ProjectTo<PermissionViewModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (permission == null)
            {
                Log.Information("Permission with Id: {Id} not found.", id);
                return NotFound("Permission not found.");
            }

            return Ok(permission);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving permission with Id: {Id}.", id);
            return new ObjectResult(HandleEnvironmentBasedResponse(ex, Configuration["ApplicationSettings:Environment"],
                nameof(Get), Configuration["ApplicationSettings:GenericErrorMessageForEndUser"],
                Request.Headers["X-Correlation-Id"], Request.Headers.Origin))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    #endregion Public get
}