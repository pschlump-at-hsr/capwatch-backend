using AutoMapper;
using CapWatchBackend.Domain.Entities;
using CapWatchBackend.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapWatchBackend.WebApi.Mapper {
  public class MapperProfile : Profile {
    public MapperProfile() {
      CreateMap<StoreModel, Store>()
        .ForMember(dest => dest.Secret, map => map.MapFrom(src => Guid.Parse(src.Secret)))
        .ReverseMap();
    }
  }
}
