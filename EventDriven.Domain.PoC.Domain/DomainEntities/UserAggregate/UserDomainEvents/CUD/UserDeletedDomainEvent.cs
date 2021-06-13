using System;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD
{
    public class UserDeletedDomainEvent : DomainEventBase
    {
        public UserDeletedDomainEvent(string email, long deletedByUserId, string deletionReason)
        {
            Email = email;
            DeletedByUserId = deletedByUserId;
            DeletionReason = deletionReason;
        }

        public UserDeletedDomainEvent(Guid id, long deletedByUserId, string deletionReason)
        {
            DeletedByUserId = deletedByUserId;
            DeletionReason = deletionReason;
        }

        public string Email { get; set; }
        public long DeletedByUserId { get; set; }
        public string DeletionReason { get; set; }
        public string UserName { get; set; }
        public UserId UserId { get; set; }
    }

    public class UserDeletedDomainByEmailEvent : DomainEventBase
    {
        public long DeletedByUserId;
        public string DeletionReason;

        public string Email;
        public long Id;
        public string UserName;

        public UserDeletedDomainByEmailEvent(string email, long deletedByUserId, string deletionReason)
        {
            Email = email;
            DeletedByUserId = deletedByUserId;
            DeletionReason = deletionReason;
        }

        public UserDeletedDomainByEmailEvent(long id, long deletedByUserId, string deletionReason)
        {
            Id = id;
            DeletedByUserId = deletedByUserId;
            DeletionReason = deletionReason;
        }
    }
}