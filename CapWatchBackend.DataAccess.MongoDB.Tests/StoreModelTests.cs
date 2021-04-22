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
      var typeRepository = new TypeRepository("mongodb://capwusr:capwusr123@localhost:27017/admin");
      _storeRepository = new StoreRepository("mongodb://capwusr:capwusr123@localhost:27017/admin", typeRepository);
      _storeRepository.DeleteAllStores();
    }

    [Fact]
    public void TestAddStore() {
      Store store = new Store { Name = "Botanischer Garten der Universität Bern", Secret = new Guid("57bd0e44-4032-4a9c-b48e-c89d990bcd6f"), Street = "Altenbergrain 21", ZipCode = "3013", City = "Bern", CurrentCapacity = 103, MaxCapacity = 103, StoreType = new StoreType { Id = Guid.Parse("c73e9c5f-de5c-479a-b116-7ee1b93ab4f9"), Description = "Detailhändler" }, };
      _storeRepository.AddStoreAsync(store).Wait();
      store.Id.Should().NotBe(Guid.Parse("00000000-0000-0000-0000-000000000000"));
      store = _storeRepository.GetStore(store.Id);
      store.Name.Should().Be("Botanischer Garten der Universität Bern");
    }

    [Fact]
    public void TestGetStores() {
      List<Store> stores = new List<Store> {
        new Store { Name = "Ikea", Secret = new Guid("02a4299e-f472-41d1-9468-df369bd30872"), Street = "Zrcherstrasse 460", ZipCode = "9015", City = "St. Gallen", CurrentCapacity = 135, MaxCapacity = 201 },
        new Store { Name = "Zoo Zrich", Secret = new Guid("9e573805-f5b4-49bb-bbaf-844b44ea3942"), Street = "Zrichbergstrasse 221", ZipCode = "8044", City = "Zrich", CurrentCapacity = 487, MaxCapacity = 1125 },
        new Store { Name = "Polenmuseum - Schloss Rapperswil", Secret = new Guid("048cedbd-78d0-4837-9f6f-334aebb7a04e"), Street = "Schloss", ZipCode = "8640", City = "Raperswil-Jona", CurrentCapacity = 11, MaxCapacity = 62 },
        new Store { Name = "Alpamare", Secret = new Guid("9f1fb7fb-5e9e-4ebd-90d7-30f82ebec8e7"), Street = "Gwattstrasse 12", ZipCode = "8808", City = "Pfffikon", CurrentCapacity = 152, MaxCapacity = 612 }
      };
      _storeRepository.AddStoresAsync(stores).Wait();
      stores = (List<Store>)_storeRepository.GetStores();
      stores.Count.Should().Be(4);
      foreach (var store in stores) {
        store.Name.Should().NotBeNullOrEmpty();
      }
    }

    [Fact]
    public void TestUpdateStore() {
      Store store = new Store { Name = "Botanischer Garten der Universität Bern", Street = "Altenbergrain 21", ZipCode = "3013", City = "Bern", CurrentCapacity = 103, MaxCapacity = 103 };
      _storeRepository.AddStoreAsync(store).Wait();
      store = new Store { Id = store.Id, Name = "Botanischer Garten St. Gallen", Street = "Keine Ahnung", ZipCode = "9008", City = "Bern", CurrentCapacity = 103, MaxCapacity = 103 };
      _storeRepository.UpdateStoreAsync(store).Wait();
      _storeRepository.GetStore(store.Id).Name.Should().Be("Botanischer Garten St. Gallen");
    }
  }
}
