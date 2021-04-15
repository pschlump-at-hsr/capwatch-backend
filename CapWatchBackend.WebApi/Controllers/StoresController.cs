using AutoMapper;
using CapWatchBackend.Application.Handlers;
using CapWatchBackend.Domain.Entities;
using CapWatchBackend.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CapWatchBackend.WebApi.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class StoresController : ControllerBase {
    private readonly IStoreHandler _handler;
    private readonly IMapper _mapper;

    public StoresController(ILogger<StoresController> logger, IStoreHandler handler, IMapper mapper) {
      _handler = handler;
      _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetStores() {
      var stores = _handler.GetStores();
      var result = stores.Select(store => new StoreModel(store));
      return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetStores(int id) {
      var store = _handler.GetStore(id);
      var result = new StoreModel(store);
      return Ok(result);
    }

    [HttpPatch]
    public IActionResult UpdateStores(StoreModel model) {
      var store = _mapper.Map<Store>(model);
      _handler.UpdateStore(store);
      return Ok();
    }

    [HttpPost]
    public IActionResult PostStores(StoreModel model) {
      var store = _mapper.Map<Store>(model);
      _handler.AddStore(store);
      return Ok(store.Secret);
    }
  }
}
