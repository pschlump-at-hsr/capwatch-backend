using CapWatchBackend.Application.Repositories;
using CapWatchBackend.DataAccess.MongoDB.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CapWatchBackend.WebApi.Tests.Fakes {
  public class StartupFake : Startup {
    public StartupFake(IConfiguration configuration)
      : base(configuration) {
    }

    protected override void RegisterDependencies(IServiceCollection services) {
      //services.AddSingleton<IStoreRepository, StoreRepositoryMock>();
      services.AddSingleton<IStoreRepository, StoreRepository>();
    }
  }
}
