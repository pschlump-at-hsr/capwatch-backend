using System;
using System.Net;
using System.Runtime.Serialization;

namespace CapWatchBackend.Application.Exceptions {
  [Serializable]
  public abstract class BaseException : ApplicationException {
    public int Status { get; set; } = (int)HttpStatusCode.InternalServerError;

    protected BaseException(string message) : base(message) { }
    protected BaseException(string message, Exception innerException) : base(message, innerException) { }

    protected BaseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
}
