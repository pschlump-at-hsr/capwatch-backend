using CapWatchBackend.Domain.Entities;
using System.Collections.Generic;

namespace CapWatchBackend.Application.Repositories {
  public interface IStoreRepository {
    IEnumerable<Store> GetStores();
  }
}
