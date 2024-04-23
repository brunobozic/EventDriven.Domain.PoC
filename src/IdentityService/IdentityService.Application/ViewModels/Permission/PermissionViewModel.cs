using IdentityService.Application.ViewModels.Resource;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace IdentityService.Application.ViewModels.Permission;

[Serializable]
public class PermissionViewModel
{
    [JsonProperty] public string Name { get; set; }
    [JsonProperty] public List<ResourceViewModel> Resources { get; set; } = new List<ResourceViewModel>();
    [JsonProperty] public string Description { get; set; }
    [JsonProperty] public DateTimeOffset DateCreated { get; set; }
    [JsonProperty] public bool IsActive { get; set; }
    [JsonProperty] public List<string> ResourcePermissionGrantedViaRole { get; set; }
    [JsonProperty]
    public ICollection<RolePermissionViewModel> RolePermissions { get; set; } = new List<RolePermissionViewModel>();
    [JsonProperty] public bool IsDeleted { get; set; }
}