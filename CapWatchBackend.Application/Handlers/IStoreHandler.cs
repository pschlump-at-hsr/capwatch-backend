using CapWatchBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CapWatchBackend.Application.Handlers {
  public interface IStoreHandler {
    Task AddStoreAsync(Store store);
    Task UpdateStoreAsync(Store store);
    Task<IEnumerable<Store>> GetStoresAsync();
    Task<IEnumerable<Store>> GetStoresAsync(string filter);
    Task<Store> GetStoreAsync(Guid id);
  }
}
