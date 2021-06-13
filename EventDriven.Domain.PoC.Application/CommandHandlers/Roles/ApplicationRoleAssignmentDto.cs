namespace EventDriven.Domain.PoC.Application.CommandHandlers.Roles
{
    public class ApplicationRoleAssignmentDto
    {
        public string RoleName { get; set; }
        public string AssigneeUserName { get; set; }
    }
}