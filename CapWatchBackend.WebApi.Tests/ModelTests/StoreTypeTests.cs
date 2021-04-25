using AutoMapper;
using CapWatchBackend.Domain.Entities;
using CapWatchBackend.WebApi.Mapper;
using CapWatchBackend.WebApi.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CapWatchBackend.WebApi.Tests.ModelTests {
  public class StoreTypeTests {
    private readonly IMapper _mapper;

    public StoreTypeTests() {
      _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile())).CreateMapper();
    }

    [Theory]
    [InlineData("c73e9c5f-de5c-479a-b116-7ee1b93ab4f9", "Detailhändler")]
    public void TestMapperModelToEntity(string id, string description) {
      var storeType = new StoreType {
        Id = Guid.Parse(id),
        Description = description
      };

      var storeTypeModel = _mapper.Map<StoreTypeModel>(storeType);

      storeTypeModel.Id.Should().Be(id);
      storeTypeModel.Description.Should().Be(description);
    }

    [Theory]
    [InlineData("c73e9c5f-de5c-479a-b116-7ee1b93ab4f9", "Detailhändler")]
    public void TestMapperEntityToModel(string id, string description) {
      var storeTypeModel = new StoreTypeModel{
        Id = id,
        Description = description
      };

      var storeType = _mapper.Map<StoreType>(storeTypeModel);
      storeType.Id.Should().Be(id);
      storeType.Description.Should().Be(description);
    }
  }
}
