using CapWatchBackend.Application.Handlers;
using CapWatchBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapWatchBackend.WebApi.Tests.Mocks {
  class StoreHandlerMock : IStoreHandler {
    public Task AddStoreAsync(Store store) {
      store.Id = Guid.Parse("9c9cee44-c839-48f2-b54e-246d95fe5d7f");
      store.Secret = Guid.Parse("9c9cee44-c839-48f2-b54e-236d95fe5d7f");
      return Task.CompletedTask;
    }

    public Task<Store> GetStore(Guid id) {
      if (id == Guid.Parse("9c9cee44-c839-48f2-b54e-235d95fe5d7f")) {
        return Task.Factory.StartNew(() => {
          return new Store {
            Id = Guid.Parse("9c9cee44-c839-48f2-b54e-235d95fe5d7f"),
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
          };
        });
      } else {
        throw new Exception();
      }
    }

    public Task<IEnumerable<Store>> GetStores() {
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

      return Task.Factory.StartNew(() => { return (IEnumerable<Store>)stores; });
    }

    public Task<IEnumerable<Store>> GetStores(string filter) {
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

      return Task.Factory.StartNew(() => {
        return stores.Where(x => x.Name.Contains(filter));
      });
    }

    public Task UpdateStoreAsync(Store store) {
      return Task.CompletedTask;
    }
  }
}
