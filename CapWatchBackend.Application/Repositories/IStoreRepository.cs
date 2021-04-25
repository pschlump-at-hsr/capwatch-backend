using CapWatchBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CapWatchBackend.Application.Repositories {
  public interface IStoreRepository {
    public Task AddStoreAsync(Store store);
    public Task AddStoresAsync(IEnumerable<Store> stores);

    public Task UpdateStoreAsync(Store store);
    public Task UpdateStoresAsync(IEnumerable<Store> stores);

    public Task<IEnumerable<Store>> GetStores();
    public Task<IEnumerable<Store>> GetStores(Func<Store, bool> filter);
    public Task<Store> GetStore(Guid id);

    public void DeleteAllStores();
    public void DeleteStore(Store store);
  }
}
