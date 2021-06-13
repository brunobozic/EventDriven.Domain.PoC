using System;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers;

namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationRoles
{
    public class RoleViewModel
    {
        public bool IsDraft { get; set; }

        // public Guid RoleId { get; set; }
        public DateTimeOffset? ActiveFrom { get; set; }
        public DateTimeOffset? ActiveTo { get; set; }
        public bool Deleted { get; set; } = false;
        public DateTimeOffset? DateDeleted { get; set; }


        public bool Active { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateModified { get; set; }
        public long? LastModifiedBy { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public UserViewModel CreatedBy { get; set; }
        public long Id { get; set; }
        public UserViewModel DeletedBy { get; set; }
        public UserViewModel ModifiedBy { get; set; }
        public UserViewModel ActivatedBy { get; set; }
    }
}