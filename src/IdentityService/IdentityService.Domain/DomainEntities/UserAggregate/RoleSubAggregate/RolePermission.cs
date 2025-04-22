using SharedKernel.DomainBaseAbstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;

public class RolePermission : DomainEntity<long>
{
    public long RoleId { get; set; }
    public Role Role { get; set; }

    public long PermissionId { get; set; }
    public Permission Permission { get; set; }

    public long ResourceId { get; set; }
    public Resource Resource { get; set; }
    public bool TestData { get; set; } = false;
    public User DeletedBy { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        throw new System.NotImplementedException();
    }
}