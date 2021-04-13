using CapWatchBackend.Application.Repositories;
using CapWatchBackend.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CapWatchBackend.WebApi.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class StoresController : ControllerBase {
    private readonly ILogger<StoresController> _logger;
    private readonly IStoreRepository _repository;

    public StoresController(ILogger<StoresController> logger, IStoreRepository repository) {
      _logger = logger;
      _repository = repository;
    }

    [HttpGet]
    public IActionResult GetStores()
    {
      // var stores = _repository.GetStores().Select(store => new StoreModel(store));
      // return Ok(stores);
      return Ok(new StoreModel(new Domain.Entities.Store() { Id = 1, Name = "Ikea", Street = "Zürcherstrasse 460", ZipCode = "9015", City = "St. Gallen", CurrentCapacity = 135, MaxCapacity = 201 }));
    } 

    [HttpGet("{id}")]
    public IActionResult GetStores(int id) {
      // var stores = _repository.GetStores().Where(s => s.Id == id).Select(store => new StoreModel(store));
      // return Ok(stores);
      return Ok("Store 1");
    }

    [HttpPut("{id}")]
    public IActionResult PutStores(int id, StoreModel store) {

      return NoContent();
    }

    [HttpPost]
    public IActionResult PostStores(StoreModel store) {

      return Ok(store);
    }
  }
}
