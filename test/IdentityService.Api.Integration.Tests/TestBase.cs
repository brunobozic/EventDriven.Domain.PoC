using System.Net.Http;
using Xunit;
using IdentityService.Tests.Integration.Fixtures;
using IdentityService.Api;

namespace IdentityService.Tests.Integration.Tests
{
    public class TestBase : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        protected readonly CustomWebApplicationFactory<Program> Factory;
        protected readonly HttpClient Client;

        public TestBase(CustomWebApplicationFactory<Program> factory)
        {
            Factory = factory;
            Client = Factory.CreateClient();
        }
    }
}
