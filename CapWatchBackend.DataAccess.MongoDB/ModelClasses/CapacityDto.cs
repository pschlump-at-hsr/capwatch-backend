using System;

namespace CapWatchBackend.DataAccess.MongoDB.ModelClasses {
  class CapacityDto {

    public int Id { get; set; }

    public int StoreId { get; set; }

    public int Capacity { get; set; }

    public int MaxCapacity { get; set; }

    public DateTime Timestamp { get; set; }
  }
}
