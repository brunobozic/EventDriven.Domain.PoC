namespace IdentityService.Application.ViewModels;

public sealed record GetPaginatedCountryRequest
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public string OrderBy { get; set; }
    public string SearchCriteria { get; set; } = string.Empty;
    public string SortOrder { get; set; } = "ASC";
}