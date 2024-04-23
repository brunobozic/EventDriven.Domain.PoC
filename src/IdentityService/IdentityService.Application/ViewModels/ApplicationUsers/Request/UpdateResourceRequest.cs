namespace IdentityService.Application.ViewModels.ApplicationUsers.Request;

public sealed record UpdateResourceRequest
{
    public long ResourceId { get; set; }
}