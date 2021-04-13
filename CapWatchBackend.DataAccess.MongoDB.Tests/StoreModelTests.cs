using CapWatchBackend.DataAccess.MongoDB.Repositories;
using CapWatchBackend.Domain.Entities;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace CapWatchBackend.DataAccess.MongoDB.Tests {

  public class StoreModelTests {

    StoreRepository sm;
    public StoreModelTests() {
      sm = new StoreRepository("mongodb://capwusr:capwusr123@db:27017/admin");
      sm.DeleteAllStores();
    }

    [Fact]
    public void TestAddStore() {
      Store store = new Store { Id = 5, Name = "Botanischer Garten der Universität Bern", Street = "Altenbergrain 21", ZipCode = "3013", City = "Bern", CurrentCapacity = 103, MaxCapacity = 103 };
      sm.AddStoreAsync(store).Wait();
      sm.GetStore(5).Name.Should().Be("Botanischer Garten der Universität Bern");
      sm.DeleteStore(store);
    }

    [Fact]
    public void TestGetStores() {
      List<Store> stores = new List<Store>();
      stores.Add(new Store { Id = 1, Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", CurrentCapacity = 135, MaxCapacity = 201 });
      stores.Add(new Store { Id = 2, Name = "Zoo Zürich", Street = "Zürichbergstrasse 221", ZipCode = "8044", City = "Zürich", CurrentCapacity = 487, MaxCapacity = 1125 });
      stores.Add(new Store { Id = 3, Name = "Polenmuseum - Schloss Rapperswil", Street = "Schloss", ZipCode = "8640", City = "Raperswil-Jona", CurrentCapacity = 11, MaxCapacity = 62 });
      stores.Add(new Store { Id = 4, Name = "Alpamare", Street = "Gwattstrasse 12", ZipCode = "8808", City = "Pfäffikon", CurrentCapacity = 152, MaxCapacity = 612 });
      sm.AddStoresAsync(stores).Wait();
      stores = (List<Store>)sm.GetStores();
      stores.Count.Should().Be(4);
      foreach (var store in stores) {
        store.Name.Should().NotBeNullOrEmpty();
      }
      sm.DeleteAllStores();
    }

    [Fact]
    public void TestUpdateStore() {
      Store store = new Store { Id = 5, Name = "Botanischer Garten der Universität Bern", Street = "Altenbergrain 21", ZipCode = "3013", City = "Bern", CurrentCapacity = 103, MaxCapacity = 103 };
      sm.AddStoreAsync(store).Wait();
      store = new Store { Id = 5, Name = "Botanischer Garten St. Gallen", Street = "Keine Ahnung", ZipCode = "9008", City = "Bern", CurrentCapacity = 103, MaxCapacity = 103 };
      sm.UpdateStoreAsync(store).Wait();
      sm.GetStore(5).Name.Should().Be("Botanischer Garten St. Gallen");
    }

  }
}
