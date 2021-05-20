using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Diagnostics.CodeAnalysis;

namespace CapWatchBackend.WebApi {
  [ExcludeFromCodeCoverage]
  public static class Program {
    public static void Main(string[] args) {
      var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
      try {
        logger.Trace("CapWatch started");
        CreateHostBuilder(args).Build().Run();
      } catch (Exception exception) {
        logger.Error(exception, "CapWatch could not start because of exception");
        throw;
      } finally {
        NLog.LogManager.Shutdown();
      }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => {
              webBuilder.UseStartup<Startup>();
            }).ConfigureLogging(logging => {
              logging.SetMinimumLevel(LogLevel.Trace);
            }).UseNLog();
  }
}
