using System;
using System.Net;
using System.Runtime.Serialization;

namespace CapWatchBackend.Application.Exceptions {
  [Serializable]
  public class TypeInvalidException : BaseException {
    public TypeInvalidException() : base("Der Typ ist ungültig.") {
      Status = (int)HttpStatusCode.Unauthorized;
    }

    protected TypeInvalidException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
}
