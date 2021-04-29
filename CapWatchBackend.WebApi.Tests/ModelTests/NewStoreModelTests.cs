using AutoMapper;
using CapWatchBackend.Domain.Entities;
using CapWatchBackend.WebApi.Mapper;
using CapWatchBackend.WebApi.Models;
using FluentAssertions;
using System.Text;
using Xunit;

namespace CapWatchBackend.WebApi.Tests.ModelTests {
  public class NewStoreModelTests {
    private readonly IMapper _mapper;

    public NewStoreModelTests() {
      _mapper = new MapperConfiguration(config => config.AddProfile(new MapperProfile())).CreateMapper();
    }

    [Theory]
    [InlineData("Ikea", "Zuercherstrasse 460", "9015", "St. Gallen", 135, "Test", "00000000-1000-0000-0000-000000000000", "Detailhaendler")]
    [InlineData("Zoo Zuerich", "Zuerichbergstrasse 221", "8044", "Zuerich", 487, "asdf", "00000000-2000-0000-0000-000000000000", "Freizeit")]
    [InlineData("Polenmuseum - Schloss Rapperswil", "Schloss", "8640", "Rapperswil-Jona", 11, "jkl", "00000000-3000-0000-0000-000000000000", "Bank")]
    public void TestMapper(string name, string street, string zipCode, string city, int maxCapacity, string logo, string typeId, string typeDescription) {
      var newStore = new NewStoreModel {
        Name = name,
        Street = street,
        ZipCode = zipCode,
        City = city,
        MaxCapacity = maxCapacity,
        Logo = Encoding.UTF8.GetBytes(logo),
        StoreType = new StoreTypeModel {
          Id = typeId,
          Description = typeDescription
        }
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
