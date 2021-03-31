using CapWatchBackend.DataAccess.MongoDB.ModelClasses;
using CapWatchBackend.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapWatchBackend.DataAccess.MongoDB.Model {
  public class StoreModel {
    private static StoreModel _instance;
    private const string _connectionString = "mongodb://capwusr:capwusr123@localhost:27017/admin"; //TODO: Auslagern in Settingsobjekt
    private MongoClient _client;
    private IMongoCollection<StoreDto> _storesCol;
    private IMongoCollection<CapacityDto> _capacitiesCol;
    private StoreModel() {
      try { //TODO: Errorhandling ausbauen
        _client = new MongoClient(_connectionString);
      } catch (MongoClientException e) {
        Console.WriteLine("Error opening Database. Message = {0}", e.Message);
      }

      BsonClassMap.RegisterClassMap<StoreDto>(
       map => {
         map.MapProperty(x => x.Name).SetElementName("name");
         map.MapProperty(x => x.Secret).SetElementName("secret")
         .SetSerializer(new GuidSerializer(BsonType.String));
         map.MapProperty(x => x.Id).SetElementName("_id");
       });

      BsonClassMap.RegisterClassMap<CapacityDto>(
      map => {
        map.MapProperty(x => x.Id).SetElementName("_id");
        map.MapProperty(x => x.StoreId).SetElementName("storeId");
      });
      var database = _client.GetDatabase("capwatchDB");
      _storesCol = database.GetCollection<StoreDto>("stores").WithWriteConcern(WriteConcern.WMajority);
      _capacitiesCol = database.GetCollection<CapacityDto>("capacities").WithWriteConcern(WriteConcern.WMajority);
    }

    public static StoreModel GetStoreModel() {
      if (_instance == null) {
        _instance = new StoreModel();
      }
      return _instance;
    }

    public IEnumerable<Store> GetStores() {
      var filter = Builders<StoreDto>.Filter.Empty;
      var query = _storesCol.Find(filter).ToList();
      List<Store> stores = new List<Store>();
      foreach (var store in query) {
        var capacity = _capacitiesCol.AsQueryable().Where(x => x.StoreId == store.Id).MaxBy(x => x.Timestamp).FirstOrDefault();
        stores.Add(new Store { Name = store.Name, CurrentCapacity = capacity.Capacity, MaxCapacity = capacity.MaxCapacity, Id = store.Id });
      }
      return stores;
    }
  }
}
