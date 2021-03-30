using System;

namespace CapWatchBackend.DataAccess.MongoDB.ModelClasses {
  class StoreDto {
    public Guid Id { get; set; }
    public int Secret { get; set; }
    public string Name { get; set; }

  }
}
