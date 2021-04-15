using System.Net;

namespace CapWatchBackend.Application.Exceptions {
  public class SecretInvalidException : ExceptionBase {
    public SecretInvalidException() : base("Das Secret ist ungültig.") {
      Status = (int)HttpStatusCode.Unauthorized;
    }
  }
}
