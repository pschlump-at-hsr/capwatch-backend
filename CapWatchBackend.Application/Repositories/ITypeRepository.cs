using CapWatchBackend.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CapWatchBackend.Application.Repositories {
  public interface ITypeRepository {

    public StoreType AddTypeAsync(string description);

    public List<StoreType> AddTypesAsync(List<string> descriptions);

    public Boolean IsValidType(StoreType type);
  }
}
