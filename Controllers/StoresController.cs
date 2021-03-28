using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CapWatchBackend.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class StoresController : ControllerBase {
    private readonly ILogger<StoresController> _logger;

    public StoresController(ILogger<StoresController> logger) {
      _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Store> GetStores() {
      List<Store> shops = new List<Store>();

      shops.Add(new Store { Name = "Migros St. Gallen", CurrentCapacity = 70, MaxCapacity = 180 });
      shops.Add(new Store { Name = "Säntispark Bäder", CurrentCapacity = 125, MaxCapacity = 150 });
      shops.Add(new Store { Name = "Interdiscount", CurrentCapacity = 7, MaxCapacity = 26 });

      return shops;
    }
  }
}
