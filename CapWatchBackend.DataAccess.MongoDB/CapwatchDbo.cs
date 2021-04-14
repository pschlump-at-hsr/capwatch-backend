using CapWatchBackend.Application.Repositories;
using CapWatchBackend.DataAccess.MongoDB.Repositories;
using CapWatchBackend.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace CapWatchBackend.DataAccess.MongoDB {
  class CapwatchDbo {

    private static string _connectionString;

    private static CapwatchDbo _instance;
    private MongoClient _client;
    private IMongoDatabase _database;
    private CapwatchDbo() {
      try {
        _client = new MongoClient(_connectionString);
        _database = _client.GetDatabase("capwatchDB");
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
      BsonClassMap.RegisterClassMap<Store>(
       map => {
         map.MapProperty(x => x.Name).SetElementName("name");
         map.MapProperty(x => x.Street).SetElementName("street");
         map.MapProperty(x => x.ZipCode).SetElementName("zipCode");
         map.MapProperty(x => x.City).SetElementName("city");
         map.MapProperty(x => x.Logo).SetElementName("logo");
         map.MapProperty(x => x.Secret).SetElementName("secret")
         .SetSerializer(new GuidSerializer(BsonType.String));
         map.MapProperty(x => x.Id).SetElementName("_id")
         .SetIdGenerator(IntIdGenerator.Instance);
         map.MapProperty(x => x.CurrentCapacity).SetElementName("currentCapacity");
         map.MapProperty(x => x.MaxCapacity).SetElementName("maxCapacity");
       });
    }
    public IMongoCollection<Store> GetStoreCollection() {
      try {
        return _database.GetCollection<Store>("stores").WithWriteConcern(WriteConcern.WMajority);
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }
  }
}
