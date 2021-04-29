using AutoMapper;
using CapWatchBackend.Domain.Entities;
using CapWatchBackend.WebApi.Mapper;
using CapWatchBackend.WebApi.Models;
using FluentAssertions;
using Xunit;

namespace CapWatchBackend.WebApi.Tests.ModelTests {
  public class StoreTypeModelTests {
    private readonly IMapper _mapper;

    public StoreTypeModelTests() {
      _mapper = new MapperConfiguration(config => config.AddProfile(new MapperProfile())).CreateMapper();
    }

    [Theory]
    [InlineData("00000000-1000-0000-0000-000000000000", "Detailhaendler")]
    public void TestMapperModelToEntity(string id, string description) {
      var storeType = new StoreType {
        Id = new(id),
        Description = description
      };

      var storeTypeModel = _mapper.Map<StoreTypeModel>(storeType);

      storeTypeModel.Id.Should().Be(id);
      storeTypeModel.Description.Should().Be(description);
    }

    [Theory]
    [InlineData("00000000-1000-0000-0000-000000000000", "Detailhaendler")]
    public void TestMapperEntityToModel(string id, string description) {
      var storeTypeModel = new StoreTypeModel {
        Id = id,
        Description = description
      };

      var storeType = _mapper.Map<StoreType>(storeTypeModel);

      storeType.Id.Should().Be(id);
      storeType.Description.Should().Be(description);
    }
  }
}
