using CapWatchBackend.Application.Handlers;
using CapWatchBackend.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CapWatchBackend.WebApi.Tests.Mocks {
  class StoreHandlerMock : IStoreHandler {
    public Task AddStoreAsync(Store store) {
      store.Id = Guid.Parse("9c9cee44-c839-48f2-b54e-246d95fe5d7f");
      store.Secret = Guid.Parse("9c9cee44-c839-48f2-b54e-236d95fe5d7f");
      return Task.CompletedTask;
    }

    public Store GetStore(Guid id) {
      if (id == Guid.Parse("9c9cee44-c839-48f2-b54e-235d95fe5d7f")) {
        return new Store { Id = Guid.Parse("9c9cee44-c839-48f2-b54e-235d95fe5d7f"), Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", CurrentCapacity = 135, MaxCapacity = 201 };
      } else {
        throw new Exception();
      }
    }

    public IEnumerable<Store> GetStores() {
      List<Store> stores = new List<Store> {
        new Store { Id = Guid.Parse("9c9cee44-c839-48f2-b54e-237d95fe5d7f"), Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", CurrentCapacity = 135, MaxCapacity = 201 },
        new Store { Id = Guid.Parse("9c9cee44-c839-48f2-b54e-238d95fe5d7f"), Name = "Zoo Zürich", Street = "Zürichbergstrasse 221", ZipCode = "8044", City = "Zürich", CurrentCapacity = 487, MaxCapacity = 1125 },
        new Store { Id = Guid.Parse("9c9cee44-c839-48f2-b54e-239d95fe5d7f"), Name = "Polenmuseum - Schloss Rapperswil", Street = "Schloss", ZipCode = "8640", City = "Raperswil-Jona", CurrentCapacity = 11, MaxCapacity = 62 }
      };

      return stores;
    }

    public void UpdateStore(Store store) {
      // Not needed for testing
    }
  }
}
