using MongoDB.Bson.Serialization;
using System.Threading;

namespace CapWatchBackend.DataAccess.MongoDB.Repositories {
  public class IntIdGenerator : IIdGenerator {

    private static readonly IntIdGenerator __instance = new IntIdGenerator();
    private static int __increment;
    static IntIdGenerator() { }

    public static IntIdGenerator Instance {
      get { return __instance; }
    }

    public object GenerateId(object container, object offset) {
      if ((int)offset > __increment) {
        __increment = (int)offset;
      }
      return Interlocked.Increment(ref __increment);
    }

    public bool IsEmpty(object id) {
      return id == null || (int)id == -1;
    }
  }
}
