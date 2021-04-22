using AutoMapper;
using CapWatchBackend.Domain.Entities;
using CapWatchBackend.WebApi.Models;
using System;

namespace CapWatchBackend.WebApi.Mapper {
  public class MapperProfile : Profile {
    public MapperProfile() {
      CreateMap<StoreModel, Store>()
        .ForMember(dest => dest.Secret, map => map.MapFrom(src => Guid.Parse(src.Secret)))
        .ReverseMap();
      CreateMap<StoreOverview, Store>().ReverseMap();
      CreateMap<StoreNew, Store>().ReverseMap();
      CreateMap<StoreNewResponse, Store>().ReverseMap();
      CreateMap<StoreTypeModel, Store>().ReverseMap();
    }
  }
}
