﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CapWatchBackend.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class ShopController : ControllerBase {
    private readonly ILogger<ShopController> _logger;

    public ShopController(ILogger<ShopController> logger) {
      _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Shop> Get() {
      List<Shop> shops = new List<Shop>();

      shops.Add(new Shop { Name = "Migros St. Gallen", CurrentCapacity = 70, MaxCapacity = 180, Secret = ("Migros St. Gallen" + DateTime.Now.ToString()).GetHashCode() });
      shops.Add(new Shop { Name = "Säntispark Bäder", CurrentCapacity = 125, MaxCapacity = 150, Secret = ("Säntispark Bäder" + DateTime.Now.ToString()).GetHashCode() });
      shops.Add(new Shop { Name = "Interdiscount", CurrentCapacity = 7, MaxCapacity = 26, Secret = ("Interdiscount" + DateTime.Now.ToString()).GetHashCode() });

      return shops;
    }
  }
}
