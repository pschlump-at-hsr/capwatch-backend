using CapWatchBackend.DataAccess.MongoDB.ModelClasses;
using CapWatchBackend.Domain.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapWatchBackend.DataAccess.MongoDB.Model {
  public class StoreModel {
    private static StoreModel instance;
    private const string connectionString = "mongodb://capwusr:capwusr123@localhost:27017/admin"; //todo: Auslagern in Settingsobjekt
    private MongoClient client;
    private IMongoCollection<DbStore> storesCol;
    private IMongoCollection<DbCapacity> capacitiesCol;
    private StoreModel() {
      try {
        client = new MongoClient(connectionString);
      } catch (MongoClientException e) {
        Console.WriteLine("Error opening Database. Message = {0}", e.Message);
      }

      var database = client.GetDatabase("capwatchDB");
      storesCol = database.GetCollection<DbStore>("stores").WithWriteConcern(WriteConcern.WMajority);
      capacitiesCol = database.GetCollection<DbCapacity>("capacities").WithWriteConcern(WriteConcern.WMajority);
    }

    public static StoreModel GetStoreModel() {
      if (instance == null) {
        instance = new StoreModel();
      }
      return instance;
    }

    public IEnumerable<Store> GetStores() {
      var filter = Builders<DbStore>.Filter.Empty;
      var query = storesCol.Find(filter).ToList();
      List<Store> stores = new List<Store>();
      foreach (var store in query) {
        var capacity = capacitiesCol.Find("{secret: " + store.Secret + "}").FirstOrDefault();
        stores.Add(new Store { Name = store.Name, CurrentCapacity = capacity.Capacity, MaxCapacity = capacity.MaxCapacity, Id = store.Secret });
      }
      return stores;

    }
  }
}
