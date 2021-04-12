using CapWatchBackend.Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CapWatchBackend.WebApi.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class StoresController : ControllerBase {
    // todo 2021.04.08 Christoph: reactivate Repository
    private readonly ILogger<StoresController> _logger;
    private readonly IStoreRepository _repository;

    public StoresController(ILogger<StoresController> logger, IStoreRepository repository) {
      _logger = logger;
      _repository = repository;
    }

    [HttpGet]
    public IActionResult GetStores() {
      var stores = _repository.GetStores();
      //List<StoreModel> stores = new List<StoreModel>();
      //stores.Add(new StoreModel(new Domain.Entities.Store { Id = 1, Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", CurrentCapacity = 135, MaxCapacity = 201 }));
      //stores.Add(new StoreModel(new Domain.Entities.Store { Id = 2, Name = "Zoo Zürich", Street = "Zürichbergstrasse 221", ZipCode = "8044", City = "Zürich", CurrentCapacity = 487, MaxCapacity = 1125 }));
      //stores.Add(new StoreModel(new Domain.Entities.Store { Id = 3, Name = "Polenmuseum - Schloss Rapperswil", Street = "Schloss", ZipCode = "8640", City = "Raperswil-Jona", CurrentCapacity = 11, MaxCapacity = 62 }));
      //stores.Add(new StoreModel(new Domain.Entities.Store { Id = 4, Name = "Alpamare", Street = "Gwattstrasse 12", ZipCode = "8808", City = "Pfäffikon", CurrentCapacity = 152, MaxCapacity = 612 }));
      //stores.Add(new StoreModel(new Domain.Entities.Store { Id = 5, Name = "Botanischer Garten der Universität Bern", Street = "Altenbergrain 21", ZipCode = "3013", City = "Bern", CurrentCapacity = 103, MaxCapacity = 103 }));
      return Ok(stores);
    }

  }
}
