using CapWatchBackend.Application.Repositories;
using CapWatchBackend.DataAccess.MongoDB.Repositories;
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

    public string CorsOrigins = "allowDev";

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.AddCors(options => {
        options.AddPolicy(CorsOrigins, builder => {
          builder.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod();
        });
      });

      RegisterDependencies(services);

      services.AddControllers();

      services.AddSwaggerGen(c => {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "CapWatchBackend.WebApi", Version = "v1" });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

      app.UseCors(CorsOrigins);

      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CapWatchBackend.WebApi v1"));
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
    }

    protected virtual void RegisterDependencies(IServiceCollection services) {
      services.AddSingleton<IStoreRepository, StoreRepository>();
    }
  }
}
