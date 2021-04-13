using CapWatchBackend.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CapWatchBackend.Application.Repositories {
  public interface IStoreRepository {
    IEnumerable<Store> GetStores();
    Store GetStore(int id);
    Task AddStoreAsync(Store store);
    Task UpdateStoreAsync(Store store);
  }
}
