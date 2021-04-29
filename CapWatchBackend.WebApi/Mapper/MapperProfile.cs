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
      CreateMap<StoreOverviewModel, Store>().ReverseMap();
      CreateMap<NewStoreModel, Store>().ReverseMap();
      CreateMap<NewStoreResponseModel, Store>().ReverseMap();
      CreateMap<StoreTypeModel, StoreType>().ReverseMap();
    }
  }
}
