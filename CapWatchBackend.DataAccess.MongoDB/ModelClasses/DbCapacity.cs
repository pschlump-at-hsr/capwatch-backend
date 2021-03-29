using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CapWatchBackend.DataAccess.MongoDB.ModelClasses {
  class DbCapacity {
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("secret")]
    public int Secret { get; set; }

    [BsonElement("capacity")]
    public int Capacity { get; set; }

    [BsonElement("maxCapacity")]
    public int MaxCapacity { get; set; }

    [BsonElement("timestamp")]
    public Double Timestamp { get; set; }
  }
}
