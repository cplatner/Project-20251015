using Microsoft.AspNetCore.Mvc.Testing;
using StargateAPI.Controllers;

namespace StargateAPI.IntegrationTests;

public class WebTestFactory : WebApplicationFactory<AstronautDutyController>, IAsyncLifetime
{
    public HttpClient HttpClient { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        HttpClient = CreateClient();
    }

    public new async Task DisposeAsync()
    {
    }
}