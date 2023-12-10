using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Domain.DomainEntities.DomainExceptions;
using IdentityService.Domain.DomainEntities.UserAggregate;
using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using Microsoft.EntityFrameworkCore;
using SharedKernel.DomainContracts;
using URF.Core.Abstractions.Trackable;

namespace IdentityService.Application.CommandsAndHandlers.Roles;

public class
    RemoveRoleFromUserCommandHandler : ICommandHandler<RemoveRoleFromUserCommand, ApplicationRoleAssignmentDto>
{
    public RemoveRoleFromUserCommandHandler(
        IMyUnitOfWork unitOfWork,
        ITrackableRepository<User> userRepository,
        ITrackableRepository<Role> roleRepository,
        User removerUser
    )
    {
        UnitOfWork = unitOfWork;
        UserRepository = userRepository;
        RoleRepository = roleRepository;
        RemoverUser = removerUser;
    }

    public User RemoverUser { get; set; }

    private IMyUnitOfWork UnitOfWork { get; }
    private ITrackableRepository<Role> RoleRepository { get; }
    private ITrackableRepository<User> UserRepository { get; }

    public async Task<ApplicationRoleAssignmentDto> Handle(RemoveRoleFromUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await UserRepository
            .Queryable()
            .Where(user => user.Id == command.UserId)
            .SingleOrDefaultAsync(cancellationToken);

        if (user == null)
            throw new DomainException("Application user not found by requested Id of: [ " + command.Id + " ]");

        var role = await RoleRepository
            .Queryable()
            .Where(role => role.Name == command.RoleName)
            .SingleOrDefaultAsync(cancellationToken);

        if (role == null)
            throw new DomainException("Application role not found by requested name of: [ " + command.RoleName +
                                      " ]");

        user.RemoveRole(role, RemoverUser);

        UserRepository.Update(user);

        var res = await UnitOfWork.SaveChangesAsync(cancellationToken);

        return new ApplicationRoleAssignmentDto { RoleName = role.Name, AssigneeUserName = user.UserName };
    }
}