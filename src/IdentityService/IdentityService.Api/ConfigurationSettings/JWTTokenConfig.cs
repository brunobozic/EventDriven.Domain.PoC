namespace IdentityService.Api.ConfigurationSettings;

public class JWTTokenConfig
{
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public char[] SecretKey { get; set; }
}