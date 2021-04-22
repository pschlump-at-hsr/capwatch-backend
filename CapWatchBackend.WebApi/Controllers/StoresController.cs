using AutoMapper;
using CapWatchBackend.Application.Handlers;
using CapWatchBackend.Domain.Entities;
using CapWatchBackend.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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

    // todo Christoph 2021.04.15: Implement Type in Backend (Pseudodata for Frontend)
    [HttpGet]
    public IActionResult GetStores() {
      var stores = _handler.GetStores();
      var result = stores.Select(store => _mapper.Map<StoreOverview>(store));
      var type = new StoreType() { Description = "Detailhandel" };
      var tmpRes = new List<StoreOverview>();
      foreach (var store in result) {
        store.Type = type;
        tmpRes.Add(store);
      }
      return Ok(tmpRes);
    }

    [HttpGet("{id}")]
    public IActionResult GetStores(int id) {
      var store = _handler.GetStore(id);
      var result = _mapper.Map<StoreOverview>(store);
      var type = new StoreType() { Description = "Detailhandel" };
      result.Type = type;
      return Ok(result);
    }

    [HttpPatch]
    public IActionResult UpdateStores(StoreModel model) {
      var store = _mapper.Map<Store>(model);
      _handler.UpdateStore(store);
      return Ok();
    }

    [HttpPost]
    public IActionResult PostStores(StoreNew model) {
      var store = _mapper.Map<Store>(model);
      _handler.AddStore(store);
      var result = _mapper.Map<StoreNewResponse>(store);
      return Ok(result);
    }
  }
}
