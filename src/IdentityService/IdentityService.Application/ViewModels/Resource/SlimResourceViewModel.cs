using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace IdentityService.Application.ViewModels.Resource;

[Serializable]
public class SlimResourceViewModel
{
    [JsonProperty] public string Name { get; set; }
    [JsonProperty] public List<string> Permissions { get; set; } = new();
}