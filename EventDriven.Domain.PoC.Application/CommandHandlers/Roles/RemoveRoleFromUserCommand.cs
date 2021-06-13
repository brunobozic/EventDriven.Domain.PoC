using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.Roles
{
    public class RemoveRoleFromUserCommand : CommandBase<ApplicationRoleAssignmentDto>
    {
        public RemoveRoleFromUserCommand(
            long userId
            , string roleName
        )
        {
            UserId = userId;
            RoleName = roleName;
        }

        public string RoleName { get; set; }

        public long UserId { get; set; }
        public User RemoverUser { get; set; }
    }
}