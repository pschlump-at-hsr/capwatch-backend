using CapWatchBackend.DataAccess.MongoDB.Repositories;
using CapWatchBackend.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace CapWatchBackend.DataAccess.MongoDB.Tests {

  public class StoreModelTests {

    private readonly StoreRepository _storeRepository;
    public StoreModelTests() {
      _storeRepository = new StoreRepository("mongodb://capwusr:capwusr123@localhost:27017/admin");
      _storeRepository.DeleteAllStores();
    }

    [Fact]
    public void TestAddStore() {
      Store store = new Store { Name = "Botanischer Garten der Universität Bern", Secret = new Guid("57bd0e44-4032-4a9c-b48e-c89d990bcd6f"), Street = "Altenbergrain 21", ZipCode = "3013", City = "Bern", CurrentCapacity = 103, MaxCapacity = 103 };
      var res = _storeRepository.AddStore(store);
      res.Id.Should().BeGreaterThan(0);
      _storeRepository.GetStore(res.Id).Name.Should().Be("Botanischer Garten der Universität Bern");
    }

    [Fact]
    public void TestGetStores() {
      List<Store> stores = new List<Store> {
        new Store { Name = "Ikea", Secret = new Guid("02a4299e-f472-41d1-9468-df369bd30872"), Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", CurrentCapacity = 135, MaxCapacity = 201 },
        new Store { Name = "Zoo Zürich", Secret = new Guid("9e573805-f5b4-49bb-bbaf-844b44ea3942"), Street = "Zürichbergstrasse 221", ZipCode = "8044", City = "Zürich", CurrentCapacity = 487, MaxCapacity = 1125 },
        new Store { Name = "Polenmuseum - Schloss Rapperswil", Secret = new Guid("048cedbd-78d0-4837-9f6f-334aebb7a04e"), Street = "Schloss", ZipCode = "8640", City = "Raperswil-Jona", CurrentCapacity = 11, MaxCapacity = 62 },
        new Store { Name = "Alpamare", Secret = new Guid("9f1fb7fb-5e9e-4ebd-90d7-30f82ebec8e7"), Street = "Gwattstrasse 12", ZipCode = "8808", City = "Pfäffikon", CurrentCapacity = 152, MaxCapacity = 612 }
      };
      _storeRepository.AddStores(stores);
      stores = (List<Store>)_storeRepository.GetStores();
      stores.Count.Should().Be(4);
      foreach (var store in stores) {
        store.Name.Should().NotBeNullOrEmpty();
      }
    }

    [Fact]
    public void TestUpdateStore() {
      Store store = new Store { Name = "Botanischer Garten der Universität Bern", Street = "Altenbergrain 21", ZipCode = "3013", City = "Bern", CurrentCapacity = 103, MaxCapacity = 103 };
      store = _storeRepository.AddStore(store);
      store = new Store { Id = store.Id, Name = "Botanischer Garten St. Gallen", Street = "Keine Ahnung", ZipCode = "9008", City = "Bern", CurrentCapacity = 103, MaxCapacity = 103 };
      store = _storeRepository.UpdateStore(store);
      _storeRepository.GetStore(store.Id).Name.Should().Be("Botanischer Garten St. Gallen");
    }
  }
}
