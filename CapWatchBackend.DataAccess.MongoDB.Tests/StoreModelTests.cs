using CapWatchBackend.DataAccess.MongoDB.Repositories;
using CapWatchBackend.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace CapWatchBackend.DataAccess.MongoDB.Tests {

  public class StoreModelTests {

    StoreRepository sm;
    public StoreModelTests() {
      sm = new StoreRepository("mongodb://capwusr:capwusr123@152.96.56.34:27018/admin");
      sm.DeleteAllStores();
    }

    [Fact]
    public void TestAddStore() {
      Store store = new Store { Name = "Botanischer Garten der Universit�t Bern", Secret = new Guid("57bd0e44-4032-4a9c-b48e-c89d990bcd6f"), Street = "Altenbergrain 21", ZipCode = "3013", City = "Bern", CurrentCapacity = 103, MaxCapacity = 103 };
      var res = sm.AddStore(store);
      res.Id.Should().BeGreaterThan(0);
      sm.GetStore(res.Id).Name.Should().Be("Botanischer Garten der Universit�t Bern");
    }

    [Fact]
    public void TestGetStores() {
      List<Store> stores = new List<Store>();
      stores.Add(new Store { Name = "Ikea", Secret = new Guid("02a4299e-f472-41d1-9468-df369bd30872"), Street = "Z�rcherstrasse 460", ZipCode = "9015", City = "St. Gallen", CurrentCapacity = 135, MaxCapacity = 201 });
      stores.Add(new Store { Name = "Zoo Z�rich", Secret = new Guid("9e573805-f5b4-49bb-bbaf-844b44ea3942"), Street = "Z�richbergstrasse 221", ZipCode = "8044", City = "Z�rich", CurrentCapacity = 487, MaxCapacity = 1125 });
      stores.Add(new Store { Name = "Polenmuseum - Schloss Rapperswil", Secret = new Guid("048cedbd-78d0-4837-9f6f-334aebb7a04e"), Street = "Schloss", ZipCode = "8640", City = "Raperswil-Jona", CurrentCapacity = 11, MaxCapacity = 62 });
      stores.Add(new Store { Name = "Alpamare", Secret = new Guid("9f1fb7fb-5e9e-4ebd-90d7-30f82ebec8e7"), Street = "Gwattstrasse 12", ZipCode = "8808", City = "Pf�ffikon", CurrentCapacity = 152, MaxCapacity = 612 });
      sm.AddStores(stores);
      stores = (List<Store>)sm.GetStores();
      stores.Count.Should().Be(4);
      foreach (var store in stores) {
        store.Name.Should().NotBeNullOrEmpty();
      }
    }

    [Fact]
    public void TestUpdateStore() {
      Store store = new Store { Name = "Botanischer Garten der Universit�t Bern", Street = "Altenbergrain 21", ZipCode = "3013", City = "Bern", CurrentCapacity = 103, MaxCapacity = 103 };
      store = sm.AddStore(store);
      store = new Store { Id = store.Id, Name = "Botanischer Garten St. Gallen", Street = "Keine Ahnung", ZipCode = "9008", City = "Bern", CurrentCapacity = 103, MaxCapacity = 103 };
      store = sm.UpdateStore(store);
      sm.GetStore(store.Id).Name.Should().Be("Botanischer Garten St. Gallen");
    }
  }
}
