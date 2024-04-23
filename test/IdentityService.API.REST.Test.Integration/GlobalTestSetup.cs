namespace IdentityService.API.REST.Test.Integration;

[SetUpFixture]
public class GlobalTestSetup
{
    [OneTimeSetUp]
    public void GlobalSetup()
    {
        // Set the ASPNETCORE_ENVIRONMENT to Test before the tests start
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test", EnvironmentVariableTarget.Process);
    }

    [OneTimeTearDown]
    public void GlobalTeardown()
    {
        // Cleanup, if needed
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", null, EnvironmentVariableTarget.Process);
    }
}