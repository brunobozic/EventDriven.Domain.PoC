namespace IdentityService.Application.ViewModels;

public sealed record UpdateCountryRequest
{
    public long CountryId { get; set; }
    public string Name { get; set; }

    public string LocalName { get; set; }

    public string ISO_3166_ALPHA_2 { get; set; }

    public string ISO_3166_ALPHA_3 { get; set; }

    public string ISO_3166_NUMERIC { get; set; }

    public bool IsActive { get; set; }
}