using CapWatchBackend.DataAccess.MongoDB.Repositories;
using CapWatchBackend.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CapWatchBackend.DataAccess.MongoDB.Tests {

  public class StoreModelTests {
    private readonly StoreRepository _storeRepository;

    public StoreModelTests() {
      _storeRepository = new StoreRepository("mongodb://capwusr:capwusr123@localhost:27017/admin");
      _storeRepository.DeleteAllStoresAsync().Wait();
    }

    [Fact]
    public async Task TestAddStoreAsync() {
      Store store = GetTestStore();

      await _storeRepository.AddStoreAsync(store);

      store.Id.Should().NotBe(Guid.Empty)
          .And.Should().NotBeNull();

      store = await _storeRepository.GetStoreAsync(store.Id);
      store.Name.Should().Be("Botanischer Garten der Universitaet Bern");
    }

    [Fact]
    public async Task TestGetStoresAsync() {
      await _storeRepository.AddStoresAsync(GetTestStores());

      var stores = await _storeRepository.GetStoresAsync();

      stores.Count().Should().Be(3);
      foreach (var store in stores) {
        store.Name.Should().NotBeNullOrEmpty();
      }
    }

    [Fact]
    public async Task TestUpdateStoreAsync() {
      Store store = GetTestStore();

      await _storeRepository.AddStoreAsync(store);

      store.Name = "Botanischer Garten St. Gallen";
      store.Street = "Keine Ahnung";
      store.ZipCode = "9008";
      store.City = "St. Gallen";

      await _storeRepository.UpdateStoreAsync(store);

      _storeRepository.GetStoreAsync(store.Id).GetAwaiter().GetResult().Name.Should().Be("Botanischer Garten St. Gallen");
    }

    private static Store GetTestStore() {
      return new Store {
        Name = "Botanischer Garten der Universitaet Bern",
        Secret = new("00000000-0000-0000-0000-000000000001"),
        Street = "Altenbergrain 21",
        ZipCode = "3013",
        City = "Bern",
        CurrentCapacity = 103,
        MaxCapacity = 103,
        StoreType = new StoreType {
          Id = new("c73e9c5f-de5c-479a-b116-7ee1b93ab4f9"),
          Description = "Detailhändler"
        }
      };
    }

    private static IList<Store> GetTestStores() {
      return new List<Store> {
        new Store {
          Name = "Ikea",
          Street = "Zuercherstrasse 460",
          ZipCode = "9015",
          City = "St. Gallen",
          CurrentCapacity = 135,
          MaxCapacity = 201,
          Secret = new("00000000-0000-0000-0000-000000000001"),
          StoreType = new StoreType {
            Id = new("c73e9c5f-de5c-479a-b116-7ee1b93ab4f9"),
            Description = "Detailhändler"
          }
        },
        new Store {
          Name = "Zoo Zuerich",
          Street = "Zuerichbergstrasse 221",
          ZipCode = "8044",
          City = "Zuerich",
          CurrentCapacity = 487,
          MaxCapacity = 1125,
          Secret = new("00000000-0000-0000-0000-000000000002"),
          StoreType = new StoreType {
            Id = new("7b0523b7-4efd-4fdf-b11d-3f4d26cf7b19"),
            Description = "Freizeit"
          }},
        new Store {
          Name = "Polenmuseum - Schloss Rapperswil",
          Street = "Schloss",
          ZipCode = "8640",
          City = "Raperswil-Jona",
          CurrentCapacity = 11,
          MaxCapacity = 62,
          Secret = new("00000000-0000-0000-0000-000000000003"),
          StoreType = new StoreType {
            Id = new("f58957ce-fb83-4f62-ac2c-6d1fe810d85c"),
            Description = "Bank"
          }
        }
      };
    }
  }
}
