using CapWatchBackend.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace CapWatchBackend.WebApi.Models {
  public class StoreModel {
    public StoreModel() { }
    public StoreModel(Store store) {
      Id = store.Id;
      Name = store.Name;
      Street = store.Street;
      ZipCode = store.ZipCode;
      City = store.City;
      MaxCapacity = store.MaxCapacity;
      CurrentCapacity = store.CurrentCapacity;
      Logo = store.Logo;
    }

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
    public String Secret { get; set; }
  }
}
