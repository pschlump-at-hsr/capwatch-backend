using CapWatchBackend.WebApi.Tests.Fakes;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Net.Http;
using Xunit;

namespace CapWatchBackend.WebApi.Tests.ControllerTests {
  public class StoresControllerTests {

    private readonly TestServer _server;
    private readonly HttpClient _client;

    public StoresControllerTests() {
      var webHostBuilder = new WebHostBuilder()
      .UseStartup<StartupFake>();

      _server = new TestServer(webHostBuilder);
      _client = _server.CreateClient();
    }

    [Fact]
    public async void TestControllerWorks() {
      var response = await _client.GetAsync("stores");
      response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async void TestGetStores() {
      HttpResponseMessage response = await _client.GetAsync("stores");
      var result = await response.Content.ReadAsStringAsync();
      result.Should().Contain("Migros St. Gallen");
      result.Should().Contain("Säntispark Bäder");
      result.Should().Contain("Interdiscount");
    }
  }
}
