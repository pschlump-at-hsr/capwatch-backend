using System;
using System.ComponentModel.DataAnnotations;

namespace CapWatchBackend.WebApi.Models {
  public class NewStoreModel {

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
    [Required]
    public StoreTypeModel StoreType { get; set; }
  }
}
