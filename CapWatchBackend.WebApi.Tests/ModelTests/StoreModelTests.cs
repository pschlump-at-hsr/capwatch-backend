using AutoMapper;
using CapWatchBackend.Domain.Entities;
using CapWatchBackend.WebApi.Mapper;
using CapWatchBackend.WebApi.Models;
using FluentAssertions;
using System.Text;
using Xunit;

namespace CapWatchBackend.WebApi.Tests.ModelTests {
  public class StoreModelTests {
    private readonly IMapper _mapper;

    public StoreModelTests() {
      _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile())).CreateMapper();
    }

    [Theory]
    [InlineData("9c9cee44-c839-48f1-b54e-237d95fe5d7f", "Ikea", "Zürcherstrasse 460", "9015", "St. Gallen", 135, 201, "Test", "c73e9c5f-de5c-479a-b116-7ee1b93ab4f9", "c73e9c5f-de5c-479a-b116-7ee1b93ab4f9", "Detailhändler")]
    [InlineData("9c9cee44-c839-48f2-b54e-237d95fe5d7f", "Zoo Zürich", "Zürichbergstrasse 221", "8044", "Zürich", 487, 1125, "asdf", "9c9cee44-c839-48f2-b54e-236d95fe5d7f", "7b0523b7-4efd-4fdf-b11d-3f4d26cf7b19", "Freizeit")]
    [InlineData("9c9cee44-c839-48f3-b54e-237d95fe5d7f", "Polenmuseum - Schloss Rapperswil", "Schloss", "8640", "Raperswil-Jona", 11, 62, "jkl", "9c9cee44-c839-48f2-b54e-236d95fe5d7f", "f58957ce-fb83-4f62-ac2c-6d1fe810d85c", "Bank")]
    public void TestMapper(string id, string name, string street, string zipCode, string city, int maxCapacity, int currentCapacity, string logo, string secret, string typeId, string typeDescription) {
      var storeModel = new StoreModel {
        Id = id,
        Name = name,
        Street = street,
        ZipCode = zipCode,
        City = city,
        MaxCapacity = maxCapacity,
        CurrentCapacity = currentCapacity,
        Logo = Encoding.UTF8.GetBytes(logo),
        Secret = secret,
        StoreType = new StoreTypeModel { Id = typeId, Description = typeDescription }
      };

      var store = _mapper.Map<Store>(storeModel);

      store.Id.Should().Be(id);
      store.Name.Should().Be(name);
      store.Street.Should().Be(street);
      store.ZipCode.Should().Be(zipCode);
      store.City.Should().Be(city);
      store.MaxCapacity.Should().Be(maxCapacity);
      store.CurrentCapacity.Should().Be(currentCapacity);
      store.Logo.Should().BeEquivalentTo(Encoding.UTF8.GetBytes(logo));
      store.Secret.Should().Be(secret);
      store.StoreType.Id.Should().Be(typeId);
      store.StoreType.Description.Should().Be(typeDescription);
    }
  }
}
