using CapWatchBackend.Application.Exceptions;
using CapWatchBackend.Application.Repositories;
using CapWatchBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("CapWatchBackend.WebApi")]
[assembly: InternalsVisibleTo("CapWatchBackend.Application.Tests")]
namespace CapWatchBackend.Application.Handlers {
  internal class StoreHandler : IStoreHandler {
    private readonly IStoreRepository _repository;

    public StoreHandler(IStoreRepository repository) {
      _repository = repository;
    }

    public Task AddStoreAsync(Store store) {
      store.Secret = Guid.NewGuid();
      return _repository.AddStoreAsync(store);
    }

    public Task UpdateStoreAsync(Store store) {
      if (!_repository.GetStore(store.Id).Result.Secret.Equals(store.Secret)) {
        throw new SecretInvalidException();
      }

      return _repository.UpdateStoreAsync(store);
    }

    public Task<IEnumerable<Store>> GetStores() {
      return _repository.GetStores();
    }

    public Task<IEnumerable<Store>> GetStores(string filter) {
      bool filterFunction(Store store) => store.Name.Contains(filter, StringComparison.CurrentCultureIgnoreCase)
        || store.Street.Contains(filter, StringComparison.CurrentCultureIgnoreCase)
        || store.City.Contains(filter, StringComparison.CurrentCultureIgnoreCase);

      return _repository.GetStores(filterFunction);
    }

    public Task<Store> GetStore(Guid id) {
      return _repository.GetStore(id);
    }
  }
}
