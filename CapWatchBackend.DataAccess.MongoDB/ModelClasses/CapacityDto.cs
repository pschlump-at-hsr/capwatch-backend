using System;

namespace CapWatchBackend.DataAccess.MongoDB.ModelClasses {
  class CapacityDto {

    public Guid Id { get; set; }

    public Guid StoreId { get; set; }

    public int Capacity { get; set; }

    public int MaxCapacity { get; set; }

    public DateTime Timestamp { get; set; }
  }
}
