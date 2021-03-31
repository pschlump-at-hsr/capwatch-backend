using System;

namespace CapWatchBackend.DataAccess.MongoDB.ModelClasses {
  class StoreDto {
    public int Id { get; set; }
    public Guid Secret { get; set; }
    public string Name { get; set; }

  }
}
