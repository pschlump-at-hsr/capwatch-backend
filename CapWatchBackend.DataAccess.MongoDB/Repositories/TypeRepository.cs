using CapWatchBackend.Application.Exceptions;
using CapWatchBackend.Application.Repositories;
using CapWatchBackend.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapWatchBackend.DataAccess.MongoDB.Repositories {
  public class TypeRepository : ITypeRepository {
    private readonly IMongoCollection<StoreType> _typesCol;

    public TypeRepository(IOptions<ConfigureDatabase> options) {
      try {
        var capWatchDbo = CapwatchDbo.GetInstance(options.Value.ConnectionString);
        _typesCol = capWatchDbo.GetTypeCollection();
      } catch (RepositoryException e) {
        DbLogger.Log(e.Message);
      }
    }

    public TypeRepository(string connectionString) {
      try {
        var capWatchDbo = CapwatchDbo.GetInstance(connectionString);
        _typesCol = capWatchDbo.GetTypeCollection();
      } catch (RepositoryException e) {
        DbLogger.Log(e.Message);
      }
    }

    public StoreType AddTypeAsync(string description) {
      try {
        var type = new StoreType();
        type.Description = description;
        type.Id = (Guid)GuidGenerator.Instance.GenerateId(_typesCol, type);
        _typesCol.InsertOne(type);
        return type;
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public List<StoreType> AddTypesAsync(List<string> descriptions) {
      var types = new List<StoreType>();
      try {
        foreach (var description in descriptions) {
          var type = new StoreType();
          type.Description = description;
          type.Id = (Guid)GuidGenerator.Instance.GenerateId(_typesCol, type);
          types.Add(type);
        }
        _typesCol.InsertMany(types);
        return types;
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public Boolean IsValidType(StoreType type) {
      var blah = _typesCol.AsQueryable().Where(x => x.Id == type.Id);
      return blah.Count() == 1;
    }
  }
}
