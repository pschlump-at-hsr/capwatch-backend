using AutoMapper;
using CapWatchBackend.Application.Handlers;
using CapWatchBackend.Domain.Entities;
using CapWatchBackend.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

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
    public async Task<IActionResult> GetStores(string filter = null) {
      var stores = filter != null ? await _handler.GetStores(filter) : await _handler.GetStores();
      var result = stores.Select(store => _mapper.Map<StoreOverview>(store));
      return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStore(Guid id) {
      var store = await _handler.GetStore(id);
      var result = _mapper.Map<StoreOverview>(store);
      return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateStores(StoreModel model) {
      var store = _mapper.Map<Store>(model);
      await _handler.UpdateStoreAsync(store);
      return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> PostStores(StoreNew model) {
      var store = _mapper.Map<Store>(model);
      await _handler.AddStoreAsync(store);
      var result = _mapper.Map<StoreNewResponse>(store);
      return Ok(result);
    }
  }
}
