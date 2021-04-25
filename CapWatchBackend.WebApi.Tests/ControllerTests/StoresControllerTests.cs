using CapWatchBackend.WebApi.Models;
using CapWatchBackend.WebApi.Tests.Fakes;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
    public async Task TestControllerWorks() {
      var response = await _client.GetAsync("stores");
      response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task TestGetStores() {
      HttpResponseMessage response = _client.GetAsync("stores").Result;
      var result = await response.Content.ReadAsStringAsync();
      result.Should().Contain("Ikea");
      result.Should().Contain("Zoo Zürich");
      result.Should().Contain("Polenmuseum - Schloss Rapperswil");
      result.Should().NotContain("Botanischer Garten der Universität Bern");
    }

    [Fact]
    public async Task TestGetStoreById() {
      HttpResponseMessage response = await _client.GetAsync("stores/9c9cee44-c839-48f2-b54e-235d95fe5d7f");
      await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task TestInsertStore() {
      var newStore = new NewStoreModel() { Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", MaxCapacity = 201, StoreType = new StoreTypeModel { Id = "c73e9c5f-de5c-479a-b116-7ee1b93ab4f9", Description = "Detailhändler" } };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PostAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      result.Should().Contain("\"id\":\"9c9cee44-c839-48f2-b54e-246d95fe5d7f\"");
      result.Should().Contain("\"secret\":\"9c9cee44-c839-48f2-b54e-236d95fe5d7f\"");
    }

    [Fact]
    public async Task TestInsertStoreNoName() {
      var newStore = new NewStoreModel() { Name = null, Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", MaxCapacity = 201 };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PostAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The Name field is required.");
    }

    [Fact]
    public async Task TestInsertStoreNoStreet() {
      var newStore = new NewStoreModel() { Name = "Ikea", Street = null, ZipCode = "9015", City = "St. Gallen", MaxCapacity = 201 };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PostAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The Street field is required.");
    }

    [Fact]
    public async Task TestInsertStoreNoZipCode() {
      var newStore = new NewStoreModel() { Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = null, City = "St. Gallen", MaxCapacity = 201 };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PostAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The ZipCode field is required.");
    }


    [Fact]
    public async Task TestInsertStoreNoCity() {
      var newStore = new NewStoreModel() { Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = null, MaxCapacity = 201 };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PostAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The City field is required.");
    }

    [Fact]
    public async Task TestInsertStoreMaxCapacityToLow() {
      var newStore = new NewStoreModel() { Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", MaxCapacity = 0 };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PostAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The field MaxCapacity must be between 1 and 2147483647.");
    }

    [Fact]
    public async Task TestUpdateStore() {
      var newStore = new StoreModel() { Id = "9c9cee44-c839-48f1-b54e-237d95fe5d7f", Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", MaxCapacity = 201, CurrentCapacity = 50, Secret = "9c9cee44-c839-48f2-b54e-236d95fe5d7f", StoreType = new StoreTypeModel { Id = "c73e9c5f-de5c-479a-b116-7ee1b93ab4f9", Description = "Detailhändler" } };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PatchAsync("stores", content);
      await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task TestUpdateStoreNoName() {
      var newStore = new StoreModel() { Id = "9c9cee44-c839-48f1-b54e-237d95fe5d7f", Name = null, Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", MaxCapacity = 201, CurrentCapacity = 50, Secret = "9c9cee44-c839-48f2-b54e-236d95fe5d7f" };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PatchAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The Name field is required.");
    }

    [Fact]
    public async Task TestUpdateStoreNoStreet() {
      var newStore = new StoreModel() { Id = "9c9cee44-c839-48f1-b54e-237d95fe5d7f", Name = "Ikea", Street = null, ZipCode = "9015", City = "St. Gallen", MaxCapacity = 201, CurrentCapacity = 50, Secret = "9c9cee44-c839-48f2-b54e-236d95fe5d7f" };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PatchAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The Street field is required.");
    }

    [Fact]
    public async Task TestUpdateStoreNoZipCode() {
      var newStore = new StoreModel() { Id = "9c9cee44-c839-48f1-b54e-237d95fe5d7f", Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = null, City = "St. Gallen", MaxCapacity = 201, CurrentCapacity = 50, Secret = "9c9cee44-c839-48f2-b54e-236d95fe5d7f" };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PatchAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The ZipCode field is required.");
    }

    [Fact]
    public async Task TestUpdateStoreNoCity() {
      var newStore = new StoreModel() { Id = "9c9cee44-c839-48f1-b54e-237d95fe5d7f", Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = null, MaxCapacity = 201, CurrentCapacity = 50, Secret = "9c9cee44-c839-48f2-b54e-236d95fe5d7f" };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PatchAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The City field is required.");
    }

    [Fact]
    public async Task TestUpdateStoreMaxCapacityToLow() {
      var newStore = new StoreModel() { Id = "9c9cee44-c839-48f1-b54e-237d95fe5d7f", Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", MaxCapacity = 0, CurrentCapacity = 50, Secret = "9c9cee44-c839-48f2-b54e-236d95fe5d7f" };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PatchAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The field MaxCapacity must be between 1 and 2147483647.");
    }
  }
}
