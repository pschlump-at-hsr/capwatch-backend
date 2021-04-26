using AutoMapper;
using CapWatchBackend.Application.Handlers;
using CapWatchBackend.Domain.Entities;
using CapWatchBackend.WebApi.Hubs;
using CapWatchBackend.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CapWatchBackend.WebApi.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class StoresController : ControllerBase {
    private readonly IStoreHandler _handler;
    private readonly IMapper _mapper;
    private readonly IHubContext<StoresHub> _hubContext;

    public StoresController(IStoreHandler handler, IMapper mapper, IHubContext<StoresHub> hubContext) {
      _handler = handler;
      _mapper = mapper;
      _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetStores(string filter = null) {
      var stores = filter != null ? await _handler.GetStoresAsync(filter) : await _handler.GetStoresAsync();
      var result = stores.Select(store => _mapper.Map<StoreOverviewModel>(store));
      return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStore(Guid id) {
      var store = await _handler.GetStoreAsync(id);
      var result = _mapper.Map<StoreOverviewModel>(store);
      return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateStores(StoreModel model) {
      var store = _mapper.Map<Store>(model);
      await _handler.UpdateStoreAsync(store);
      await Notify("Update", store);
      return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> PostStores(NewStoreModel model) {
      var store = _mapper.Map<Store>(model);
      await _handler.AddStoreAsync(store);
      var result = _mapper.Map<NewStoreResponseModel>(store);
      await Notify("New", store);
      return Ok(result);
    }

    private async Task Notify(string method, Store store) {
      var result = _mapper.Map<StoreOverviewModel>(store);
      await _hubContext.Clients.All.SendAsync(method, result);
    }
  }
}
