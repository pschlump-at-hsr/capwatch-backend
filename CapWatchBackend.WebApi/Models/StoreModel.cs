using CapWatchBackend.Domain.Entities;
using System;

namespace CapWatchBackend.WebApi.Models {
  public class StoreModel {
    public StoreModel(Store store) {
      Id = store.Id;
      Name = store.Name;
      Street = store.Street;
      ZipCode = store.ZipCode;
      MaxCapacity = store.MaxCapacity;
      CurrentCapacity = store.CurrentCapacity;
      Logo = store.Logo;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public int MaxCapacity { get; set; }
    public int CurrentCapacity { get; set; }
    public byte[] Logo { get; set; }
    public String Secret { get; set; }
  }
}
