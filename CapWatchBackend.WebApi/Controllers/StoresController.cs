using AutoMapper;
using CapWatchBackend.Application.Repositories;
using CapWatchBackend.Domain.Entities;
using CapWatchBackend.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace CapWatchBackend.WebApi.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class StoresController : ControllerBase {
    private readonly ILogger<StoresController> _logger;
    private readonly IStoreRepository _repository;
    private readonly IMapper _mapper;

    public StoresController(ILogger<StoresController> logger, IStoreRepository repository, IMapper mapper) {
      _logger = logger;
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetStores() {
      var stores = _repository.GetStores().Select(store => new StoreModel(store));
      return Ok(stores);
    }

    [HttpGet("{id}")]
    public IActionResult GetStores(int id) {
      var store = new StoreModel(_repository.GetStore(id));
      return Ok(store);
    }

    [HttpPatch]
    public IActionResult UpdateStores(StoreModel model) {
      var store = _mapper.Map<Store>(model);
      // if (!_repository.GetStore(store.Id).Secret.Equals(store.Secret))
      //  return Forbid();
      store = _repository.UpdateStore(store);
      return Ok();
    }

    [HttpPost]
    public IActionResult PostStores(StoreModel model) {
      var store = _mapper.Map<Store>(model);
      store.Secret = Guid.NewGuid();
      store = _repository.AddStore(store);
      return Ok();
    }
  }
}
