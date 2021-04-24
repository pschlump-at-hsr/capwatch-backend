using System;

namespace CapWatchBackend.Domain.Entities {
  public class StoreType : IEntity {
    public Guid Id { get; set; }
    public string Description { get; set; }
  }
}
