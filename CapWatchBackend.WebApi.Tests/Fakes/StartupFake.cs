using CapWatchBackend.Application.Handlers;
using CapWatchBackend.WebApi.Tests.Mocks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CapWatchBackend.WebApi.Tests.Fakes {
  public class StartupFake : Startup {
    public StartupFake(IConfiguration configuration)
      : base(configuration) {
    }

    protected override void RegisterDependencies(IServiceCollection services) {
      services.AddSingleton<IStoreHandler, StoreHandlerMock>();
    }
  }
}
