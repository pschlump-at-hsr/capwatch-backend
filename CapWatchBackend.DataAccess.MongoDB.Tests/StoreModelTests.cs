using CapWatchBackend.DataAccess.MongoDB.Model;
using Xunit;

namespace CapWatchBackend.DataAccess.MongoDB.Tests {

  public class StoreModelTests {

    StoreModel sm;
    public StoreModelTests() {
      sm = StoreModel.GetStoreModel();
    }

    [Fact]
    public void TestGetStores() {
      sm.GetStores();
    }
  }
}
