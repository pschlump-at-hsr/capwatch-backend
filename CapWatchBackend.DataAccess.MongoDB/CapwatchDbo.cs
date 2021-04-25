using CapWatchBackend.Application.Exceptions;
using CapWatchBackend.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace CapWatchBackend.DataAccess.MongoDB {
  internal sealed class CapwatchDbo {
    private static string _connectionString;
    private static CapwatchDbo _instance;
    private readonly IMongoDatabase _database;

    private CapwatchDbo() {
      try {
        var client = new MongoClient(_connectionString);
        _database = client.GetDatabase("capwatchDB");
        SetupDatabaseMapping();
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public static CapwatchDbo GetInstance(string connectionString) {
      _connectionString = connectionString;
      if (_instance == null) {
        _instance = new CapwatchDbo();
      }
      return _instance;
    }

    private void SetupDatabaseMapping() {
      //Due to a bug this obsolete method is necessary
#pragma warning disable CS0618 // Type or member is obsolete
      BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
      BsonClassMap.RegisterClassMap<Store>(
       map => {
         map.MapProperty(store => store.Name).SetElementName("name");
         map.MapProperty(store => store.Street).SetElementName("street");
         map.MapProperty(store => store.ZipCode).SetElementName("zipCode");
         map.MapProperty(store => store.City).SetElementName("city");
         map.MapProperty(store => store.Logo).SetElementName("logo");
         map.MapProperty(store => store.Secret).SetElementName("secret").SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
         map.MapProperty(store => store.Id).SetElementName("_id").SetIdGenerator(GuidGenerator.Instance).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
         map.MapProperty(store => store.CurrentCapacity).SetElementName("currentCapacity");
         map.MapProperty(store => store.MaxCapacity).SetElementName("maxCapacity");
         map.MapProperty(store => store.StoreType).SetElementName("storeType");
       });

      BsonClassMap.RegisterClassMap<StoreType>(
       map => {
         map.MapProperty(storeType => storeType.Description).SetElementName("name");
         map.MapProperty(storeType => storeType.Id).SetElementName("_id").SetIdGenerator(GuidGenerator.Instance).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
       });
    }

    public IMongoCollection<Store> GetStoreCollection() {
      try {
        return _database.GetCollection<Store>("stores").WithWriteConcern(WriteConcern.WMajority);
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public IMongoCollection<StoreType> GetTypeCollection() {
      try {
        return _database.GetCollection<StoreType>("types").WithWriteConcern(WriteConcern.WMajority);
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }
  }
}
