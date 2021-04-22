using CapWatchBackend.DataAccess.MongoDB.Repositories;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace CapWatchBackend.DataAccess.MongoDB.Tests {
  public class TypeRepositoryTests {

    private readonly TypeRepository _typeRepository;
    public TypeRepositoryTests() {
      _typeRepository = new TypeRepository("mongodb://capwusr:capwusr123@localhost:27017/admin");
    }

    [Fact]
    public void TestAddTypesAsync() {
      List<string> descriptions = new List<string>{
        "Detailhändler",
        "Freizeit",
        "Bank",
        "Restaurant",
        "Kino",
        "Bar"
      };
      var types = _typeRepository.AddTypesAsync(descriptions);
      types.Count.Should().Be(6);

    }
  }
}
