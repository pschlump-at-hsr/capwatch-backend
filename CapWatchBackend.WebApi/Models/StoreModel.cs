using CapWatchBackend.Domain.Entities;

namespace CapWatchBackend.WebApi.Models {
  public class StoreModel : EntityModel {
    public StoreModel(Store store) {
      Data = new {
        store.Name,
        store.MaxCapacity,
        store.CurrentCapacity
      };
      Links = new {
        Update = $"/stores/{store.Id}"
      };
    }
  }
}
