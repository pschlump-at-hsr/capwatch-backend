using System;
using System.ComponentModel.DataAnnotations;

namespace CapWatchBackend.WebApi.Models {
  public class StoreModel {
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Street { get; set; }
    [Required]
    public string ZipCode { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int MaxCapacity { get; set; }
    [Required]
    [Range(0, int.MaxValue)]
    public int CurrentCapacity { get; set; }
    public byte[] Logo { get; set; }
    [Required]
    public string Secret { get; set; }
    [Required]
    public StoreTypeModel Type { get; set; }
  }
}
