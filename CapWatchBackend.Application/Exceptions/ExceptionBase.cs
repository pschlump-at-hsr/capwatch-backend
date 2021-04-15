using System;
using System.Net;

namespace CapWatchBackend.Application.Exceptions {
  public class ExceptionBase : ApplicationException {
    public int Status { get; set; } = (int)HttpStatusCode.InternalServerError;

    public ExceptionBase(string message) : base(message) { }
    public ExceptionBase(string message, Exception innerException) : base(message, innerException) { }
  }
}
