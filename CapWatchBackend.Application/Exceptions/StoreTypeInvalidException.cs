using System;
using System.Net;
using System.Runtime.Serialization;

namespace CapWatchBackend.Application.Exceptions {
  [Serializable]
  public class StoreTypeInvalidException : BaseException {
    public StoreTypeInvalidException() : base("Der Typ ist ungültig.") {
      Status = (int)HttpStatusCode.BadRequest;
    }

    protected StoreTypeInvalidException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
}
