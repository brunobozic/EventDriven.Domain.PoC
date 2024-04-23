
using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace IdentityService.Application.DomainServices.RoleService;

public class RoleService : Service<Role>, IRoleService
{
    public RoleService(ITrackableRepository<Role> repository) : base(repository)
    {
    }
}