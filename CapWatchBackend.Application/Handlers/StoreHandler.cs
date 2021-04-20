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

    public async Task AddStoreAsync(Store store) {
      store.Secret = Guid.NewGuid();
      await _repository.AddStoreAsync(store);
    }

    public async Task UpdateStoreAsync(Store store) {
      if (!_repository.GetStore(store.Id).Secret.Equals(store.Secret)) {
        throw new SecretInvalidException();
      }

      await _repository.UpdateStoreAsync(store);
    }

    public IEnumerable<Store> GetStores() {
      return _repository.GetStores();
    }

    public Store GetStore(Guid id) {
      return _repository.GetStore(id);
    }
  }
}
