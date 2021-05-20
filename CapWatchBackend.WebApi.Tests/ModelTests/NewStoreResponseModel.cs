using AutoMapper;
using CapWatchBackend.Domain.Entities;
using CapWatchBackend.WebApi.Mapper;
using FluentAssertions;
using System.Text;
using Xunit;

namespace CapWatchBackend.WebApi.Tests.ModelTests {
  public class NewStoreResponseModel {
    private readonly IMapper _mapper;

    public NewStoreResponseModel() {
      _mapper = new MapperConfiguration(config => config.AddProfile(new MapperProfile())).CreateMapper();
    }

    [Theory]
    [InlineData("10000000-0000-0000-0000-000000000000", "Ikea", "Zuercherstrasse 460", "9015", "St. Gallen", 135, 201, "Test", "00000000-0000-0000-0000-000000000001", "00000000-1000-0000-0000-000000000000", "Detailhaendler")]
    [InlineData("20000000-0000-0000-0000-000000000000", "Zoo Zuerich", "Zuerichbergstrasse 221", "8044", "Zuerich", 487, 1125, "asdf", "00000000-0000-0000-0000-000000000002", "00000000-2000-0000-0000-000000000000", "Freizeit")]
    [InlineData("30000000-0000-0000-0000-000000000000", "Polenmuseum - Schloss Rapperswil", "Schloss", "8640", "Rapperswil-Jona", 11, 62, "jkl", "00000000-0000-0000-0000-000000000003", "00000000-3000-0000-0000-000000000000", "Bank")]
    public void TestMapper(string id, string name, string street, string zipCode, string city, int maxCapacity, int currentCapacity, string logo, string secret, string typeId, string typeDescription) {
      var store = new Store {
        Id = new(id),
        Name = name,
        Street = street,
        ZipCode = zipCode,
        City = city,
        MaxCapacity = maxCapacity,
        CurrentCapacity = currentCapacity,
        Logo = Encoding.UTF8.GetBytes(logo),
        Secret = new(secret),
        StoreType = new StoreType {
          Id = new(typeId),
          Description = typeDescription
        }
      };

      var newStoreResponse = _mapper.Map<Models.NewStoreResponseModel>(store);

      newStoreResponse.Id.Should().BeEquivalentTo(id);
      newStoreResponse.Secret.Should().BeEquivalentTo(secret);
    }
  }
}
