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
      var webHostBuilder = new WebHostBuilder().UseStartup<StartupFake>();

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
      result.Should().Contain("Zoo Zuerich");
      result.Should().Contain("Polenmuseum - Schloss Rapperswil");
      result.Should().NotContain("Botanischer Garten der Universitaet Bern");
    }

    [Fact]
    public async Task TestGetStoreById() {
      HttpResponseMessage response = await _client.GetAsync("stores/10000000-0000-0000-0000-000000000000");
      await response.Content.ReadAsStringAsync();
      response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    #region Insert

    [Fact]
    public void TestInsertStore() {
      var newStore = GetNewStoreModel();
      var result = InsertStore(newStore, HttpStatusCode.OK);
      result.Should().Contain("\"id\":\"10000000-0000-0000-0000-000000000000\"");
      result.Should().Contain("\"secret\":\"00000000-0000-0000-0000-000000000001\"");
    }

    [Fact]
    public void TestInsertStoreNoName() {
      var newStore = GetNewStoreModel();
      newStore.Name = null;
      var result = InsertStore(newStore, HttpStatusCode.BadRequest);
      result.Should().Contain("The Name field is required.");
    }

    [Fact]
    public void TestInsertStoreNoStreet() {
      var newStore = GetNewStoreModel();
      newStore.Street = null;
      var result = InsertStore(newStore, HttpStatusCode.BadRequest);
      result.Should().Contain("The Street field is required.");
    }

    [Fact]
    public void TestInsertStoreNoZipCode() {
      var newStore = GetNewStoreModel();
      newStore.ZipCode = null;
      var result = InsertStore(newStore, HttpStatusCode.BadRequest);
      result.Should().Contain("The ZipCode field is required.");
    }


    [Fact]
    public void TestInsertStoreNoCity() {
      var newStore = GetNewStoreModel();
      newStore.City = null;
      var result = InsertStore(newStore, HttpStatusCode.BadRequest);
      result.Should().Contain("The City field is required.");
    }

    [Fact]
    public void TestInsertStoreMaxCapacityToLow() {
      var newStore = GetNewStoreModel();
      newStore.MaxCapacity = 0;
      var result = InsertStore(newStore, HttpStatusCode.BadRequest);
      result.Should().Contain("The field MaxCapacity must be between 1 and 2147483647.");
    }

    private static NewStoreModel GetNewStoreModel() {
      return new NewStoreModel() {
        Name = "Ikea",
        Street = "Zuercherstrasse 460",
        ZipCode = "9015",
        City = "St. Gallen",
        MaxCapacity = 201,
        StoreType = new StoreTypeModel {
          Id = "00000000-1000-0000-0000-000000000000",
          Description = "Detailhaendler"
        }
      };
    }

    private string InsertStore(NewStoreModel model, HttpStatusCode statusCode) {
      var json = JsonConvert.SerializeObject(model);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = _client.PostAsync("stores", content).Result;
      var result = response.Content.ReadAsStringAsync().Result;
      response.StatusCode.Should().Be(statusCode);
      return result;
    }

    #endregion

    #region Update

    [Fact]
    public void TestUpdateStore() {
      var store = GetStoreModel();
      var result = UpdateStore(store, HttpStatusCode.OK);
      result.Should().BeEmpty();
    }

    [Fact]
    public void TestUpdateStoreNoName() {
      var store = GetStoreModel();
      store.Name = null;
      var result = UpdateStore(store, HttpStatusCode.BadRequest);
      result.Should().Contain("The Name field is required.");
    }

    [Fact]
    public void TestUpdateStoreNoStreet() {
      var store = GetStoreModel();
      store.Street = null;
      var result = UpdateStore(store, HttpStatusCode.BadRequest);
      result.Should().Contain("The Street field is required.");
    }

    [Fact]
    public void TestUpdateStoreNoZipCode() {
      var store = GetStoreModel();
      store.ZipCode = null;
      var result = UpdateStore(store, HttpStatusCode.BadRequest);
      result.Should().Contain("The ZipCode field is required.");
    }

    [Fact]
    public void TestUpdateStoreNoCity() {
      var store = GetStoreModel();
      store.City = null;
      var result = UpdateStore(store, HttpStatusCode.BadRequest);
      result.Should().Contain("The City field is required.");
    }

    [Fact]
    public void TestUpdateStoreMaxCapacityToLow() {
      var store = GetStoreModel();
      store.MaxCapacity = 0;
      var result = UpdateStore(store, HttpStatusCode.BadRequest);
      result.Should().Contain("The field MaxCapacity must be between 1 and 2147483647.");
    }

    private static StoreModel GetStoreModel() {
      return new StoreModel() {
        Id = "10000000-0000-0000-0000-000000000000",
        Name = "Ikea",
        Street = "Zuercherstrasse 460",
        ZipCode = "9015",
        City = "St. Gallen",
        MaxCapacity = 201,
        CurrentCapacity = 50,
        Secret = "00000000-0000-0000-0000-000000000001",
        StoreType = new StoreTypeModel {
          Id = "00000000-1000-0000-0000-000000000000",
          Description = "Detailhaendler"
        }
      };
    }

    private string UpdateStore(StoreModel model, HttpStatusCode statusCode) {
      string json = JsonConvert.SerializeObject(model);
      StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage response = _client.PatchAsync("stores", content).Result;
      string result = response.Content.ReadAsStringAsync().Result;
      response.StatusCode.Should().Be(statusCode);
      return result;
    }

    #endregion

  }
}
