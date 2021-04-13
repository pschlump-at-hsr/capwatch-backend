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
    public StoreRepository(IOptions<ConfigureDatabase> options) {
      try {
        var capWatchDbo = CapwatchDbo.GetInstance(options.Value.ConnectionString);
        _storesCol = capWatchDbo.GetStoreCollection();
      } catch (RepositoryException e) {
        DbLogger.Log(e.Message);
      }
    }

    public StoreRepository(string connectionString) {
      var capWatchDbo = CapwatchDbo.GetInstance(connectionString);
      _storesCol = capWatchDbo.GetStoreCollection();
    }

    public Task AddStoreAsync(Store store) {
      try {
        return _storesCol.InsertOneAsync(store);
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public Task AddStoresAsync(IEnumerable<Store> stores) {
      try {
        return _storesCol.InsertManyAsync(stores);
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public Task UpdateStoreAsync(Store store) {
      try {
        return _storesCol.ReplaceOneAsync(filter: new BsonDocument("storeId", store.Id), replacement: store);
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
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
        throw new RepositoryException(e.Message, e);
      }
    }

    public IEnumerable<Store> GetStores() {
      try {
        return _storesCol.AsQueryable().OrderByDescending(x => x.Id).DistinctBy(x => x.Id);
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public IEnumerable<Store> GetStores(Func<Store, bool> filter, Func<Store, int> ordering, OrderByDirection orderBy) {
      try {
        return _storesCol.AsQueryable().Where(filter).OrderBy(ordering, orderBy);
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public Store GetStore(int id) {
      try {
        return _storesCol.AsQueryable().Where(x => x.Id == id).SingleOrDefault();
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public async void DeleteAllStores() {
      try {
        await _storesCol.DeleteManyAsync(FilterDefinition<Store>.Empty);
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public async void DeleteStore(Store store) {
      try {
        await _storesCol.DeleteManyAsync(new BsonDocument("storeId", store.Id));
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

  }
}
