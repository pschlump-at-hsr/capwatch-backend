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
    public IActionResult GetStores() {
      return Ok(_repository.GetStores().Select(x => new StoreModel(x)));
    }

  }
}
