using CapWatchBackend.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CapWatchBackend.WebApi.ActionFilter {
  public class ExceptionFilter : IActionFilter, IOrderedFilter {
    public int Order { get; } = int.MaxValue;

    public void OnActionExecuting(ActionExecutingContext context) {
      // Implemented because of interface, no logic needed before API execution
    }

    public void OnActionExecuted(ActionExecutedContext context) {
      if (context.Exception is BaseException exception) {
        context.Result = new ObjectResult(null) {
          StatusCode = exception.Status
        };
        context.ExceptionHandled = true;
      }


    }
  }
}
