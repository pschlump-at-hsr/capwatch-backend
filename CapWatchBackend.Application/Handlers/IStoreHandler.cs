using CapWatchBackend.Domain.Entities;
using System.Collections.Generic;

namespace CapWatchBackend.Application.Handlers {
  public interface IStoreHandler {
    void AddStore(Store store);
    void UpdateStore(Store store);
    IEnumerable<Store> GetStores();
    IEnumerable<Store> GetStores(string filter);
    Store GetStore(int id);
  }
}
