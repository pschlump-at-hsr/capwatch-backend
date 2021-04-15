using CapWatchBackend.Domain.Entities;
using CapWatchBackend.WebApi.Models;
using FluentAssertions;
using System;
using System.Diagnostics;
using System.Text;
using Xunit;

namespace CapWatchBackend.WebApi.Tests.ModelTests {
  public class StoreModelTests {

    [Theory]
    [InlineData(1, "Ikea", "Zürcherstrasse 460", "9015", "St. Gallen", 135, 201, "Test")]
    [InlineData(2, "Zoo Zürich", "Zürichbergstrasse 221", "8044", "Zürich", 487, 1125, "asdf")]
    [InlineData(3, "Polenmuseum - Schloss Rapperswil", "Schloss", "8640", "Raperswil-Jona", 11, 62, "jkl")]
    public void TestConstructor(int id, string name, string street, string zipCode, string city, int maxCapacity, int currentCapacity, string logo) {
      var store = new Store {
        Id = id,
        Name = name,
        Street = street,
        ZipCode = zipCode,
        City = city,
        MaxCapacity = maxCapacity,
        CurrentCapacity = currentCapacity,
        Logo = Encoding.UTF8.GetBytes(logo),
      };

      var model = new StoreModel(store);

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
