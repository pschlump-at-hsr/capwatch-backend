using CapWatchBackend.Application.Exceptions;
using CapWatchBackend.DataAccess.MongoDB.DbContext;
using CapWatchBackend.DataAccess.MongoDB.Repositories;
using CapWatchBackend.Domain.Entities;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace CapWatchBackend.DataAccess.MongoDB.Tests {

  public class StoreRepositoryTests {
    private readonly ILogger<StoreRepository> _logger;

    public StoreRepositoryTests() {
      _logger = A.Fake<ILogger<StoreRepository>>();
    }

    [Fact]
    public async Task TestAddStoreAsync() {
      var store = GetTestStore();

      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection(new List<Store> { store });
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);
      A.CallTo(() => context.GetTypeCollection()).Returns(GetFakeStoreTypeCollection());

      var storeRepository = new StoreRepository(context, _logger);

      await storeRepository.AddStoreAsync(store);

      store.Id.Should().NotBe(Guid.Empty).And.Should().NotBeNull();
      A.CallTo(() => storeCollection.InsertOneAsync(A<Store>._, A<InsertOneOptions>._, default)).MustHaveHappened();
    }

    [Fact]
    public void TestAddStoreAsyncInvalidStoreType() {
      var store = GetTestStore();

      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection();
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);
      A.CallTo(() => context.GetTypeCollection()).Returns(GetFakeStoreTypeCollection(0));

      var storeRepository = new StoreRepository(context, _logger);

      storeRepository.Invoking(async repository => await repository.AddStoreAsync(store)).Should().Throw<StoreTypeInvalidException>();
      A.CallTo(() => storeCollection.InsertOneAsync(A<Store>._, A<InsertOneOptions>._, default)).MustNotHaveHappened();
    }

    [Fact]
    public async Task TestAddStoresAsync() {
      var stores = GetTestStores();

      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection();
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);
      A.CallTo(() => context.GetTypeCollection()).Returns(GetFakeStoreTypeCollection());

      var storeRepository = new StoreRepository(context, _logger);

      await storeRepository.AddStoresAsync(stores);

      stores.Any(store => store.Id == Guid.Empty).Should().BeFalse();
      A.CallTo(() => storeCollection.InsertManyAsync(A<IList<Store>>._, A<InsertManyOptions>._, default)).MustHaveHappened();
    }

    [Fact]
    public void TestAddStoresAsyncInvalidStoreType() {
      var stores = GetTestStores();

      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection();
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);
      A.CallTo(() => context.GetTypeCollection()).Returns(GetFakeStoreTypeCollection(0));

      var storeRepository = new StoreRepository(context, _logger);

      storeRepository.Invoking(async repository => await repository.AddStoresAsync(stores)).Should().Throw<StoreTypeInvalidException>();
      A.CallTo(() => storeCollection.InsertManyAsync(A<IList<Store>>._, A<InsertManyOptions>._, default)).MustNotHaveHappened();
    }

    [Fact]
    public async Task TestUpdateStoreAsync() {
      var store = GetTestStore();

      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection();
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);
      A.CallTo(() => context.GetTypeCollection()).Returns(GetFakeStoreTypeCollection());

      var storeRepository = new StoreRepository(context, _logger);

      await storeRepository.UpdateStoreAsync(store);
      A.CallTo(() => storeCollection.ReplaceOneAsync(A<FilterDefinition<Store>>._, A<Store>._, A<ReplaceOptions>._, default)).MustHaveHappened();
    }

    [Fact]
    public void TestUpdateStoreAsyncInvalidStoreType() {
      var store = GetTestStore();

      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection();
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);
      A.CallTo(() => context.GetTypeCollection()).Returns(GetFakeStoreTypeCollection(0));

      var storeRepository = new StoreRepository(context, _logger);

      storeRepository.Invoking(async repository => await repository.UpdateStoreAsync(store)).Should().Throw<StoreTypeInvalidException>();
      A.CallTo(() => storeCollection.ReplaceOneAsync(A<FilterDefinition<Store>>._, A<Store>._, A<ReplaceOptions>._, default)).MustNotHaveHappened();
    }

    [Fact]
    public async Task TestUpdateStoresAsync() {
      var stores = GetTestStores();

      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection();
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);
      A.CallTo(() => context.GetTypeCollection()).Returns(GetFakeStoreTypeCollection());

      var storeRepository = new StoreRepository(context, _logger);

      await storeRepository.UpdateStoresAsync(stores);
      A.CallTo(() => storeCollection.ReplaceOneAsync(A<FilterDefinition<Store>>._, A<Store>._, A<ReplaceOptions>._, default)).MustHaveHappened(3, Times.Exactly);
    }

    [Fact]
    public void TestUpdateStoresAsyncInvalidStoreType() {
      var stores = GetTestStores();

      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection();
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);
      A.CallTo(() => context.GetTypeCollection()).Returns(GetFakeStoreTypeCollection(0));

      var storeRepository = new StoreRepository(context, _logger);

      storeRepository.Invoking(async repository => await repository.UpdateStoresAsync(stores)).Should().Throw<StoreTypeInvalidException>();
      A.CallTo(() => storeCollection.ReplaceOneAsync(A<FilterDefinition<Store>>._, A<Store>._, A<ReplaceOptions>._, default)).MustNotHaveHappened();
    }

    [Fact]
    public async Task TestGetStoresAsync() {
      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection(GetTestStores());
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);
      A.CallTo(() => context.GetTypeCollection()).Returns(GetFakeStoreTypeCollection());

      var storeRepository = new StoreRepository(context, _logger);

      var stores = await storeRepository.GetStoresAsync();

      stores.Count().Should().Be(3);
      A.CallTo(() => storeCollection.FindSync(FilterDefinition<Store>.Empty, A<FindOptions<Store>>._, default)).MustHaveHappened();
    }

    [Fact]
    public async Task TestGetStoresAsyncFiltered() {
      Expression<Func<Store, bool>> filterFunction = store => store.Name.Equals("Ikea");

      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection(GetTestStores(), filterFunction);
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);

      var storeRepository = new StoreRepository(context, _logger);

      var stores = await storeRepository.GetStoresAsync(filterFunction);

      stores.Count().Should().Be(1);
      A.CallTo(() => storeCollection.FindSync(A<FilterDefinition<Store>>._, A<FindOptions<Store>>._, default)).MustHaveHappened();
    }

    [Fact]
    public async Task TestGetStoresAsyncFilteredNoResult() {
      Expression<Func<Store, bool>> filterFunction = store => store.Name.Equals("Invalid");

      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection(GetTestStores(), filterFunction);
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);

      var storeRepository = new StoreRepository(context, _logger);

      var stores = await storeRepository.GetStoresAsync(filterFunction);

      stores.Count().Should().Be(0);
      A.CallTo(() => storeCollection.FindSync(A<FilterDefinition<Store>>._, A<FindOptions<Store>>._, default)).MustHaveHappened();
    }

    [Fact]
    public async Task TestGetStoreAsync() {
      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection(new List<Store> { GetTestStore() });
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);

      var storeRepository = new StoreRepository(context, _logger);

      var store = await storeRepository.GetStoreAsync(GetTestStore().Id);

      store.Name.Should().Be("Botanischer Garten der Universitaet Bern");
      A.CallTo(() => storeCollection.FindSync(A<FilterDefinition<Store>>._, A<FindOptions<Store>>._, default)).MustHaveHappened();
    }

    [Fact]
    public async Task TestDeleteAllStoresAsync() {
      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection();
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);

      var storeRepository = new StoreRepository(context, _logger);

      await storeRepository.DeleteAllStoresAsync();

      A.CallTo(() => storeCollection.DeleteManyAsync(FilterDefinition<Store>.Empty, default)).MustHaveHappened();
    }

    [Fact]
    public async Task TestDeleteStoreAsync() {
      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection();
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);

      var storeRepository = new StoreRepository(context, _logger);

      await storeRepository.DeleteStoreAsync(GetTestStore());

      A.CallTo(() => storeCollection.DeleteManyAsync(A<FilterDefinition<Store>>._, default)).MustHaveHappened();
    }

    [Fact]
    public void TestAddStoreAsyncWrapsMongoException() {
      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection(GetTestStores());
      A.CallTo(() => storeCollection.InsertOneAsync(A<Store>._, A<InsertOneOptions>._, default)).Throws(() => throw new MongoClientException("test exception"));
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);
      A.CallTo(() => context.GetTypeCollection()).Returns(GetFakeStoreTypeCollection());

      var storeRepository = new StoreRepository(context, _logger);
      storeRepository.Invoking(async repository => await repository.AddStoreAsync(GetTestStore())).Should().Throw<RepositoryException>();
    }

    [Fact]
    public void TestAddStoresAsyncWrapsMongoException() {
      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection(GetTestStores());
      A.CallTo(() => storeCollection.InsertManyAsync(A<IEnumerable<Store>>._, A<InsertManyOptions>._, default)).Throws(() => throw new MongoClientException("test exception"));
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);
      A.CallTo(() => context.GetTypeCollection()).Returns(GetFakeStoreTypeCollection());

      var storeRepository = new StoreRepository(context, _logger);
      storeRepository.Invoking(async repository => await repository.AddStoresAsync(GetTestStores())).Should().Throw<RepositoryException>();
    }

    [Fact]
    public void TestUpdateStoreAsyncWrapsMongoException() {
      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection(GetTestStores());
      A.CallTo(() => storeCollection.ReplaceOneAsync(A<FilterDefinition<Store>>._, A<Store>._, A<ReplaceOptions>._, default)).Throws(() => throw new MongoClientException("test exception"));
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);
      A.CallTo(() => context.GetTypeCollection()).Returns(GetFakeStoreTypeCollection());

      var storeRepository = new StoreRepository(context, _logger);
      storeRepository.Invoking(async repository => await repository.UpdateStoreAsync(GetTestStore())).Should().Throw<RepositoryException>();
    }

    [Fact]
    public void TestUpdateStoresAsyncWrapsMongoException() {
      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection(GetTestStores());
      A.CallTo(() => storeCollection.ReplaceOneAsync(A<FilterDefinition<Store>>._, A<Store>._, A<ReplaceOptions>._, default)).Throws(() => throw new MongoClientException("test exception"));
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);
      A.CallTo(() => context.GetTypeCollection()).Returns(GetFakeStoreTypeCollection());

      var storeRepository = new StoreRepository(context, _logger);
      storeRepository.Invoking(async repository => await repository.UpdateStoresAsync(GetTestStores())).Should().Throw<RepositoryException>();
    }

    [Fact]
    public void TestGetStoresAsyncWrapsMongoException() {
      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection(GetTestStores());
      A.CallTo(() => storeCollection.FindSync(A<FilterDefinition<Store>>._, A<FindOptions<Store>>._, default)).Throws(() => throw new MongoClientException("test exception"));
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);
      A.CallTo(() => context.GetTypeCollection()).Returns(GetFakeStoreTypeCollection());

      var storeRepository = new StoreRepository(context, _logger);
      storeRepository.Invoking(async repository => await repository.GetStoresAsync()).Should().Throw<RepositoryException>();
    }

    [Fact]
    public void TestGetStoresAsyncFilteredWrapsMongoException() {
      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection(GetTestStores());
      A.CallTo(() => storeCollection.FindSync(A<FilterDefinition<Store>>._, A<FindOptions<Store>>._, default)).Throws(() => throw new MongoClientException("test exception"));
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);
      A.CallTo(() => context.GetTypeCollection()).Returns(GetFakeStoreTypeCollection());

      var storeRepository = new StoreRepository(context, _logger);
      storeRepository.Invoking(async repository => await repository.GetStoresAsync((store) => false)).Should().Throw<RepositoryException>();
    }

    [Fact]
    public void TestGetStoreAsyncWrapsMongoException() {
      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection(GetTestStores());
      A.CallTo(() => storeCollection.FindSync(A<FilterDefinition<Store>>._, A<FindOptions<Store>>._, default)).Throws(() => throw new MongoClientException("test exception"));
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);
      A.CallTo(() => context.GetTypeCollection()).Returns(GetFakeStoreTypeCollection());

      var storeRepository = new StoreRepository(context, _logger);
      storeRepository.Invoking(async repository => await repository.GetStoreAsync(Guid.Empty)).Should().Throw<RepositoryException>();
    }

    [Fact]
    public void TestDeleteAllStoresAsyncWrapsMongoException() {
      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection(GetTestStores());
      A.CallTo(() => storeCollection.DeleteManyAsync(A<FilterDefinition<Store>>._, default)).Throws(() => throw new MongoClientException("test exception"));
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);
      A.CallTo(() => context.GetTypeCollection()).Returns(GetFakeStoreTypeCollection());

      var storeRepository = new StoreRepository(context, _logger);
      storeRepository.Invoking(async repository => await repository.DeleteAllStoresAsync()).Should().Throw<RepositoryException>();
    }

    [Fact]
    public void TestDeleteStoreAsyncWrapsMongoException() {
      var context = A.Fake<ICapwatchDBContext>();
      var storeCollection = GetFakeStoreCollection(GetTestStores());
      A.CallTo(() => storeCollection.DeleteManyAsync(A<FilterDefinition<Store>>._, default)).Throws(() => throw new MongoClientException("test exception"));
      A.CallTo(() => context.GetStoreCollection()).Returns(storeCollection);
      A.CallTo(() => context.GetTypeCollection()).Returns(GetFakeStoreTypeCollection());

      var storeRepository = new StoreRepository(context, _logger);
      storeRepository.Invoking(async repository => await repository.DeleteStoreAsync(GetTestStore())).Should().Throw<RepositoryException>();
    }

    private static IMongoCollection<StoreType> GetFakeStoreTypeCollection(long returnValue = 1) {
      var collection = A.Fake<IMongoCollection<StoreType>>();
      A.CallTo(() => collection.CountDocuments(A<FilterDefinition<StoreType>>._, A<CountOptions>._, default)).ReturnsNextFromSequence(returnValue, 1, 1);
      return collection;
    }

    private static IMongoCollection<Store> GetFakeStoreCollection(IList<Store> findReturnValue = null, Expression<Func<Store, bool>> filter = null) {
      var collection = A.Fake<IMongoCollection<Store>>();
      var asyncCursor = A.Fake<IAsyncCursor<Store>>();

      A.CallTo(() => collection.FindSync(A<FilterDefinition<Store>>._, A<FindOptions<Store>>._, default)).Returns(asyncCursor);

      A.CallTo(() => asyncCursor.MoveNext(default)).ReturnsNextFromSequence(true, false);
      A.CallTo(() => asyncCursor.Current).Returns(filter == null ? findReturnValue : findReturnValue.Where(filter.Compile()));

      return collection;
    }

    private static Store GetTestStore() {
      return new Store {
        Name = "Botanischer Garten der Universitaet Bern",
        Secret = new("00000000-0000-0000-0000-000000000001"),
        Street = "Altenbergrain 21",
        ZipCode = "3013",
        City = "Bern",
        CurrentCapacity = 103,
        MaxCapacity = 103,
        StoreType = new StoreType {
          Id = new("c73e9c5f-de5c-479a-b116-7ee1b93ab4f9"),
          Description = "Detailhändler"
        }
      };
    }

    private static IList<Store> GetTestStores() {
      return new List<Store> {
        new Store {
          Name = "Ikea",
          Street = "Zuercherstrasse 460",
          ZipCode = "9015",
          City = "St. Gallen",
          CurrentCapacity = 135,
          MaxCapacity = 201,
          Secret = new("00000000-0000-0000-0000-000000000001"),
          StoreType = new StoreType {
            Id = new("c73e9c5f-de5c-479a-b116-7ee1b93ab4f9"),
            Description = "Detailhändler"
          }
        },
        new Store {
          Name = "Zoo Zuerich",
          Street = "Zuerichbergstrasse 221",
          ZipCode = "8044",
          City = "Zuerich",
          CurrentCapacity = 487,
          MaxCapacity = 1125,
          Secret = new("00000000-0000-0000-0000-000000000002"),
          StoreType = new StoreType {
            Id = new("7b0523b7-4efd-4fdf-b11d-3f4d26cf7b19"),
            Description = "Freizeit"
          }},
        new Store {
          Name = "Polenmuseum - Schloss Rapperswil",
          Street = "Schloss",
          ZipCode = "8640",
          City = "Rapperswil-Jona",
          CurrentCapacity = 11,
          MaxCapacity = 62,
          Secret = new("00000000-0000-0000-0000-000000000003"),
          StoreType = new StoreType {
            Id = new("f58957ce-fb83-4f62-ac2c-6d1fe810d85c"),
            Description = "Bank"
          }
        }
      };
    }
  }
}
