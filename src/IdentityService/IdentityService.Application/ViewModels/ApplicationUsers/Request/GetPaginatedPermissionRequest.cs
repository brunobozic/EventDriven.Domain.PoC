using System;

namespace IdentityService.Application.ViewModels.ApplicationUsers.Request;

public sealed record GetPaginatedPermissionRequest
{
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
    public string OrderBy { get; set; } = "Id";
    public string SearchCriteria { get; set; } = string.Empty;
    public string SortOrder { get; set; } = "ASC";
    public string SearchCreatedFrom { get; set; } = DateTimeOffset.UtcNow.AddYears(-1).ToString();
    public string SearchCreatedTo { get; set; } = DateTimeOffset.UtcNow.ToString();
}