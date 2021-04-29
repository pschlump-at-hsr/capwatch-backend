using CapWatchBackend.Application.Handlers;
using CapWatchBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapWatchBackend.WebApi.Tests.Mocks {
  class StoreHandlerMock : IStoreHandler {
    private static readonly Guid _storeId = new("10000000-0000-0000-0000-000000000000");

    public Task AddStoreAsync(Store store) {
      store.Id = _storeId;
      store.Secret = new("00000000-0000-0000-0000-000000000001");
      return Task.CompletedTask;
    }

    public Task<Store> GetStoreAsync(Guid id) {
      if (id == _storeId) {
        return Task.Factory.StartNew(() => {
          return GetStore();
        });
      } else {
        throw new Exception();
      }
    }

    public Task<IEnumerable<Store>> GetStoresAsync() {
      var stores = GetStores();
      return Task.Factory.StartNew(() => { return stores; });
    }

    public Task<IEnumerable<Store>> GetStoresAsync(string filter) {
      var stores = GetStores();
      return Task.Factory.StartNew(() => { return stores.Where(store => store.Name.Contains(filter)); });
    }

    public Task UpdateStoreAsync(Store store) {
      return Task.CompletedTask;
    }

    private static Store GetStore() {
      return new Store {
        Id = _storeId,
        Name = "Ikea",
        Street = "Zuercherstrasse 460",
        ZipCode = "9015",
        City = "St. Gallen",
        CurrentCapacity = 135,
        MaxCapacity = 201,
        Secret = new("00000000-0000-0000-0000-000000000001"),
        StoreType = new StoreType {
          Id = new("00000000-1000-0000-0000-000000000000"),
          Description = "Detailhaendler"
        }
      };
    }

    private static IEnumerable<Store> GetStores() {
      return new List<Store> {
        GetStore(),
        new Store {
          Id = new("20000000-0000-0000-0000-000000000000"),
          Name = "Zoo Zuerich",
          Street = "Zuerichbergstrasse 221",
          ZipCode = "8044",
          City = "Zuerich",
          CurrentCapacity = 487,
          MaxCapacity = 1125,
          Secret = new("00000000-0000-0000-0000-000000000002"),
          StoreType = new StoreType {
            Id = new("00000000-2000-0000-0000-000000000000"),
            Description = "Freizeit"
          }
        },
        new Store {
          Id = new("30000000-0000-0000-0000-000000000000"),
          Name = "Polenmuseum - Schloss Rapperswil",
          Street = "Schloss",
          ZipCode = "8640",
          City = "Raperswil-Jona",
          CurrentCapacity = 11,
          MaxCapacity = 62,
          Secret = new("00000000-0000-0000-0000-000000000003"),
          StoreType = new StoreType {
            Id = new("00000000-3000-0000-0000-000000000000"),
            Description = "Bank"
          }
        }
      };
    }
  }
}
