using CapWatchBackend.Domain.Entities;

namespace CapWatchBackend.WebApi.Models {
  public class StoreOverview {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public int MaxCapacity { get; set; }
    public int CurrentCapacity { get; set; }
    public byte[] Logo { get; set; }
    public StoreType StoreType { get; set; }
  }
}
