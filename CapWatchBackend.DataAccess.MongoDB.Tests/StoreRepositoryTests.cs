using CapWatchBackend.Application.Exceptions;
using CapWatchBackend.DataAccess.MongoDB.Repositories;
using CapWatchBackend.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CapWatchBackend.DataAccess.MongoDB.Tests {

  public class StoreRepositoryTests {
    private readonly StoreRepository _storeRepository;
    private readonly Guid _invalidStoreTypeId = new("00000000-1000-0000-0000-000000000000");

    public StoreRepositoryTests() {
      _storeRepository = new StoreRepository("mongodb://capwusr:capwusr123@localhost:27017/admin");
      _storeRepository.DeleteAllStoresAsync().Wait();
    }

    [Fact]
    public async Task TestAddStoreAsync() {
      var store = GetTestStore();

      await _storeRepository.AddStoreAsync(store);

      store.Id.Should().NotBe(Guid.Empty)
          .And.Should().NotBeNull();

      store = await _storeRepository.GetStoreAsync(store.Id);
      store.Name.Should().Be("Botanischer Garten der Universitaet Bern");
    }

    [Fact]
    public void TestAddStoreAsyncInvalidStoreType() {
      var store = GetTestStore();
      store.StoreType.Id = _invalidStoreTypeId;

      _storeRepository.Invoking(async repository => await repository.AddStoreAsync(store)).Should().Throw<StoreTypeInvalidException>();
    }

    [Fact]
    public async Task TestAddStoresAsync() {
      var stores = GetTestStores();

      await _storeRepository.AddStoresAsync(stores);

      stores.Any(store => store.Id == Guid.Empty).Should().BeFalse();

      var storesAfterInsert = await _storeRepository.GetStoresAsync();
      storesAfterInsert.Any(store => store.Name.Equals("Ikea")).Should().BeTrue();
      storesAfterInsert.Any(store => store.Name.Equals("Zoo Zuerich")).Should().BeTrue();
      storesAfterInsert.Any(store => store.Name.Equals("Polenmuseum - Schloss Rapperswil")).Should().BeTrue();
    }

    [Fact]
    public void TestAddStoresAsyncInvalidStoreType() {
      var stores = GetTestStores();
      stores[0].StoreType.Id = _invalidStoreTypeId;

      _storeRepository.Invoking(async repository => await repository.AddStoresAsync(stores)).Should().Throw<StoreTypeInvalidException>();
    }

    [Fact]
    public async Task TestUpdateStoreAsync() {
      var store = GetTestStore();

      await _storeRepository.AddStoreAsync(store);

      store.Name = "Botanischer Garten St. Gallen";
      store.Street = "Keine Ahnung";
      store.ZipCode = "9008";
      store.City = "St. Gallen";

      await _storeRepository.UpdateStoreAsync(store);

      var storeAfterUpdate = await _storeRepository.GetStoreAsync(store.Id);
      storeAfterUpdate.Name.Should().Be("Botanischer Garten St. Gallen");
    }

    [Fact]
    public async Task TestUpdateStoreAsyncInvalidStoreType() {
      var store = GetTestStore();

      await _storeRepository.AddStoreAsync(store);

      store.StoreType.Id = _invalidStoreTypeId;

      _storeRepository.Invoking(async repository => await repository.UpdateStoreAsync(store)).Should().Throw<StoreTypeInvalidException>();
    }

    [Fact]
    public async Task TestUpdateStoresAsync() {
      var stores = GetTestStores();

      await _storeRepository.AddStoresAsync(stores);

      var storesAfterInsert = await _storeRepository.GetStoresAsync();
      storesAfterInsert.FirstOrDefault(x => x.Name.Equals("Ikea")).Name = "Invalid";

      await _storeRepository.UpdateStoresAsync(storesAfterInsert);

      var storesAfterUpdate = await _storeRepository.GetStoresAsync();
      storesAfterUpdate.Any(x => x.Name.Equals("Ikea")).Should().BeFalse();
    }

    [Fact]
    public async Task TestUpdateStoresAsyncInvalidStoreType() {
      var stores = GetTestStores();

      await _storeRepository.AddStoresAsync(stores);

      var storesAfterInsert = await _storeRepository.GetStoresAsync();
      storesAfterInsert.FirstOrDefault(x => x.Name.Equals("Ikea")).StoreType.Id = _invalidStoreTypeId;

      _storeRepository.Invoking(async repository => await repository.UpdateStoresAsync(storesAfterInsert)).Should().Throw<StoreTypeInvalidException>();
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
    public async Task TestGetStoresAsyncFiltered() {
      await _storeRepository.AddStoresAsync(GetTestStores());

      bool filterFunction(Store store) => store.Name.Equals("Ikea");

      var stores = await _storeRepository.GetStoresAsync(filterFunction);

      stores.Count().Should().Be(1);
      stores.Any(store => store.Street.Equals("Zuercherstrasse 460")).Should().BeTrue();
    }

    [Fact]
    public async Task TestGetStoresAsyncFilteredNoResult() {
      await _storeRepository.AddStoresAsync(GetTestStores());

      bool filterFunction(Store store) => store.Name.Equals("Invalid");

      var stores = await _storeRepository.GetStoresAsync(filterFunction);

      stores.Count().Should().Be(0);
    }

    [Fact]
    public async Task TestGetStoreAsync() {
      var stores = GetTestStores();
      await _storeRepository.AddStoresAsync(stores);

      var store = await _storeRepository.GetStoreAsync(stores.FirstOrDefault(store => store.Name.Equals("Ikea")).Id);

      store.Name.Should().Be("Ikea");
    }

    [Fact]
    public async Task TestDeleteAllStoresAsync() {
      await _storeRepository.AddStoresAsync(GetTestStores());

      var stores = await _storeRepository.GetStoresAsync();
      stores.Count().Should().Be(3);

      await _storeRepository.DeleteAllStoresAsync();

      stores = await _storeRepository.GetStoresAsync();
      stores.Count().Should().Be(0);
    }

    [Fact]
    public async Task TestDeleteStoreAsync() {
      await _storeRepository.AddStoresAsync(GetTestStores());

      var stores = await _storeRepository.GetStoresAsync();
      await _storeRepository.DeleteStoreAsync(stores.FirstOrDefault(store => store.Name.Equals("Ikea")));

      stores = await _storeRepository.GetStoresAsync();
      stores.Count().Should().Be(2);
      stores.Any(store => store.Name.Equals("Ikea")).Should().BeFalse();
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
          City = "Rapperswil-Jona",
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
