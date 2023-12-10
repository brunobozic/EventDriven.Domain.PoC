using System;
using IdentityService.Domain.DomainEntities.UserAggregate;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Application.CommandsAndHandlers.Roles;

public class AssignRoleToUserCommand : CommandBase<ApplicationRoleAssignmentDto>
{
    public AssignRoleToUserCommand(
        Guid userId
        , string roleName
    )
    {
        UserId = userId;
        RoleName = roleName;
    }

    public string RoleName { get; set; }

    public Guid UserId { get; set; }
    public string Origin { get; set; }
    public User RemoverUser { get; set; }
    public User AssignerUser { get; set; }
}