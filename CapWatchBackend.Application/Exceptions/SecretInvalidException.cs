using System;
using System.Net;
using System.Runtime.Serialization;

namespace CapWatchBackend.Application.Exceptions {
  [Serializable]
  public class SecretInvalidException : BaseException {
    public SecretInvalidException() : base("Das Secret ist ungültig.") {
      Status = (int)HttpStatusCode.Unauthorized;
    }

    protected SecretInvalidException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
}
