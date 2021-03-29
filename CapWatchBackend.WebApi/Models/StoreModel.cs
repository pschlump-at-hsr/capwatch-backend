using CapWatchBackend.Domain.Entities;

namespace CapWatchBackend.WebApi.Models {
  public class StoreModel {
    public StoreModel(Store store) {
      Id = store.Id;
      Name = store.Name;
      MaxCapacity = store.MaxCapacity;
      CurrentCapacity = store.CurrentCapacity;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public int MaxCapacity { get; set; }
    public int CurrentCapacity { get; set; }
  }
}
