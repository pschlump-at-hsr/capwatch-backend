using CapWatchBackend.Application.Repositories;
using CapWatchBackend.Domain.Entities;
using System.Collections.Generic;

namespace CapWatchBackend.WebApi.Tests.Mocks {
  public class StoreRepositoryMock : IStoreRepository {
    public IEnumerable<Store> GetStores() {
      List<Store> stores = new List<Store> {
        new Store { Id = 1, Name = "Migros St. Gallen", CurrentCapacity = 70, MaxCapacity = 180 },
        new Store { Id = 2, Name = "Säntispark Bäder", CurrentCapacity = 125, MaxCapacity = 150 },
        new Store { Id = 3, Name = "Interdiscount", CurrentCapacity = 7, MaxCapacity = 26 }
      };

      return stores;
    }
  }
}
