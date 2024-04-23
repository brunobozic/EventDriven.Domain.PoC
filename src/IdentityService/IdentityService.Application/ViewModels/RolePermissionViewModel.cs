using IdentityService.Application.ViewModels.Permission;
using IdentityService.Application.ViewModels.Resource;

namespace IdentityService.Application.ViewModels;

public class RolePermissionViewModel
{
    public long RoleId { get; set; }
    public long PermissionId { get; set; }
    public long ResourceId { get; set; }
    public ResourceViewModel Resource { get; set; }
    public PermissionViewModel Permission { get; set; }
    public bool? IsActive { get; set; }
    public long DepartmentId { get; set; }
}