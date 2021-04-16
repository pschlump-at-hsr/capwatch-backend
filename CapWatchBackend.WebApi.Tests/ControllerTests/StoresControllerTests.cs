using CapWatchBackend.Domain.Entities;
using CapWatchBackend.WebApi.Models;
using CapWatchBackend.WebApi.Tests.Fakes;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
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
      result.Should().Contain("Ikea");
      result.Should().Contain("Zoo Zürich");
      result.Should().Contain("Polenmuseum - Schloss Rapperswil");
      result.Should().NotContain("Botanischer Garten der Universität Bern");
    }

    [Fact]
    public async void TestGetStoreById() {
      HttpResponseMessage response = await _client.GetAsync("stores/1");
      await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async void TestInsertStore() {
      var newStore = new StoreNew() { Name = "Ikea", Street = "Zürcherstrasse 460" , ZipCode = "9015", City = "St. Gallen", MaxCapacity = 201};
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PostAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      result.Should().Contain("\"id\":10");
      result.Should().Contain("\"secret\":\"9c9cee44-c839-48f2-b54e-236d95fe5d7f\"");
    }

    [Fact]
    public async void TestInsertStoreNoName() {
      var newStore = new StoreNew() { Name = null, Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", MaxCapacity = 201 };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PostAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The Name field is required.");
    }

    [Fact]
    public async void TestInsertStoreNoStreet() {
      var newStore = new StoreNew() { Name = "Ikea", Street = null, ZipCode = "9015", City = "St. Gallen", MaxCapacity = 201 };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PostAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The Street field is required.");
    }

    [Fact]
    public async void TestInsertStoreNoZipCode() {
      var newStore = new StoreNew() { Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = null, City = "St. Gallen", MaxCapacity = 201 };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PostAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The ZipCode field is required.");
    }


    [Fact]
    public async void TestInsertStoreNoCity() {
      var newStore = new StoreNew() { Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = null, MaxCapacity = 201 };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PostAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The City field is required.");
    }

    [Fact]
    public async void TestInsertStoreMaxCapacityToLow() {
      var newStore = new StoreNew() { Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", MaxCapacity = 0 };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PostAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The field MaxCapacity must be between 1 and 2147483647.");
    }

    [Fact]
    public async void TestUpdateStore() {
      var newStore = new StoreModel() { Id = 1, Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", MaxCapacity = 201, CurrentCapacity = 50, Secret = "9c9cee44-c839-48f2-b54e-236d95fe5d7f" };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PatchAsync("stores", content);
      await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async void TestUpdateStoreNoName() {
      var newStore = new StoreModel() { Id = 1, Name = null, Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", MaxCapacity = 201, CurrentCapacity = 50, Secret = "9c9cee44-c839-48f2-b54e-236d95fe5d7f" };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PatchAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The Name field is required.");
    }

    [Fact]
    public async void TestUpdateStoreNoStreet() {
      var newStore = new StoreModel() { Id = 1, Name = "Ikea", Street = null, ZipCode = "9015", City = "St. Gallen", MaxCapacity = 201, CurrentCapacity = 50, Secret = "9c9cee44-c839-48f2-b54e-236d95fe5d7f" };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PatchAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The Street field is required.");
    }

    [Fact]
    public async void TestUpdateStoreNoZipCode() {
      var newStore = new StoreModel() { Id = 1, Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = null, City = "St. Gallen", MaxCapacity = 201, CurrentCapacity = 50, Secret = "9c9cee44-c839-48f2-b54e-236d95fe5d7f" };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PatchAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The ZipCode field is required.");
    }

    [Fact]
    public async void TestUpdateStoreNoCity() {
      var newStore = new StoreModel() { Id = 1, Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = null, MaxCapacity = 201, CurrentCapacity = 50, Secret = "9c9cee44-c839-48f2-b54e-236d95fe5d7f" };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PatchAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The City field is required.");
    }

    [Fact]
    public async void TestUpdateStoreMaxCapacityToLow() {
      var newStore = new StoreModel() { Id = 1, Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", MaxCapacity = 0, CurrentCapacity = 50, Secret = "9c9cee44-c839-48f2-b54e-236d95fe5d7f" };
      var json = JsonConvert.SerializeObject(newStore);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await _client.PatchAsync("stores", content);
      var result = await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Should().Contain("The field MaxCapacity must be between 1 and 2147483647.");
    }
  }
}
