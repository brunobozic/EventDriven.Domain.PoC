using System;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents
{
    public class RoleRemovedFromUserDomainEvent : DomainEventBase
    {
        public DateTimeOffset? DateRemoved;
        public string Name;
        public string RemoverEmail;

        public long RemoverUserId;
        public string RemoverUsername;
        public long RoleId;
        public string UserEmail;
        public long UserId;
        public string UserName;

        public RoleRemovedFromUserDomainEvent(long userId, string userName, string userEmail, long roleId,
            string roleName, long removerUserId, string removerUserName, string removerEmail,
            DateTimeOffset dateRemoved)
        {
            UserId = userId;
            RoleId = roleId;
            RemoverUserId = removerUserId;
            DateRemoved = dateRemoved;
            Name = roleName;
            UserName = userName;
            UserEmail = userEmail;
            RemoverUsername = removerUserName;
            RemoverEmail = removerEmail;
        }
    }
}