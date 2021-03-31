using CapWatchBackend.Application.Repositories;
using CapWatchBackend.DataAccess.MongoDB.Model;
using CapWatchBackend.Domain.Entities;
using System.Collections.Generic;

namespace CapWatchBackend.DataAccess.MongoDB.Repositories {
  public class StoreRepository : IStoreRepository {
    public IEnumerable<Store> GetStores() {
      var sm = StoreModel.GetStoreModel();
      return sm.GetStores();
    }
  }
}
