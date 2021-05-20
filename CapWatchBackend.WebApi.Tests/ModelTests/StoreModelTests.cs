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
      _mapper = new MapperConfiguration(config => config.AddProfile(new MapperProfile())).CreateMapper();
    }

    [Theory]
    [InlineData("10000000-0000-0000-0000-000000000000", "Ikea", "Zuercherstrasse 460", "9015", "St. Gallen", 135, 201, "Test", "00000000-0000-0000-0000-000000000001", "00000000-1000-0000-0000-000000000000", "Detailhaendler")]
    [InlineData("20000000-0000-0000-0000-000000000000", "Zoo Zuerich", "Zuerichbergstrasse 221", "8044", "Zuerich", 487, 1125, "asdf", "00000000-0000-0000-0000-000000000002", "00000000-2000-0000-0000-000000000000", "Freizeit")]
    [InlineData("30000000-0000-0000-0000-000000000000", "Polenmuseum - Schloss Rapperswil", "Schloss", "8640", "Rapperswil-Jona", 11, 62, "jkl", "00000000-0000-0000-0000-000000000003", "00000000-3000-0000-0000-000000000000", "Bank")]
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
        StoreType = new StoreTypeModel {
          Id = typeId,
          Description = typeDescription
        }
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
