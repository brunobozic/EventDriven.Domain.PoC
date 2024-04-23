using IdentityService.Application.ViewModels.Permission;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace IdentityService.Application.ViewModels.Resource;

[Serializable]
public class ResourceViewModel
{
    [JsonProperty] public string Name { get; set; }
    [JsonProperty] public string Description { get; set; }
    [JsonProperty] public DateTimeOffset DateCreated { get; set; }
    [JsonProperty] public bool IsActive { get; set; }
    [JsonProperty] public bool Deleted { get; set; }
    [JsonProperty] public List<PermissionViewModel> Permissions { get; set; }
    [JsonProperty] public List<RolePermissionViewModel> RolePermissions { get; set; }
    [JsonProperty] public long DepartmentId { get; set; }
}