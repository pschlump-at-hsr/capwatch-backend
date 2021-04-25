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
    public async System.Threading.Tasks.Task TestAddStoreGeneratesGuidAsync() {
      var store = new Store();

      var repository = A.Fake<IStoreRepository>();
      A.CallTo(() => repository.AddStoreAsync(store));

      var storeHandler = new StoreHandler(repository);
      await storeHandler.AddStoreAsync(store);

      store.Secret.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void TestUpdateStoreIncorrectSecret() {
      var store = new Store {
        Secret = Guid.Parse("00000000-0000-0000-0000-000000000001")
      };

      var repository = A.Fake<IStoreRepository>();
      A.CallTo(() => repository.GetStore(Guid.Parse("9c9cee44-c839-48f2-b54e-235d95fe5d7f"))).Returns(store);

      var updatedStore = new Store {
        Id = Guid.Parse("9c9cee44-c839-48f2-b54e-235d95fe5d7f"),
        Secret = Guid.Parse("00000000-0000-0000-0000-000000000002")
      };

      var storeHandler = new StoreHandler(repository);
      storeHandler.Invoking(x => x.UpdateStoreAsync(updatedStore).Wait()).Should().Throw<SecretInvalidException>();
    }

    [Fact]
    public void TestUpdateStoreCorrectSecret() {
      var store = new Store {
        Secret = Guid.Parse("00000000-0000-0000-0000-000000000001")
      };

      var repository = A.Fake<IStoreRepository>();
      A.CallTo(() => repository.GetStore(Guid.Parse("9c9cee44-c839-48f2-b54e-235d95fe5d7f"))).Returns(store);

      var updatedStore = new Store {
        Id = Guid.Parse("9c9cee44-c839-48f2-b54e-235d95fe5d7f"),
        Secret = Guid.Parse("00000000-0000-0000-0000-000000000001")
      };

      var storeHandler = new StoreHandler(repository);
      storeHandler.Invoking(x => x.UpdateStoreAsync(updatedStore).Wait()).Should().NotThrow();
    }
  }
}
