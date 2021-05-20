using System;

namespace CapWatchBackend.Domain {
  public interface IEntity {
    Guid Id { get; set; }
  }
}
