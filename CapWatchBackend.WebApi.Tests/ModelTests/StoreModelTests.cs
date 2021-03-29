using CapWatchBackend.Domain.Entities;
using CapWatchBackend.WebApi.Models;
using FluentAssertions;
using Xunit;

namespace CapWatchBackend.WebApi.Tests.ModelTests {
  public class StoreModelTests {

    [Theory]
    [InlineData(1, "Migros St. Gallen", 70, 180)]
    [InlineData(2, "Säntispark Bäder", 125, 150)]
    [InlineData(3, "Interdiscount", 7, 26)]
    public void TestConstructor(int id, string name, int maxCapacity, int currentCapacity) {
      var store = new Store {
        Id = id,
        Name = name,
        MaxCapacity = maxCapacity,
        CurrentCapacity = currentCapacity
      };

      var model = new StoreModel(store);

      model.Id.Should().Be(id);
      model.Name.Should().Be(name);
      model.MaxCapacity.Should().Be(maxCapacity);
      model.CurrentCapacity.Should().Be(currentCapacity);
    }

  }
}
