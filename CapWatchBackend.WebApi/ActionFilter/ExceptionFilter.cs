using CapWatchBackend.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

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
      if (context.Exception is BaseException baseException) {
        context.Result = new ObjectResult(null) {
          StatusCode = baseException.Status
        };
        context.ExceptionHandled = true;

        _logger.LogError(baseException, baseException.Message);
      } else if (context.Exception is Exception exception) {
        context.Result = new ObjectResult(null) {
          StatusCode = (int)HttpStatusCode.InternalServerError
        };
        context.ExceptionHandled = true;

        _logger.LogError(exception, exception.Message);
      } else {
        _logger.LogTrace("API call handled");
      }
    }
  }
}
