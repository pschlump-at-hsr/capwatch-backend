using CapWatchBackend.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CapWatchBackend.Application.Repositories {
  public interface IStoreRepository {
    public Store AddStore(Store store);
    public IEnumerable<Store> AddStores(IEnumerable<Store> stores);

    public Store UpdateStore(Store store);
    public IEnumerable<Store> UpdateStores(IEnumerable<Store> stores);

    public IEnumerable<Store> GetStores();
    public IEnumerable<Store> GetStores(Func<Store, bool> filter, Func<Store, int> ordering, int orderBy);
    public Store GetStore(int id);

    public void DeleteAllStores();
    public void DeleteStore(Store store);
  }
}
