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
  internal sealed class StoreHandler : IStoreHandler {
    private readonly IStoreRepository _repository;

    public StoreHandler(IStoreRepository repository) {
      _repository = repository;
    }

    public Task AddStoreAsync(Store store) {
      store.Secret = Guid.NewGuid();
      return _repository.AddStoreAsync(store);
    }

    public Task UpdateStoreAsync(Store store) {
      if (!_repository.GetStoreAsync(store.Id).GetAwaiter().GetResult().Secret.Equals(store.Secret)) {
        throw new SecretInvalidException();
      }

      return _repository.UpdateStoreAsync(store);
    }

    public Task<IEnumerable<Store>> GetStoresAsync() {
      return _repository.GetStoresAsync();
    }

    public Task<IEnumerable<Store>> GetStoresAsync(string filter) {
      bool filterFunction(Store store) => store.Name.Contains(filter, StringComparison.CurrentCultureIgnoreCase)
        || store.Street.Contains(filter, StringComparison.CurrentCultureIgnoreCase)
        || store.City.Contains(filter, StringComparison.CurrentCultureIgnoreCase);

      return _repository.GetStoresAsync(filterFunction);
    }

    public Task<Store> GetStoreAsync(Guid id) {
      return _repository.GetStoreAsync(id);
    }
  }
}
