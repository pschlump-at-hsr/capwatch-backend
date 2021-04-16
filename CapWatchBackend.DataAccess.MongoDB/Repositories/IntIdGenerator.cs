using MongoDB.Bson.Serialization;
using System.Threading;

namespace CapWatchBackend.DataAccess.MongoDB.Repositories {
  public class IntIdGenerator : IIdGenerator {

    private static readonly IntIdGenerator _instance = new IntIdGenerator();
    private static int _increment;
    static IntIdGenerator() { }

    public static IntIdGenerator Instance {
      get { return _instance; }
    }

    public object GenerateId(object container, object offset) {
      if ((int)offset > _increment) {
        _increment = (int)offset;
      }
      return Interlocked.Increment(ref _increment);
    }

    public bool IsEmpty(object id) {
      return id == null || (int)id == -1;
    }
  }
}
