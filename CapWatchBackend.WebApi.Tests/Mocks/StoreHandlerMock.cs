using CapWatchBackend.Application.Handlers;
using CapWatchBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapWatchBackend.WebApi.Tests.Mocks {
  class StoreHandlerMock : IStoreHandler {
    public void AddStore(Store store) {
      store.Id = 10;
      store.Secret = Guid.Parse("9c9cee44-c839-48f2-b54e-236d95fe5d7f");
    }

    public Store GetStore(int id) {
      if (id == 1) {
        return new Store { Id = 1, Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", CurrentCapacity = 135, MaxCapacity = 201 };
      } else {
        throw new Exception();
      }
    }

    public IEnumerable<Store> GetStores() {
      List<Store> stores = new List<Store> {
        new Store { Id = 1, Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", CurrentCapacity = 135, MaxCapacity = 201 },
        new Store { Id = 2, Name = "Zoo Zürich", Street = "Zürichbergstrasse 221", ZipCode = "8044", City = "Zürich", CurrentCapacity = 487, MaxCapacity = 1125 },
        new Store { Id = 3, Name = "Polenmuseum - Schloss Rapperswil", Street = "Schloss", ZipCode = "8640", City = "Raperswil-Jona", CurrentCapacity = 11, MaxCapacity = 62 }
      };

      return stores;
    }

    public IEnumerable<Store> GetStores(string filter) {
      List<Store> stores = new List<Store> {
        new Store { Id = 1, Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", CurrentCapacity = 135, MaxCapacity = 201 },
        new Store { Id = 2, Name = "Zoo Zürich", Street = "Zürichbergstrasse 221", ZipCode = "8044", City = "Zürich", CurrentCapacity = 487, MaxCapacity = 1125 },
        new Store { Id = 3, Name = "Polenmuseum - Schloss Rapperswil", Street = "Schloss", ZipCode = "8640", City = "Raperswil-Jona", CurrentCapacity = 11, MaxCapacity = 62 }
      };

      return stores.Where(x => x.Name.Contains(filter));
    }

    public void UpdateStore(Store store) {
      // Not needed for testing
    }
  }
}
