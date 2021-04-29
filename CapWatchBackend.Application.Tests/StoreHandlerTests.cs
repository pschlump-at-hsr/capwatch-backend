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
    private readonly Guid _correctSecret = new("00000000-0000-0000-0000-000000000001");
    private readonly Guid _incorrectSecret = new("00000000-0000-0000-0000-000000000002");
    private readonly Guid _storeId = new("10000000-0000-0000-0000-000000000000");

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
        Secret = _correctSecret
      };

      var repository = A.Fake<IStoreRepository>();
      A.CallTo(() => repository.GetStoreAsync(_storeId)).Returns(store);

      var updatedStore = new Store {
        Id = _storeId,
        Secret = _incorrectSecret
      };

      var storeHandler = new StoreHandler(repository);
      storeHandler.Invoking(async handler => await handler.UpdateStoreAsync(updatedStore)).Should().Throw<SecretInvalidException>();
    }

    [Fact]
    public void TestUpdateStoreCorrectSecret() {
      var store = new Store {
        Secret = _correctSecret
      };

      var repository = A.Fake<IStoreRepository>();
      A.CallTo(() => repository.GetStoreAsync(_storeId)).Returns(store);

      var updatedStore = new Store {
        Id = _storeId,
        Secret = _correctSecret
      };

      var storeHandler = new StoreHandler(repository);
      storeHandler.Invoking(async handler => await handler.UpdateStoreAsync(updatedStore)).Should().NotThrow();
    }

    [Theory]
    [InlineData("001", "00000000-0000-0000-0000-000000000001")]
    [InlineData("050", "00000000-0000-0000-0000-000000000005")]
    [InlineData("600", "00000000-0000-0000-0000-000000000006")]
    public async Task TestGetStoresFilterWorksForAllExpectedFieldsAsync(string filter, string guidShouldBe) {
      var repository = A.Fake<IStoreRepository>();
      A.CallTo(() => repository.GetStoresAsync(A<Func<Store, bool>>._)).ReturnsLazily((Func<Store, bool> filterFunction) => GetStoreListForGetStoresTest().Where(filterFunction).ToList());

      var storeHandler = new StoreHandler(repository);
      var stores = await storeHandler.GetStoresAsync(filter);

      stores.FirstOrDefault().Id.Should().Be(Guid.Parse(guidShouldBe));
    }

    [Fact]
    public async Task TestGetStoresFilterAppliesToMoreThanOneAsync() {
      var repository = A.Fake<IStoreRepository>();
      A.CallTo(() => repository.GetStoresAsync(A<Func<Store, bool>>._)).ReturnsLazily((Func<Store, bool> filterFunction) => GetStoreListForGetStoresTest().Where(filterFunction).ToList());

      var storeHandler = new StoreHandler(repository);
      var stores = await storeHandler.GetStoresAsync("00");

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
      stores.Any(store => store.Id == Guid.Parse("00000000-0000-0000-0000-000000000010")).Should().BeFalse();
    }

    private static IList<Store> GetStoreListForGetStoresTest() {
      return new List<Store> {
        new Store{ Id = new("00000000-0000-0000-0000-000000000001"), City = "001", Name = string.Empty, Street = string.Empty },
        new Store{ Id = new("00000000-0000-0000-0000-000000000002"), City = "020", Name = string.Empty, Street = string.Empty },
        new Store{ Id = new("00000000-0000-0000-0000-000000000003"), City = "300", Name = string.Empty, Street = string.Empty },
        new Store{ Id = new("00000000-0000-0000-0000-000000000004"), City = string.Empty, Name = "004", Street = string.Empty },
        new Store{ Id = new("00000000-0000-0000-0000-000000000005"), City = string.Empty, Name = "050", Street = string.Empty },
        new Store{ Id = new("00000000-0000-0000-0000-000000000006"), City = string.Empty, Name = "600", Street = string.Empty },
        new Store{ Id = new("00000000-0000-0000-0000-000000000007"), City = string.Empty, Name = string.Empty, Street = "007" },
        new Store{ Id = new("00000000-0000-0000-0000-000000000008"), City = string.Empty, Name = string.Empty, Street = "080" },
        new Store{ Id = new("00000000-0000-0000-0000-000000000009"), City = string.Empty, Name = string.Empty, Street = "900" },
        new Store{ Id = new("00000000-0000-0000-0000-000000000010"), City = string.Empty, Name = string.Empty, Street = string.Empty }
      };
    }
  }
}
