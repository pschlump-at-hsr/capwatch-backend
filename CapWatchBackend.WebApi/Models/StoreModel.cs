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

    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    [Range(0,Int32.MaxValue)]
    public int MaxCapacity { get; set; }
    [Range(0, Int32.MaxValue)]
    public int CurrentCapacity { get; set; }
    public byte[] Logo { get; set; }
    public String Secret { get; set; }
  }
}
