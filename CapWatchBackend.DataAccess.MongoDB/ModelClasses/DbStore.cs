using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CapWatchBackend.DataAccess.MongoDB.ModelClasses {
  class DbStore {

    [BsonId]
    public ObjectId Id { get; set; }
    [BsonElement("secret")]
    public int Secret { get; set; }
    [BsonElement("name")]
    public string Name { get; set; }

  }
}
