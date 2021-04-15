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
        var maxId = _storesCol.AsQueryable().Max(x => x.Id);
        IntIdGenerator.Instance.GenerateId(null, maxId);
      } catch (RepositoryException e) {
        DbLogger.Log(e.Message);
      }
    }

    public StoreRepository(string connectionString) {
      var capWatchDbo = CapwatchDbo.GetInstance(connectionString);
      _storesCol = capWatchDbo.GetStoreCollection();
      var maxId = _storesCol.AsQueryable().Max(x => x.Id);
      IntIdGenerator.Instance.GenerateId(null, maxId);
    }

    public Store AddStore(Store store) {
      try {
        store.Id = (int)IntIdGenerator.Instance.GenerateId(_storesCol, 0);
        _storesCol.InsertOne(store);
        return store;
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public IEnumerable<Store> AddStores(IEnumerable<Store> stores) {
      try {
        foreach (var store in stores) {
          store.Id = (int)IntIdGenerator.Instance.GenerateId(_storesCol, 0);
        }
        _storesCol.InsertMany(stores);
        return stores;
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public Store UpdateStore(Store store) {
      try {
        _storesCol.ReplaceOne(filter: new BsonDocument("_id", store.Id), replacement: store);
        return store;
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public IEnumerable<Store> UpdateStores(IEnumerable<Store> stores) {
      List<Task> tasks = new List<Task>();
      try {
        foreach (var store in stores) {
          tasks.Add(_storesCol.ReplaceOneAsync(filter: new BsonDocument("_id", store.Id), replacement: store));
        }
        Task.WhenAll(tasks.ToArray()).Wait();
        return stores;
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public IEnumerable<Store> GetStores() {
      try {
        return _storesCol.Find(FilterDefinition<Store>.Empty).ToList();
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public IEnumerable<Store> GetStores(Func<Store, bool> filter, Func<Store, int> ordering, int orderBy) {
      try {
        return _storesCol.AsQueryable().Where(filter).OrderBy(ordering, (OrderByDirection)orderBy);
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
