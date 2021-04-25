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
  public sealed class StoreRepository : IStoreRepository {
    private readonly IMongoCollection<Store> _storesCol;
    private readonly IMongoCollection<StoreType> _typesCol;

    public StoreRepository(IOptions<DatabaseConfiguration> options) {
      try {
        var capWatchDbo = CapwatchDbo.GetInstance(options.Value.ConnectionString);
        _storesCol = capWatchDbo.GetStoreCollection();
        _typesCol = capWatchDbo.GetTypeCollection();
      } catch (RepositoryException e) {
        DbLogger.Log(e.Message);
      }
    }

    public StoreRepository(string connectionString) {
      var capWatchDbo = CapwatchDbo.GetInstance(connectionString);
      _storesCol = capWatchDbo.GetStoreCollection();
      _typesCol = capWatchDbo.GetTypeCollection();
    }

    public bool IsValidType(StoreType storeType) {
      return _typesCol.AsQueryable().Any(type => type.Id == storeType.Id);
    }

    public Task AddStoreAsync(Store store) {
      try {
        if (IsValidType(store.StoreType)) {
          store.Id = (Guid)GuidGenerator.Instance.GenerateId(_storesCol, store);
          return _storesCol.InsertOneAsync(store);
        } else {
          throw new StoreTypeInvalidException();
        }
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public async Task AddStoresAsync(IEnumerable<Store> stores) {
      try {
        foreach (var store in stores) {
          if (IsValidType(store.StoreType)) {
            store.Id = (Guid)GuidGenerator.Instance.GenerateId(_storesCol, store);
          } else {
            throw new StoreTypeInvalidException();
          }
        }
        await _storesCol.InsertManyAsync(stores);
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public async Task UpdateStoreAsync(Store store) {
      try {
        if (IsValidType(store.StoreType)) {
          await _storesCol.ReplaceOneAsync(filter: new BsonDocument("_id", new BsonBinaryData(store.Id, GuidRepresentation.Standard)), replacement: store);
        } else {
          throw new StoreTypeInvalidException();
        }
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public async Task UpdateStoresAsync(IEnumerable<Store> stores) {
      List<Task> tasks = new List<Task>();
      try {
        foreach (var store in stores) {
          if (IsValidType(store.StoreType)) {
            tasks.Add(_storesCol.ReplaceOneAsync(filter: new BsonDocument("_id", new BsonBinaryData(store.Id, GuidRepresentation.Standard)), replacement: store));
          } else {
            throw new StoreTypeInvalidException();
          }
        }
        await Task.WhenAll(tasks.ToArray());
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public Task<IEnumerable<Store>> GetStoresAsync() {
      try {
        return Task.Factory.StartNew(() => {
          return (IEnumerable<Store>)_storesCol.Find(FilterDefinition<Store>.Empty).ToList();
        });
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public Task<IEnumerable<Store>> GetStoresAsync(Func<Store, bool> filter, Func<Store, int> ordering, int orderBy) {
      try {
        return Task.Factory.StartNew(() => {
          return (IEnumerable<Store>)_storesCol.AsQueryable()
          .Where(filter)
          .OrderBy(ordering, (OrderByDirection)orderBy);
        });
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public Task<Store> GetStoreAsync(Guid id) {
      try {
        return Task.Factory.StartNew(() => {
          return _storesCol.AsQueryable().FirstOrDefault(store => store.Id == id);
        });
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public async void DeleteAllStoresAsync() {
      try {
        await _storesCol.DeleteManyAsync(FilterDefinition<Store>.Empty);
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public async void DeleteStoreAsync(Store store) {
      try {
        await _storesCol.DeleteManyAsync(new BsonDocument("storeId", new BsonBinaryData(store.Id, GuidRepresentation.Standard)));
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

  }
}
