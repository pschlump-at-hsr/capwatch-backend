using CapWatchBackend.Domain.Entities;
using MongoDB.Driver;

namespace CapWatchBackend.DataAccess.MongoDB.DbContext {
  public interface ICapwatchDBContext {
    IMongoCollection<Store> GetStoreCollection();
    IMongoCollection<StoreType> GetTypeCollection();
  }
}
