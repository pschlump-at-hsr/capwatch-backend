using CapWatchBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CapWatchBackend.Application.Handlers {
  public interface IStoreHandler {
    Task AddStoreAsync(Store store);
    Task UpdateStoreAsync(Store store);
    Task<IEnumerable<Store>> GetStores();
    Task<IEnumerable<Store>> GetStores(string filter);
    Task<Store> GetStore(Guid id);
  }
}
