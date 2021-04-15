using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapWatchBackend.WebApi.Models {
  public class StoreOverview {
    public StoreOverview() { }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public int MaxCapacity { get; set; }
    public int CurrentCapacity { get; set; }
    public byte[] Logo { get; set; }
    public StoreType Type { get; set; }
  }
}
