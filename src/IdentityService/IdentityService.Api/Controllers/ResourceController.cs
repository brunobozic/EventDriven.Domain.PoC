using AutoMapper;
using AutoMapper.QueryableExtensions;
using IdentityService.Api.Controllers.BaseControllerType;
using IdentityService.Application.DomainServices;
using IdentityService.Application.ViewModels.ApplicationUsers.Request;
using IdentityService.Application.ViewModels.Resource;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Domain.DomainEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using SharedKernel.Helpers.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using URF.Core.Abstractions;

namespace IdentityService.Api.Controllers;

[Route("api/resource")]
[ApiController]
[Produces("application/json")]
[AllowAnonymous]
public class ResourceController : BaseController
{
    #region Private props

    private readonly IAppResourceService _resourceService;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;
    private readonly ILogger<ResourceController> _logger;

    #endregion Private props

    #region Ctor

    public ResourceController(
        IMyUnitOfWork unitOfWork
        , IAppResourceService resourceService
        , IMapper mapper
        , IHttpContextAccessor contextAccessor
        , IOptionsSnapshot<MyConfigurationValues> myConfigurationValues
        , IConfiguration configuration
        , ILogger<ResourceController> logger
    ) : base(unitOfWork, mapper, myConfigurationValues, null, contextAccessor, configuration)
    {
        _resourceService = resourceService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;

        _logger = logger;
    }

    #endregion Ctor

    #region Public get

    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Retrieves a specific resource by its ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ResourceViewModel),
        Description = "Successfully retrieved the resource.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Resource not found.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.")]
    [Produces(typeof(ResourceViewModel))]
    public async Task<IActionResult> Get([Required] long id)
    {
        Log.Information("Retrieving resource with Id: {Id}", id);

        try
        {
            var resource = await _resourceService
                .Queryable()
                .AsNoTracking()
                .Where(r => r.Id == id)
                .ProjectTo<ResourceViewModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (resource == null)
            {
                Log.Information("Resource with Id: {Id} not found.", id);
                return NotFound("Resource not found.");
            }

            return Ok(resource);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving resource with Id: {Id}.", id);
            return new ObjectResult(HandleEnvironmentBasedResponse(ex, Configuration["ApplicationSettings:Environment"],
                nameof(Get), Configuration["ApplicationSettings:GenericErrorMessageForEndUser"],
                Request.Headers["X-Correlation-Id"], Request.Headers.Origin))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    #endregion Public get

    #region CUD

    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Deletes a specific resource by its ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, Description = "Resource successfully deleted.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid resource ID supplied.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Resource not found or already deleted.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.")]
    public async Task<IActionResult> Delete([Required] long id)
    {
        Log.Information("Attempting to delete resource with Id: {Id}", id);

        if (id <= 0)
        {
            Log.Warning("Delete method received an invalid Id: {Id}", id);
            return BadRequest("Invalid resource ID.");
        }

        var existingResource = await _resourceService
            .Queryable()
            .Where(resource => resource.Id == id)
            .AsNoTracking()
            .SingleOrDefaultAsync();

        if (existingResource == null)
        {
            Log.Information("Resource with Id: {Id} not found or already deleted.", id);
            return NotFound("Resource not found.");
        }

        try
        {
            _resourceService.Delete(existingResource);
            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (saveResult > 0)
            {
                Log.Information("Resource with Id: {Id} successfully deleted.", id);
                return Ok($"Resource with Id {id} successfully deleted.");
            }

            Log.Error("Failed to delete resource with Id: {Id}. No changes saved to the database.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete the resource.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while attempting to delete resource with Id: {Id}.", id);
            return new ObjectResult(HandleEnvironmentBasedResponse(ex, Configuration["ApplicationSettings:Environment"],
                nameof(Delete), Configuration["ApplicationSettings:GenericErrorMessageForEndUser"],
                Request.Headers["X-Correlation-Id"], Request.Headers.Origin))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    [HttpPut("{id:long}")]
    [SwaggerOperation(Summary = "Updates a specific resource by its ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ResourceViewModel),
        Description = "Resource successfully updated.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid resource ID or mismatch between URL and body ID.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Resource not found.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.")]
    [Produces(typeof(ResourceViewModel))]
    public async Task<IActionResult> Edit([Required] long id, [FromBody][Required] UpdateResourceRequest request)
    {
        Log.Information("Attempting to update resource with Id: {Id}", id);

        if (id <= 0 || id != request.ResourceId)
        {
            Log.Warning("Edit attempt with mismatched or invalid resource Id: {ResourceId}, Request: {@Request}", id,
                request);
            return BadRequest("Invalid request data.");
        }

        var existingResource = await _resourceService
            .Queryable()
            .Where(resource => resource.Id == id)
            .SingleOrDefaultAsync();

        if (existingResource == null)
        {
            Log.Information("Resource with Id: {Id} not found.", id);
            return NotFound("Resource not found.");
        }

        try
        {
            _mapper.Map(request, existingResource);
            _resourceService.Update(existingResource);

            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (saveResult > 0)
            {
                Log.Information("Resource with Id: {Id} successfully updated.", id);
                var updatedResource = _mapper.Map<ResourceViewModel>(existingResource);
                return Ok(updatedResource);
            }

            Log.Error("Failed to update resource with Id: {Id}. No changes saved to the database.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update the resource.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while attempting to update resource with Id: {Id}.", id);
            return new ObjectResult(HandleEnvironmentBasedResponse(ex, Configuration["ApplicationSettings:Environment"],
                nameof(Edit), Configuration["ApplicationSettings:GenericErrorMessageForEndUser"],
                Request.Headers["X-Correlation-Id"], Request.Headers.Origin))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new resource entity.")]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(ResourceViewModel),
        Description = "The resource was successfully created.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.")]
    public async Task<IActionResult> Insert([FromBody][Required] ResourceViewModel request)
    {
        Log.Information("Starting the resource creation process.");

        if (request == null)
        {
            Log.Warning("Insert method received a null request.");
            return BadRequest("Request cannot be null.");
        }

        try
        {
            var domain = _mapper.Map<Resource>(request);
            _resourceService.Insert(domain);

            var result = await _unitOfWork.SaveChangesAsync();
            if (result > 0)
            {
                Log.Information("Resource created successfully.");
                var createdResource = _mapper.Map<ResourceViewModel>(domain);
                return CreatedAtAction(nameof(Insert), new { id = domain.Id }, createdResource);
            }

            Log.Error("Resource creation failed. No changes were made during save.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create the resource.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred during resource creation.");
            return new ObjectResult(HandleEnvironmentBasedResponse(ex, Configuration["ApplicationSettings:Environment"],
                nameof(Insert), Configuration["ApplicationSettings:GenericErrorMessageForEndUser"],
                Request.Headers["X-Correlation-Id"], Request.Headers.Origin))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    #endregion CUD
}