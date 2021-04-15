using System;
using System.Runtime.Serialization;

namespace CapWatchBackend.Application.Exceptions {
  public class SecretInvalidException : ApplicationException {
    public SecretInvalidException() { }
    public SecretInvalidException(string message) : base(message) { }
    public SecretInvalidException(string message, Exception innerException) : base(message, innerException) { }
    protected SecretInvalidException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
}
