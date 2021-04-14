using CapWatchBackend.Application.Repositories;
using CapWatchBackend.DataAccess.MongoDB.Repositories;
using CapWatchBackend.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CapWatchBackend.WebApi.Tests.Mocks {
  public class StoreRepositoryMock : IStoreRepository {
    public IEnumerable<Store> GetStores() {
      List<Store> stores = new List<Store> {
        new Store { Id = 1, Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", CurrentCapacity = 135, MaxCapacity = 201 },
        new Store { Id = 2, Name = "Zoo Zürich", Street = "Zürichbergstrasse 221", ZipCode = "8044", City = "Zürich", CurrentCapacity = 487, MaxCapacity = 1125 },
        new Store { Id = 3, Name = "Polenmuseum - Schloss Rapperswil", Street = "Schloss", ZipCode = "8640", City = "Raperswil-Jona", CurrentCapacity = 11, MaxCapacity = 62 }
      };

      return stores;
    }

    public Store AddStore(Store store) {
      store.Id = (int)IntIdGenerator.Instance.GenerateId(null, 0);
      return store;
    }

    public IEnumerable<Store> AddStores(IEnumerable<Store> stores) {
      foreach (var store in stores) {
        store.Id = (int)IntIdGenerator.Instance.GenerateId(null, 0);
      }
      return stores;
    }

    public Store UpdateStore(Store store) { return store; }

    public IEnumerable<Store> UpdateStores(IEnumerable<Store> stores) { return stores; }

    public IEnumerable<Store> GetStores(Func<Store, bool> filter, Func<Store, int> ordering, int orderBy) {
      List<Store> stores = new List<Store> {
        new Store { Id = 1, Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", CurrentCapacity = 135, MaxCapacity = 201 },
        new Store { Id = 2, Name = "Zoo Zürich", Street = "Zürichbergstrasse 221", ZipCode = "8044", City = "Zürich", CurrentCapacity = 487, MaxCapacity = 1125 },
        new Store { Id = 3, Name = "Polenmuseum - Schloss Rapperswil", Street = "Schloss", ZipCode = "8640", City = "Raperswil-Jona", CurrentCapacity = 11, MaxCapacity = 62 }
      };
      return stores;
    }

    public Store GetStore(int id) {
      return new Store { Id = 1, Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", CurrentCapacity = 135, MaxCapacity = 201 };
    }

    public void DeleteAllStores() { }

    public void DeleteStore(Store store) { }
  }
}
