using CapWatchBackend.Application.Exceptions;
using CapWatchBackend.Application.Repositories;
using CapWatchBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CapWatchBackend.WebApi")]
namespace CapWatchBackend.Application.Handlers {
  internal class StoreHandler : IStoreHandler {
    private readonly IStoreRepository _repository;

    public StoreHandler(IStoreRepository repository) {
      _repository = repository;
    }

    public void AddStore(Store store) {
      store.Secret = Guid.NewGuid();
      _repository.AddStore(store);
    }

    public void UpdateStore(Store store) {
      if (!_repository.GetStore(store.Id).Secret.Equals(store.Secret)) {
        throw new SecretInvalidException();
      }

      _repository.UpdateStore(store);
    }

    public IEnumerable<Store> GetStores() {
      return _repository.GetStores();
    }

    public Store GetStore(int id) {
      return _repository.GetStore(id);
    }
  }
}
