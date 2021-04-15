using AutoMapper;
using CapWatchBackend.Application.Handlers;
using CapWatchBackend.Domain.Entities;
using CapWatchBackend.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

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

    // todo Christoph 2021.04.15: Korrekte Type implementation
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
      return Ok(result);
    }

    // todo Christoph 2021.04.15: Besseres Errorhandling
    [HttpPatch]
    public IActionResult UpdateStores(StoreModel model) {
      try {
        var store = _mapper.Map<Store>(model);
        _handler.UpdateStore(store);
        return Ok();
      } catch (Exception) {
        return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
      }
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
