using CapWatchBackend.DataAccess.MongoDB.Repositories;
using CapWatchBackend.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CapWatchBackend.DataAccess.MongoDB.Tests {

  public class StoreModelTests {

    private readonly StoreRepository _storeRepository;
    public StoreModelTests() {
      _storeRepository = new StoreRepository("mongodb://capwusr:capwusr123@localhost:27017/admin");
      _storeRepository.DeleteAllStores();
    }

    [Fact]
    public async Task TestAddStore() {
      Store store = new Store { Name = "Botanischer Garten der Universität Bern", Secret = new Guid("57bd0e44-4032-4a9c-b48e-c89d990bcd6f"), Street = "Altenbergrain 21", ZipCode = "3013", City = "Bern", CurrentCapacity = 103, MaxCapacity = 103, StoreType = new StoreType { Id = Guid.Parse("c73e9c5f-de5c-479a-b116-7ee1b93ab4f9"), Description = "Detailhändler" } };
      await _storeRepository.AddStoreAsync(store);
      store.Id.Should().NotBe(Guid.Parse("00000000-0000-0000-0000-000000000000"))
          .And.Should().NotBeNull();
      store = await _storeRepository.GetStore(store.Id);
      store.Name.Should().Be("Botanischer Garten der Universität Bern");
    }

    [Fact]
    public async Task TestGetStores() {
      List<Store> stores = new List<Store> {
        new Store {
          Id = Guid.Parse("9c9cee44-c839-48f2-b54e-237d95fe5d7f"),
          Name = "Ikea",
          Street = "Zürcherstrasse 460",
          ZipCode = "9015",
          City = "St. Gallen",
          CurrentCapacity = 135,
          MaxCapacity = 201,
          Secret = Guid.Parse("00000000-0000-0000-0000-000000000001"),
          StoreType = new StoreType {
            Id = Guid.Parse("c73e9c5f-de5c-479a-b116-7ee1b93ab4f9"),
            Description = "Detailhändler"
          }
        },
        new Store {
          Id = Guid.Parse("9c9cee44-c839-48f2-b54e-238d95fe5d7f"),
          Name = "Zoo Zürich",
          Street = "Zürichbergstrasse 221",
          ZipCode = "8044",
          City = "Zürich",
          CurrentCapacity = 487,
          MaxCapacity = 1125,
          Secret = Guid.Parse("00000000-0000-0000-0000-000000000002"),
          StoreType = new StoreType {
            Id = Guid.Parse("7b0523b7-4efd-4fdf-b11d-3f4d26cf7b19"),
            Description = "Freizeit"
          }},
        new Store {
          Id = Guid.Parse("9c9cee44-c839-48f2-b54e-239d95fe5d7f"),
          Name = "Polenmuseum - Schloss Rapperswil",
          Street = "Schloss",
          ZipCode = "8640",
          City = "Raperswil-Jona",
          CurrentCapacity = 11,
          MaxCapacity = 62,
          Secret = Guid.Parse("00000000-0000-0000-0000-000000000003"),
          StoreType = new StoreType {
            Id = Guid.Parse("f58957ce-fb83-4f62-ac2c-6d1fe810d85c"),
            Description = "Bank"
          }
        }
      };
      await _storeRepository.AddStoresAsync(stores);
      stores = (List<Store>)await _storeRepository.GetStores();
      stores.Count.Should().Be(3);
      foreach (var store in stores) {
        store.Name.Should().NotBeNullOrEmpty();
      }
    }

    [Fact]
    public async Task TestUpdateStore() {
      Store store = new Store { Name = "Botanischer Garten der Universität Bern", Secret = new Guid("57bd0e44-4032-4a9c-b48e-c89d990bcd6f"), Street = "Altenbergrain 21", ZipCode = "3013", City = "Bern", CurrentCapacity = 103, MaxCapacity = 103, StoreType = new StoreType { Id = Guid.Parse("c73e9c5f-de5c-479a-b116-7ee1b93ab4f9"), Description = "Detailhändler" } };
      await _storeRepository.AddStoreAsync(store);
      store = new Store { Id = store.Id, Name = "Botanischer Garten St. Gallen", Secret = new Guid("57bd0e44-4032-4a9c-b48e-c89d990bcd6f"), Street = "Keine Ahnung", ZipCode = "9008", City = "Bern", CurrentCapacity = 103, MaxCapacity = 103, StoreType = new StoreType { Id = Guid.Parse("c73e9c5f-de5c-479a-b116-7ee1b93ab4f9"), Description = "Detailhändler" } };
      await _storeRepository.UpdateStoreAsync(store);
      _storeRepository.GetStore(store.Id).GetAwaiter().GetResult().Name.Should().Be("Botanischer Garten St. Gallen");
    }
  }
}
