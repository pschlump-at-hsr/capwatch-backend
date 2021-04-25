using AutoMapper;
using CapWatchBackend.Domain.Entities;
using CapWatchBackend.WebApi.Mapper;
using CapWatchBackend.WebApi.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CapWatchBackend.WebApi.Tests.ModelTests {
  public class StoreNewTests {
    private readonly IMapper _mapper;

    public StoreNewTests() {
      _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile())).CreateMapper();
    }


    [Theory]
    [InlineData("Ikea", "Zürcherstrasse 460", "9015", "St. Gallen", 135, "Test", "c73e9c5f-de5c-479a-b116-7ee1b93ab4f9", "Detailhändler")]
    [InlineData("Zoo Zürich", "Zürichbergstrasse 221", "8044", "Zürich", 487, "asdf", "7b0523b7-4efd-4fdf-b11d-3f4d26cf7b19", "Freizeit")]
    [InlineData("Polenmuseum - Schloss Rapperswil", "Schloss", "8640", "Raperswil-Jona", 11, "jkl", "f58957ce-fb83-4f62-ac2c-6d1fe810d85c", "Bank")]
    public void TestMapper(string name, string street, string zipCode, string city, int maxCapacity, string logo, string typeId, string typeDescription) {
      var newStore = new NewStoreModel {
        Name = name,
        Street = street,
        ZipCode = zipCode,
        City = city,
        MaxCapacity = maxCapacity,
        Logo = Encoding.UTF8.GetBytes(logo),
        StoreType = new StoreTypeModel { Id = typeId, Description = typeDescription }
      };

      var store = _mapper.Map<Store>(newStore);


      store.Name.Should().Be(name);
      store.Street.Should().Be(street);
      store.ZipCode.Should().Be(zipCode);
      store.City.Should().Be(city);
      store.MaxCapacity.Should().Be(maxCapacity);
      store.Logo.Should().BeEquivalentTo(Encoding.UTF8.GetBytes(logo));
      store.StoreType.Id.Should().Be(typeId);
      store.StoreType.Description.Should().Be(typeDescription);
    }
  }
}
