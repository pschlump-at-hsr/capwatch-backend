using CapWatchBackend.Application.Exceptions;
using CapWatchBackend.Application.Handlers;
using CapWatchBackend.Application.Repositories;
using CapWatchBackend.Domain.Entities;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CapWatchBackend.ApplicationTests {
  public class StoreHandlerTests {
    [Fact]
    public async Task TestAddStoreGeneratesGuidAsync() {
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

    [Theory]
    [InlineData("001", "00000000-0000-0000-0000-000000000001")]
    [InlineData("050", "00000000-0000-0000-0000-000000000005")]
    [InlineData("600", "00000000-0000-0000-0000-000000000006")]
    public async Task TestGetStoresFilterWorksForAllExpectedFields(string filter, string guidShouldBe) {
      var repository = A.Fake<IStoreRepository>();
      A.CallTo(() => repository.GetStores(A<Func<Store, bool>>._)).ReturnsLazily((Func<Store, bool> filterFunction) => GetStoreListForGetStoresTest().Where(filterFunction).ToList());

      var storeHandler = new StoreHandler(repository);
      var stores = await storeHandler.GetStores(filter);

      stores.FirstOrDefault().Id.Should().Be(Guid.Parse(guidShouldBe));
    }

    [Fact]
    public async Task TestGetStoresFilterAppliesToMoreThanOne() {
      var repository = A.Fake<IStoreRepository>();
      A.CallTo(() => repository.GetStores(A<Func<Store, bool>>._)).ReturnsLazily((Func<Store, bool> filterFunction) => GetStoreListForGetStoresTest().Where(filterFunction).ToList());

      var storeHandler = new StoreHandler(repository);
      var stores = await storeHandler.GetStores("00");

      stores.Count().Should().Be(6);
      stores.Any(store => store.Id == Guid.Parse("00000000-0000-0000-0000-000000000001")).Should().BeTrue();
      stores.Any(store => store.Id == Guid.Parse("00000000-0000-0000-0000-000000000002")).Should().BeFalse();
      stores.Any(store => store.Id == Guid.Parse("00000000-0000-0000-0000-000000000003")).Should().BeTrue();
      stores.Any(store => store.Id == Guid.Parse("00000000-0000-0000-0000-000000000004")).Should().BeTrue();
      stores.Any(store => store.Id == Guid.Parse("00000000-0000-0000-0000-000000000005")).Should().BeFalse();
      stores.Any(store => store.Id == Guid.Parse("00000000-0000-0000-0000-000000000006")).Should().BeTrue();
      stores.Any(store => store.Id == Guid.Parse("00000000-0000-0000-0000-000000000007")).Should().BeTrue();
      stores.Any(store => store.Id == Guid.Parse("00000000-0000-0000-0000-000000000008")).Should().BeFalse();
      stores.Any(store => store.Id == Guid.Parse("00000000-0000-0000-0000-000000000009")).Should().BeTrue();
    }

    private IList<Store> GetStoreListForGetStoresTest() {
      return new List<Store> {
        new Store{ Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), City = "001", Name = "", Street = "" },
        new Store{ Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), City = "020", Name = "", Street = "" },
        new Store{ Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), City = "300", Name = "", Street = "" },
        new Store{ Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), City = "", Name = "004", Street = "" },
        new Store{ Id = Guid.Parse("00000000-0000-0000-0000-000000000005"), City = "", Name = "050", Street = "" },
        new Store{ Id = Guid.Parse("00000000-0000-0000-0000-000000000006"), City = "", Name = "600", Street = "" },
        new Store{ Id = Guid.Parse("00000000-0000-0000-0000-000000000007"), City = "", Name = "", Street = "007" },
        new Store{ Id = Guid.Parse("00000000-0000-0000-0000-000000000008"), City = "", Name = "", Street = "080" },
        new Store{ Id = Guid.Parse("00000000-0000-0000-0000-000000000009"), City = "", Name = "", Street = "900" },
      };
    }
  }
}
