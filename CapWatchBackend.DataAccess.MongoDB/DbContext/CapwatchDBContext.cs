using CapWatchBackend.Application.Exceptions;
using CapWatchBackend.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CapWatchBackend.DataAccess.MongoDB.Tests")]
[assembly: InternalsVisibleTo("CapWatchBackend.WebApi")]
namespace CapWatchBackend.DataAccess.MongoDB.DbContext {
  internal class CapwatchDBContext : ICapwatchDBContext {

    private readonly string _storeCollectionName = "stores";
    private readonly string _storeTypeCollectionName = "storeTypes";
    private readonly IMongoDatabase _database;

    public CapwatchDBContext(IOptions<DatabaseConfiguration> options) {
      try {
        var client = new MongoClient(options.Value.ConnectionString);
        _database = client.GetDatabase(options.Value.DatabaseName);
        SetupDatabaseMapping();
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public IMongoCollection<Store> GetStoreCollection() {
      try {
        return _database.GetCollection<Store>(_storeCollectionName).WithWriteConcern(WriteConcern.WMajority);
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    public IMongoCollection<StoreType> GetTypeCollection() {
      try {
        return _database.GetCollection<StoreType>(_storeTypeCollectionName).WithWriteConcern(WriteConcern.WMajority);
      } catch (MongoClientException e) {
        throw new RepositoryException(e.Message, e);
      }
    }

    private void SetupDatabaseMapping() {
      //Due to a bug this obsolete method is necessary
#pragma warning disable CS0618 // Type or member is obsolete
      BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
#pragma warning restore CS0618 // Type or member is obsolete

      if (!BsonClassMap.IsClassMapRegistered(typeof(StoreType))) {
        BsonClassMap.RegisterClassMap<StoreType>(
         map => {
           map.MapProperty(storeType => storeType.Description).SetElementName("name");
           map.MapProperty(storeType => storeType.Id).SetElementName("_id").SetIdGenerator(GuidGenerator.Instance).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
         });
      }

      if (!BsonClassMap.IsClassMapRegistered(typeof(Store))) {
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
           map.MapProperty(store => store.StoreType).SetElementName("storeType").SetSerializer(new BsonClassMapSerializer<StoreType>(BsonClassMap.LookupClassMap(typeof(StoreType))));
         });
      }
    }
  }
}
