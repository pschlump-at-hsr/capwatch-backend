using AutoMapper;
using CapWatchBackend.Application.Handlers;
using CapWatchBackend.Domain.Entities;
using CapWatchBackend.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CapWatchBackend.WebApi.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class StoresController : ControllerBase {
    private readonly IStoreHandler _handler;
    private readonly IMapper _mapper;

    public StoresController(IStoreHandler handler, IMapper mapper) {
      _handler = handler;
      _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetStores(string filter = null) {
      var stores = filter != null ? _handler.GetStores(filter) : _handler.GetStores();
      var result = stores.Select(store => _mapper.Map<StoreOverview>(store));
      return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetStores(Guid id) {
      var store = _handler.GetStore(id);
      var result = _mapper.Map<StoreOverview>(store);
      return Ok(result);
    }

    [HttpPatch]
    public IActionResult UpdateStores(StoreModel model) {
      var store = _mapper.Map<Store>(model);
      _handler.UpdateStoreAsync(store);
      return Ok();
    }

    [HttpPost]
    public IActionResult PostStores(StoreNew model) {
      var store = _mapper.Map<Store>(model);
      _handler.AddStoreAsync(store).Wait();
      var result = _mapper.Map<StoreNewResponse>(store);
      return Ok(result);
    }
  }
}
