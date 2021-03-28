using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CapWatchBackend.Model
{
    public class StoreModel
    {
        const string connectionString = "mongodb://capwusr:capwusr123@localhost:27017/admin";
        private static MongoClient client;
        private IMongoCollection<BsonDocument> storesCol;
        private IMongoCollection<BsonDocument> capacitiesCol;
        public StoreModel() {
            try {
                client = new MongoClient(connectionString);
            } catch (MongoClientException e)
            {
                Console.WriteLine("Error opening Database. Message = {0}", e.Message);
            }
            
            var database = client.GetDatabase("capwatchDB");
            storesCol = database.GetCollection<BsonDocument>("stores").WithWriteConcern(WriteConcern.WMajority);
            capacitiesCol = database.GetCollection<BsonDocument>("capacities").WithWriteConcern(WriteConcern.WMajority);
            BsonClassMap.RegisterClassMap<Shop>(cm =>
            {
                cm.AutoMap();
            });
        }

        public async void persistStoresAsync(IEnumerable<Shop> stores)
        {
            List<BsonDocument> docs = new List<BsonDocument>();
            foreach (Shop store in stores) {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("name", store.Name);
                dictionary.Add("secret", store.Secret);
                docs.Add(new BsonDocument(dictionary));
            }
            await storesCol.InsertManyAsync(docs);
        }

        public async void persistCapacitiesAsync(IEnumerable<Shop> stores)
        {
            List<BsonDocument> docs = new List<BsonDocument>();
            foreach (Shop store in stores)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("secret", store.Secret);
                dictionary.Add("timestamp", DateTime.UtcNow);
                dictionary.Add("capacity", store.CurrentCapacity);
                dictionary.Add("maxCapacity", store.MaxCapacity);
                docs.Add(new BsonDocument(dictionary));
            }
            await capacitiesCol.InsertManyAsync(docs);
        }

        public IEnumerable<Shop> getStores()
        {
            var filter = Builders<BsonDocument>.Filter.Empty;
            List<BsonDocument> docs = storesCol.Find(filter).ToList<BsonDocument>();
            var query = from p in capacitiesCol.AsQueryable()
                        //join s in storesCol.AsQueryable() on p.StoreId equals s.Secret into joined
                        select new { p };


            Console.Write(docs);
            return null;
            
        }
    }
}
