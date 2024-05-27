using IdentityService.Domain.DomainEntities.DomainExceptions;
using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;
using Serilog;
using SharedKernel.DomainContracts;
using SharedKernel.DomainCoreInterfaces;
using SharedKernel.DomainImplementations.BaseClasses;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;

public sealed class Role : BasicDomainEntity<long>, IAuditTrail, IAggregateRoot
{
    #region Public Properties

    public Guid RoleIdGuid { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    #endregion Public Properties

    #region Navigation Properties

    private readonly List<UserRole> _userRoles = new();
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.ToImmutableArray();

    private readonly List<RolePermission> _rolePermissions = new();
    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.ToImmutableArray();

    #endregion Navigation Properties

    #region FK

    public Guid? ReactivatedById { get; private set; }
    public Guid? DeactivatedById { get; private set; }
    public Guid? UndeletedById { get; private set; }
    public bool Deleted { get; private set; }

    #endregion FK

    #region Constructors

    private Role() { }

    public static Role NewDraft(string name, string description, User creatorUser, DateTimeOffset dateCreated)
    {
        ValidateParameters(name, description);

        var role = new Role
        {
            Name = name.Trim(),
            Description = description.Trim(),
            RoleIdGuid = Guid.NewGuid()
        };

        role.AddDomainEvent(new RoleCreatedDomainEvent(
            name, description, role.RoleIdGuid, creatorUser.Id, creatorUser.UserName, creatorUser.Email, dateCreated
        ));

        return role;
    }

    public static Role NewActiveDraft(string name, string description, DateTimeOffset from, DateTimeOffset to, User creatorUser)
    {
        ValidateParameters(name, description);

        var role = new Role
        {
            Name = name.Trim(),
            Description = description.Trim(),
            RoleIdGuid = Guid.NewGuid()
        };

        if (creatorUser is not null)
        {
            role.Activate(from, to, creatorUser);
            role.AssignCreatedBy(creatorUser);
            role.AddDomainEvent(new RoleCreatedDomainEvent(
                name, description, role.RoleIdGuid, creatorUser.Id, creatorUser.UserName, creatorUser.Email, DateTimeOffset.UtcNow
            ));
        }
        else
        {
            role.AddDomainEvent(new RoleCreatedDomainEvent(
                name, description, role.RoleIdGuid, null, "Seed", "Seed", DateTimeOffset.UtcNow
            ));
        }

        return role;
    }

    #endregion Constructors

    #region Public Methods

    public void SetDescription(string description) => Description = description;

    public bool IsDeactivated() => !Active;

    public bool IsExpired(DateTimeOffset date) => ActiveTo < date;

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => throw new NotImplementedException();

    public ValueTask<bool> AddPermission(RolePermission permission)
    {
        return ExecuteWithLoggingAsync(nameof(AddPermission), () =>
        {
            EnsureIsActive();
            _rolePermissions.Add(permission);
            Log.Information("Permission {PermissionId} added to role {RoleId}", permission.Id, Id);
            return new ValueTask<bool>(true);
        });
    }

    public ValueTask<bool> RemovePermission(RolePermission permission)
    {
        return ExecuteWithLoggingAsync(nameof(RemovePermission), () =>
        {
            EnsureIsActive();
            _rolePermissions.Remove(permission);
            Log.Information("Permission {PermissionId} removed from role {RoleId}", permission.Id, Id);
            return new ValueTask<bool>(true);
        });
    }

    #endregion Public Methods

    #region Helper Methods

    private static void ValidateParameters(string name, string description)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentNullException.ThrowIfNull(description, nameof(description));
    }

    private void EnsureIsActive()
    {
        if (!Active || IsExpired(DateTimeOffset.UtcNow) || Deleted)
            throw new DomainException($"The role [ {Name} ] is either inactive, expired, or deleted.");
    }

    private ValueTask<bool> ExecuteWithLoggingAsync(string methodName, Func<ValueTask<bool>> action)
    {
        using (SerilogHelper.PushMethodSpecificProperties(this, methodName))
        {
            try
            {
                Log.Information("{MethodName} called for role {RoleId}", methodName, Id);
                return action();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in {MethodName} for role {RoleId}", methodName, Id);
                throw;
            }
        }
    }

    #endregion Helper Methods
}
