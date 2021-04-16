using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CapWatchBackend.WebApi.Models {
  public class StoreNew {

    [Required]
    public string Name { get; set; }
    [Required]
    public string Street { get; set; }
    [Required]
    public string ZipCode { get; set; }
    [Required]
    public string City { get; set; }
    [Range(1, int.MaxValue)]
    public int? MaxCapacity { get; set; }
    public byte[] Logo { get; set; }
  }
}
