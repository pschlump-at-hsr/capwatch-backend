using System;

namespace CapWatchBackend.Domain.Entities {
  public class Store : IEntity {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public int MaxCapacity { get; set; }
    public int CurrentCapacity { get; set; }
    public byte[] Logo { get; set; }
    public Guid Secret { get; set; }
  }
}
