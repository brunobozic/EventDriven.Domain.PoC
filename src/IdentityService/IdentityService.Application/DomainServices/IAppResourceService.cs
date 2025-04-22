using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using URF.Core.Abstractions.Services;

namespace IdentityService.Application.DomainServices;

public interface IAppResourceService : IService<Resource>
{
    // Add any custom business logic (methods) here
    // All methods in Service<TEntity> are ovverridable for any custom implementations
}