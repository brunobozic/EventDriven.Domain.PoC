namespace IdentityService.Application.CommandsAndHandlers.Roles;

public class ApplicationRoleAssignmentDto
{
    public string RoleName { get; set; }
    public string AssigneeUserName { get; set; }
}