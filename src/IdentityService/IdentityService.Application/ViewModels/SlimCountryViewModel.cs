using Newtonsoft.Json;
using System;

namespace IdentityService.Application.ViewModels;

[Serializable]
public class SlimCountryViewModel
{
    [JsonProperty] public long Id { get; set; }
    [JsonProperty] public string INT_NAME { get; set; }
    [JsonProperty] public string HR_NAME { get; set; }
    [JsonProperty] public string ISO_3166_ALPHA_2 { get; set; }
    [JsonProperty] public string ISO_3166_ALPHA_3 { get; set; }
    [JsonProperty] public string ISO_3166_NUMERIC { get; set; }
    [JsonProperty] public string LocalName { get; set; }
    [JsonProperty] public string InternationalName { get; set; }
    [JsonProperty] public DateTimeOffset DateCreated { get; set; }
}