using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD
{
    public class UserUndeletedDomainEvent : DomainEventBase
    {
        public string Email;
        public long Id;
        public long UndeletedByUser;
        public string UndeletionReason;
        public UserId UserId;
        public string UserName;

        public UserUndeletedDomainEvent(string email, long undeletedByUser, string undeletionReason)
        {
            Email = email;
            UndeletedByUser = undeletedByUser;
            UndeletionReason = undeletionReason;
        }

        public UserUndeletedDomainEvent(long id, long undeletedByUser, string undeletionReason)
        {
            Id = id;
            UndeletedByUser = undeletedByUser;
            UndeletionReason = undeletionReason;
        }
    }

    public class UserUndeletedByEmailEvent : DomainEventBase
    {
        public string Email;
        public long Id;
        public long UndeletedByUser;
        public string UndeletionReason;
        public string UserName;

        public UserUndeletedByEmailEvent(string email, long undeletedByUser, string undeletionReason)
        {
            Email = email;
            UndeletedByUser = undeletedByUser;
            UndeletionReason = undeletionReason;
        }

        public UserUndeletedByEmailEvent(long id, long undeletedByUser, string undeletionReason)
        {
            Id = id;
            UndeletedByUser = undeletedByUser;
            UndeletionReason = undeletionReason;
        }
    }
}