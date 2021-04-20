using AutoMapper;
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
    [InlineData(1, "Ikea", "Zürcherstrasse 460", "9015", "St. Gallen", 135, 201, "Test")]
    [InlineData(2, "Zoo Zürich", "Zürichbergstrasse 221", "8044", "Zürich", 487, 1125, "asdf")]
    [InlineData(3, "Polenmuseum - Schloss Rapperswil", "Schloss", "8640", "Raperswil-Jona", 11, 62, "jkl")]
    public void TestConstructor(int id, string name, string street, string zipCode, string city, int maxCapacity, int currentCapacity, string logo) {
      var store = new StoreModel {
        Id = id,
        Name = name,
        Street = street,
        ZipCode = zipCode,
        City = city,
        MaxCapacity = maxCapacity,
        CurrentCapacity = currentCapacity,
        Logo = Encoding.UTF8.GetBytes(logo),
      };

      var model = _mapper.Map<StoreModel>(store);

      model.Id.Should().Be(id);
      model.Name.Should().Be(name);
      model.Street.Should().Be(street);
      model.ZipCode.Should().Be(zipCode);
      model.City.Should().Be(city);
      model.MaxCapacity.Should().Be(maxCapacity);
      model.CurrentCapacity.Should().Be(currentCapacity);
      model.Logo.Should().BeEquivalentTo(Encoding.UTF8.GetBytes(logo));

    }
  }
}
