using System.Collections.Generic;

namespace CapWatchBackend.WebApi.Models {
  public class EntityListModel {
    public EntityListModel(IEnumerable<object> entities) {
      Data = entities;
    }
    public IEnumerable<object> Data { get; set; }
    public object Links { get; set; }
  }
}
