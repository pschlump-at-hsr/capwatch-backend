using CapWatchBackend.Application.Exceptions;
using System;

namespace CapWatchBackend.Application.Repositories.Exceptions {
  [Serializable]
  public class RepositoryException : ExceptionBase {
    public RepositoryException(string message, Exception innerException)
      : base(message, innerException) { }
  }
}
