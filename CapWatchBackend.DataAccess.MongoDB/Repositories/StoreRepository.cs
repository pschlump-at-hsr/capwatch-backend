using CapWatchBackend.Application.Exceptions;
using CapWatchBackend.Application.Repositories;
using CapWatchBackend.DataAccess.MongoDB.DbContext;
using CapWatchBackend.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CapWatchBackend.DataAccess.MongoDB.Repositories {
  public sealed class StoreRepository : IStoreRepository {
    private readonly IMongoCollection<Store> _storesCol;
    private readonly IMongoCollection<StoreType> _typesCol;

    public StoreRepository(ICapwatchDBContext dbContext, ILogger<StoreRepository> logger) {
      try {
        _storesCol = dbContext.GetStoreCollection();
        _typesCol = dbContext.GetTypeCollection();
      } catch (RepositoryException exception) {
        logger.LogError(exception, exception.Message);
        throw;
      }
    }

    public async Task AddStoreAsync(Store store) {
      try {
        if (IsValidType(store.StoreType)) {
          store.Id = (Guid)GuidGenerator.Instance.GenerateId(_storesCol, store);
          await _storesCol.InsertOneAsync(store);
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
      var tasks = new List<Task>();
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

    public async Task<IEnumerable<Store>> GetStoresAsync() {
      try {
        return await Task.Factory.StartNew(() => {
          return (IEnumerable<Store>)_storesCol.Find(FilterDefinition<Store>.Empty).ToList();
        });
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public async Task<IEnumerable<Store>> GetStoresAsync(Expression<Func<Store, bool>> filter) {
      try {
        return await Task.Factory.StartNew(() => {
          return _storesCol.Find(filter).ToList();
        });
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public async Task<Store> GetStoreAsync(Guid id) {
      try {
        return await Task.Factory.StartNew(() => {
          return _storesCol.Find(store => store.Id == id).Limit(1).SingleOrDefault();
        });
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public async Task DeleteAllStoresAsync() {
      try {
        await _storesCol.DeleteManyAsync(FilterDefinition<Store>.Empty);
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public async Task DeleteStoreAsync(Store store) {
      try {
        await _storesCol.DeleteManyAsync(new BsonDocument("_id", new BsonBinaryData(store.Id, GuidRepresentation.Standard)));
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    private bool IsValidType(StoreType storeType) {
      return _typesCol.CountDocuments(type => type.Id == storeType.Id) > 0;
    }
  }
}
