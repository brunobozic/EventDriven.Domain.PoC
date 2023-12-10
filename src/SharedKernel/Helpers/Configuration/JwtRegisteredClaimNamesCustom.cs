namespace SharedKernel.Helpers.Configuration;

public struct JwtRegisteredClaimNamesCustom
{
    public const string APPLICATION_TYPE = "APPLICATION_TYPE";
    public const string EMAIL = "EMAIL";
    public const string FULL_NAME = "FULL_NAME";
    public const string ORGANIZATION_UNITS = "ORGANIZATION_UNITS";
    public const string ROLES = "ROLES";
    public const string USER_ID = "USER_ID";
    public const string USERNAME = "USERNAME";
}

public class JwtClaimNameConstants
{
    public const string APPLICATION_ACTIONS_CLAIM_NAME = "ApplicationActions";
    public const string GUEST_CLAIM_NAME = "GuestRoleName";
    public const string ID_CLAIM_NAME = "Id";
    public const string Organization_UNIT = "OrganizationUnit";
    public const string ROLE_ID_CLAIM_NAME = "RoleId";
    public const string SUPERUSER_CLAIM_NAME = "SuperUserRoleName";
}