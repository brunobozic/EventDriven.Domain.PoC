using SharedKernel.DomainBaseAbstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IdentityService.Domain.DomainEntities;

public class Permission : DomainEntity<long>
{
    [Required] public string Name { get; set; } // Create, Read, Update, Delete

    public string Description { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    public Guid? ActivatedByApplicationUserId { get; set; }
    public Guid? CreatedByApplicationUserId { get; set; }
    public Guid? DeletedByApplicationUserId { get; set; }
    public Guid? ModifiedByApplicationUserId { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        throw new System.NotImplementedException();
    }
}