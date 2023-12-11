﻿using System;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;

public class RoleDeletedDomainEvent : DomainEventBase
{
    public DateTimeOffset DateDeleted;
    public long DeletedByUserId;
    public string DeletionReason;
    public string DeletorEmail;
    public string DeletorUsername;
    public long RoleId;
    public string RoleName;

    public RoleDeletedDomainEvent(string roleName, long deletedByUserId, string deletedByUserEmail,
        string deletedByUserUserName, string deletionReason,
        DateTimeOffset dateDeleted)
    {
        RoleName = roleName;
        DeletedByUserId = deletedByUserId;
        DeletionReason = deletionReason;
        DateDeleted = dateDeleted;
        DeletorEmail = deletedByUserEmail;
        DeletorUsername = deletedByUserUserName;
    }

    public RoleDeletedDomainEvent(long id, long deletedByUserId, string deletedByUserEmail,
        string deletedByUserUserName, string deletionReason, DateTimeOffset dateDeleted)
    {
        RoleId = id;
        DeletedByUserId = deletedByUserId;
        DeletionReason = deletionReason;
        DateDeleted = dateDeleted;
        DeletorEmail = deletedByUserEmail;
        DeletorUsername = deletedByUserUserName;
    }
}