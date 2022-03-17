using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers;
using System;

namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationRoles
{
    public class UserRoleViewModel
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
        public UserViewModel ModifiedBy { get; set; }
        public RoleViewModel Role { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public DateTimeOffset ActiveFrom { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public UserViewModel DeletedBy { get; set; }
        public bool Deleted { get; set; }
        public UserViewModel CreatedBy { get; set; }
        public string Description { get; set; }
        public bool IsDraft { get; set; }
        public string Name { get; set; }
        public DateTimeOffset? ActiveTo { get; set; }
        public DateTimeOffset? DateDeleted { get; set; }
        public DateTimeOffset? DateModified { get; set; }
    }
}