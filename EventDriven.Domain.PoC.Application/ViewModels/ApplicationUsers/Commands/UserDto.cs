using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using System;
using System.Collections.Generic;

namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Commands
{
    public class UserDto
    {
        public Guid? Id { get; set; }
        public string UserName { get; set; }
        public List<Role> UserRoles { get; set; }
        public string Email { get; set; }
        public DateTimeOffset? ActiveTo { get; set; }
        public DateTime HasBeenVerified { get; set; }
        public string Status { get; set; }
    }
}