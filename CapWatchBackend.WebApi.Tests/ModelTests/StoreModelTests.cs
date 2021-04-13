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
    [InlineData(1, "Ikea", "Zürcherstrasse 460", "9015", "St. Gallen", 135, 201, "Test", "5c8c774f11e1488d9487a2970ece5a59")]
    // [InlineData(2, "Zoo Zürich", "Zürichbergstrasse 221", "8044", "Zürich", 487, 1125)]
    // [InlineData(3, "Polenmuseum - Schloss Rapperswil", "Schloss", "8640", "Raperswil-Jona", 11, 62)]
    public void TestConstructor(int id, string name, string street, string zipCode, string city, int maxCapacity, int currentCapacity, string logo, string secret) {
      var store = new Store {
        Id = id,
        Name = name,
        Street = street,
        ZipCode = zipCode,
        City = city,
        MaxCapacity = maxCapacity,
        CurrentCapacity = currentCapacity,
        Logo = Encoding.UTF8.GetBytes(logo),
        Secret = Guid.Parse(secret)
      };

      var model = new StoreModel(store);

      Debug.WriteLine(Guid.Parse(secret));

      model.Id.Should().Be(id);
      model.Name.Should().Be(name);
      model.Street.Should().Be(street);
      model.ZipCode.Should().Be(zipCode);
      model.City.Should().Be(city);
      model.MaxCapacity.Should().Be(maxCapacity);
      model.CurrentCapacity.Should().Be(currentCapacity);
      model.Logo.Should().BeEquivalentTo(Encoding.UTF8.GetBytes(logo));
      // model.Secret.ToString();
    }

  }
}
