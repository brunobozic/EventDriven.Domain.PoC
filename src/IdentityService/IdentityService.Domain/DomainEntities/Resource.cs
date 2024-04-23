using IdentityService.Domain.DomainEntities.UserAggregate;
using SharedKernel.DomainBaseAbstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Domain.DomainEntities;

public class Resource : DomainEntity<long>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ControllerName { get; set; }
    public string ControllerActionName { get; set; }
    public string ReportName { get; set; }

    public ResourceType ResourceType { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    // Foreign keys
    public Guid? ActivatedByApplicationUserId { get; set; }
    public Guid? CreatedByApplicationUserId { get; set; }
    public Guid? DeletedByApplicationUserId { get; set; }
    public Guid? ModifiedByApplicationUserId { get; set; }

    // Navigation properties
    public User ActivatedBy { get; set; }
    public User CreatedBy { get; set; }
    public User DeletedBy { get; set; }
    public User ModifiedBy { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        throw new System.NotImplementedException();
    }
}