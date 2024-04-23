using IdentityService.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace IdentityService.API.REST.Test.Integration;


[TestFixture]
public class TransformerTestStandardsControllerTests : TestBase, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private HttpClient _client;

    public TransformerTestStandardsControllerTests()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [SetUp]
    public void SetUp()
    {
        // Reinitialize the client before each test if needed
        _client = _factory.CreateClient();
    }
    public void Dispose()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    [Test]
    public async Task Echo()
    {
        var echo = "echo";

        var jsonRequest = JsonConvert.SerializeObject(echo);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        Client.DefaultRequestHeaders
            .Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json")); // Accept JSON responses

        // Act: Send the POST request to the specified route
        var response = await Client.PostAsync($"/api/echo", content);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), "Expected a 404 Not Found response");
    }
}
