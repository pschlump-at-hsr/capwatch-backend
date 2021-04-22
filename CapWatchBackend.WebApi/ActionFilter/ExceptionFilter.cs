using CapWatchBackend.Application.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CapWatchBackend.WebApi.ActionFilter {
  public class ExceptionFilter : IActionFilter, IOrderedFilter {
    private readonly IWebHostEnvironment _hostEnvironment;

    public ExceptionFilter(IWebHostEnvironment hostEnvironment) {
      _hostEnvironment = hostEnvironment;
    }

    public int Order { get; } = int.MaxValue;

    public void OnActionExecuting(ActionExecutingContext context) {
      // Implemented because of interface, no logic needed before API execution
    }

    public void OnActionExecuted(ActionExecutedContext context) {
      if (context.Exception is BaseException exception) {
        dynamic result;
        if (_hostEnvironment.EnvironmentName.Equals("Development")) {
          result = new { exception.Message, exception.StackTrace };
        } else {
          result = new { exception.Message };
        }
        context.Result = new ObjectResult(result) {
          StatusCode = exception.Status
        };
        context.ExceptionHandled = true;
      }
    }
  }
}
