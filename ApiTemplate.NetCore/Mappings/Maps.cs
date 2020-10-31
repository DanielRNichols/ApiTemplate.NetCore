using ApiTemplate.NetCore.Data;
using ApiTemplate.NetCore.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTemplate.NetCore.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<Item, ItemDTO>().ReverseMap();
            CreateMap<Item, ItemCreateDTO>().ReverseMap();
        }
    }
}
