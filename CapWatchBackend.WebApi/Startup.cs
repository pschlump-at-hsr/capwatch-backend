using CapWatchBackend.Application.Handlers;
using CapWatchBackend.Application.Repositories;
using CapWatchBackend.DataAccess.MongoDB;
using CapWatchBackend.DataAccess.MongoDB.Repositories;
using CapWatchBackend.WebApi.ActionFilter;
using CapWatchBackend.WebApi.Hubs;
using CapWatchBackend.WebApi.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CapWatchBackend.WebApi {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    private readonly string _corsOrigins = "allowDev";

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.AddCors(options => {
        options.AddPolicy(_corsOrigins, builder => {
          builder.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod();
        });
      });

      RegisterDependencies(services);

      services.AddSignalR();
      services.AddControllers(options => options.Filters.Add(typeof(ExceptionFilter)));

      services.AddAutoMapper(typeof(MapperProfile));

      services.AddSwaggerGen(options => {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "CapWatchBackend.WebApi", Version = "v1" });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      app.UseCors(_corsOrigins);

      if (env.IsDevelopment()) {
        app.UseSwagger();
        app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "CapWatchBackend.WebApi v1"));
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
        endpoints.MapHub<StoresHub>("/storeshub");
      });
    }

    protected virtual void RegisterDependencies(IServiceCollection services) {
      services.AddSingleton<IStoreRepository, StoreRepository>();
      services.AddSingleton<IStoreHandler, StoreHandler>();
      services.Configure<DatabaseConfiguration>(Configuration.GetSection(nameof(DatabaseConfiguration)));
    }
  }
}
