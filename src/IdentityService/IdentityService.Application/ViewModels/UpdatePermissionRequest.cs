namespace IdentityService.Application.ViewModels;

public sealed record UpdatePermissionRequest
{
    public long PermissionId { get; set; }
}