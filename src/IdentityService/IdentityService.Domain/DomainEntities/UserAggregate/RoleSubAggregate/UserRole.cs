using IdentityService.Domain.DomainEntities.DomainExceptions;
using Serilog;
using SharedKernel.DomainCoreInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;

public sealed class UserRole : BasicDomainEntity<long>, IAuditTrail
{
    #region Public Properties

    public Guid UserRoleGuid { get; private set; }

    #endregion Public Properties

    #region Navigation Properties

    public Role Role { get; private set; }
    public User User { get; private set; }

    #endregion Navigation Properties

    #region FK

    public Guid UserId { get; private set; }
    public long RoleId { get; private set; }
    public Guid? UndeletedById { get; private set; }
    public Guid? DeactivatedById { get; private set; }
    public Guid? ReactivatedById { get; private set; }

    #endregion FK

    #region Constructors

    private UserRole() { }

    public static UserRole NewDraft(User applicationUser, Role applicationRole, User activator)
    {
        ValidateParameters(applicationUser, applicationRole);

        var userRole = new UserRole
        {
            User = applicationUser,
            Role = applicationRole,
            UserRoleGuid = Guid.NewGuid()
        };
        userRole.AssignCreatedBy(activator);
        userRole.Activate(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(1), activator);

        return userRole;
    }

    public static UserRole NewActivatedDraft(User applicationUser, Role applicationRole, User activator)
    {
        ValidateParameters(applicationUser, applicationRole);

        var userRole = new UserRole
        {
            User = applicationUser,
            Role = applicationRole,
            UserRoleGuid = Guid.NewGuid(),
            ReactivatedById = activator.Id
        };
        userRole.AssignCreatedBy(activator);
        userRole.Activate(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(1), activator);

        return userRole;
    }

    public static UserRole NewInactiveDraft(User applicationUser, Role applicationRole, User activator)
    {
        ValidateParameters(applicationUser, applicationRole);

        var userRole = new UserRole
        {
            User = applicationUser,
            Role = applicationRole,
            UserRoleGuid = Guid.NewGuid()
        };
        userRole.AssignCreatedBy(activator);

        return userRole;
    }

    #endregion Constructors

    #region Public Methods

    public bool IsDeactivated() => !Active;

    public bool IsExpired(DateTimeOffset theDate) => ActiveTo < theDate;

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => throw new NotImplementedException();

    public async ValueTask<bool> ActivateUserRole(DateTimeOffset from, DateTimeOffset to, User activator)
    {
        return await ExecuteWithLoggingAsync(nameof(ActivateUserRole), async () =>
        {
            EnsureIsActive();
            Activate(from, to, activator);
            Log.Information("User role {UserRoleId} activated", Id);
            return true;
        });
    }

    public async ValueTask<bool> DeactivateUserRole(User deactivator, string reason)
    {
        return await ExecuteWithLoggingAsync(nameof(DeactivateUserRole), async () =>
        {
            Deactivate(deactivator, reason);
            Log.Information("User role {UserRoleId} deactivated", Id);
            return true;
        });
    }

    #endregion Public Methods

    #region Helper Methods

    private static void ValidateParameters(User user, Role role)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(role, nameof(role));
    }

    private void EnsureIsActive()
    {
        if (!Active || IsExpired(DateTimeOffset.UtcNow) || Role.Deleted)
            throw new DomainException($"The user role for [ {User.UserName} ] and role [ {Role.Name} ] is either inactive, expired, or deleted.");
    }

    private async ValueTask<bool> ExecuteWithLoggingAsync(string methodName, Func<Task<bool>> action)
    {
        using (SerilogHelper.PushMethodSpecificProperties(this, methodName))
        {
            try
            {
                Log.Information("{MethodName} called for user role {UserRoleId}", methodName, Id);
                return await action();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in {MethodName} for user role {UserRoleId}", methodName, Id);
                throw;
            }
        }
    }

    #endregion Helper Methods
}
