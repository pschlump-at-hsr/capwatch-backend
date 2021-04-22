using CapWatchBackend.Application.Exceptions;
using CapWatchBackend.Application.Repositories;
using CapWatchBackend.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CapWatchBackend.DataAccess.MongoDB.Repositories {
  public class StoreRepository : IStoreRepository {
    private readonly IMongoCollection<Store> _storesCol;
    private readonly ITypeRepository _type;
    public StoreRepository(IOptions<ConfigureDatabase> options, ITypeRepository type) {
      try {
        var capWatchDbo = CapwatchDbo.GetInstance(options.Value.ConnectionString);
        _storesCol = capWatchDbo.GetStoreCollection();
        _type = type;
      } catch (RepositoryException e) {
        DbLogger.Log(e.Message);
      }
    }

    public StoreRepository(string connectionString, ITypeRepository type) {
      var capWatchDbo = CapwatchDbo.GetInstance(connectionString);
      _storesCol = capWatchDbo.GetStoreCollection();
      _type = type;
    }

    public async Task AddStoreAsync(Store store) {
      try {
        if (_type.IsValidType(store.StoreType)) {
          store.Id = (Guid)GuidGenerator.Instance.GenerateId(_storesCol, store);
          await _storesCol.InsertOneAsync(store);
        } else {
          throw new TypeInvalidException();
        }
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public async Task AddStoresAsync(IEnumerable<Store> stores) {
      try {
        foreach (var store in stores) {
          if (_type.IsValidType(store.StoreType)) {
            store.Id = (Guid)GuidGenerator.Instance.GenerateId(_storesCol, store);
          } else {
            throw new TypeInvalidException();
          }
        }
        await _storesCol.InsertManyAsync(stores);
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public async Task UpdateStoreAsync(Store store) {
      try {
        if (_type.IsValidType(store.StoreType)) {
          await _storesCol.ReplaceOneAsync(filter: new BsonDocument("_id", new BsonBinaryData(store.Id, GuidRepresentation.Standard)), replacement: store);
        } else {
          throw new TypeInvalidException();
        }
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public async Task UpdateStoresAsync(IEnumerable<Store> stores) {
      List<Task> tasks = new List<Task>();
      try {
        foreach (var store in stores) {
          if (_type.IsValidType(store.StoreType)) {
            tasks.Add(_storesCol.ReplaceOneAsync(filter: new BsonDocument("_id", new BsonBinaryData(store.Id, GuidRepresentation.Standard)), replacement: store));
          } else {
            throw new TypeInvalidException();
          }
        }
        await Task.WhenAll(tasks.ToArray());
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

    public Store GetStore(Guid id) {
      try {
        return _storesCol.AsQueryable().Where(x => x.Id == id).FirstOrDefault();
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
        await _storesCol.DeleteManyAsync(new BsonDocument("storeId", new BsonBinaryData(store.Id, GuidRepresentation.Standard)));
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

  }
}
