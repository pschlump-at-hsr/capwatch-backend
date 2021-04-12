using CapWatchBackend.Application.Repositories;
using CapWatchBackend.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CapWatchBackend.DataAccess.MongoDB.Repositories {
  public class StoreRepository : IStoreRepository {
    private IMongoCollection<Store> _storesCol;
    private readonly ConfigureDatabase _configureDatabase;
    public StoreRepository(IOptions<ConfigureDatabase> options) {
      _configureDatabase = options.Value;
      var capWatchDbo = CapwatchDbo.GetInstance(_configureDatabase.ConnectionString);
      _storesCol = capWatchDbo.GetStoreCollection();
    }

    public StoreRepository(string connectionString) {
      var capWatchDbo = CapwatchDbo.GetInstance(connectionString);
      _storesCol = capWatchDbo.GetStoreCollection();
    }

    public Task AddStoreAsync(Store store) {
      try {
        return _storesCol.InsertOneAsync(store);
      } catch (MongoClientException e) {
        throw e;
      }
    }

    public Task AddStoresAsync(IEnumerable<Store> stores) {
      try {
        return _storesCol.InsertManyAsync(stores);
      } catch (MongoClientException e) {
        throw e;
      }
    }

    public Task UpdateStoreAsync(Store store) {
      try {
        return _storesCol.ReplaceOneAsync(filter: new BsonDocument("storeId", store.Id), replacement: store);
      } catch (MongoClientException e) {
        throw e;
      }
    }

    public Task UpdateStoresAsync(IEnumerable<Store> stores) {
      List<Task> tasks = new List<Task>();
      try {
        foreach (var store in stores) {
          tasks.Add(_storesCol.ReplaceOneAsync(filter: new BsonDocument("storeId", store.Id), replacement: store));
        }
        return Task.WhenAll(tasks.ToArray());
      } catch (MongoClientException e) {
        throw e;
      }
    }

    public IEnumerable<Domain.Entities.Store> GetStores() {
      return _storesCol.AsQueryable().OrderByDescending(x => x.Id).DistinctBy(x => x.Id);
    }

    public IEnumerable<Domain.Entities.Store> GetStores(Func<Store, bool> filter, Func<Store, int> ordering, OrderByDirection orderBy) {
      return _storesCol.AsQueryable().Where(filter).OrderBy(ordering, orderBy);
    }

    public Store GetStore(int id) {
      return _storesCol.AsQueryable().Where(x => x.Id == id).SingleOrDefault();
    }

    public async void DeleteAllStores() {
      await _storesCol.DeleteManyAsync(FilterDefinition<Store>.Empty);
    }

    public async void DeleteStore(Store store) {
      await _storesCol.DeleteManyAsync(new BsonDocument("storeId", store.Id));
    }

  }
}
