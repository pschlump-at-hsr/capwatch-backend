using CapWatchBackend.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CapWatchBackend.WebApi.ActionFilter {
  public class ExceptionFilter : IActionFilter, IOrderedFilter {
    private readonly ILogger<ExceptionFilter> _logger;

    public int Order { get; } = int.MaxValue;

    public ExceptionFilter(ILogger<ExceptionFilter> logger) {
      _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context) {
      _logger.LogTrace("API call received");
    }

    public void OnActionExecuted(ActionExecutedContext context) {
      if (context.Exception is BaseException exception) {
        context.Result = new ObjectResult(null) {
          StatusCode = exception.Status
        };
        context.ExceptionHandled = true;

        _logger.LogError(exception, exception.Message);
      }
    }
  }
}
