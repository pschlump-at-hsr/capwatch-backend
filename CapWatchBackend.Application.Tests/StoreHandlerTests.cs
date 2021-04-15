using CapWatchBackend.Application.Exceptions;
using CapWatchBackend.Application.Handlers;
using CapWatchBackend.Application.Repositories;
using CapWatchBackend.Domain.Entities;
using FakeItEasy;
using FluentAssertions;
using System;
using Xunit;

namespace CapWatchBackend.ApplicationTests {
  public class StoreHandlerTests {
    [Fact]
    public void TestAddStoreGeneratesGuid() {
      var store = new Store();

      var repository = A.Fake<IStoreRepository>();
      A.CallTo(() => repository.AddStore(store)).Returns(store);

      var storeHandler = new StoreHandler(repository);
      storeHandler.AddStore(store);

      store.Secret.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void TestUpdateStoreIncorrectSecret() {
      var store = new Store {
        Secret = Guid.Parse("00000000-0000-0000-0000-000000000001")
      };

      var repository = A.Fake<IStoreRepository>();
      A.CallTo(() => repository.GetStore(1)).Returns(store);

      var updatedStore = new Store {
        Id = 1,
        Secret = Guid.Parse("00000000-0000-0000-0000-000000000002")
      };

      var storeHandler = new StoreHandler(repository);
      storeHandler.Invoking(x => x.UpdateStore(updatedStore)).Should().Throw<SecretInvalidException>();
    }

    [Fact]
    public void TestUpdateStoreCorrectSecret() {
      var store = new Store {
        Secret = Guid.Parse("00000000-0000-0000-0000-000000000001")
      };

      var repository = A.Fake<IStoreRepository>();
      A.CallTo(() => repository.GetStore(1)).Returns(store);

      var updatedStore = new Store {
        Id = 1,
        Secret = Guid.Parse("00000000-0000-0000-0000-000000000001")
      };

      var storeHandler = new StoreHandler(repository);
      storeHandler.Invoking(x => x.UpdateStore(updatedStore)).Should().NotThrow();
    }
  }
}
