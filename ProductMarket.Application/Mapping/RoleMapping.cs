using AutoMapper;
using ProductMarket.Domain.Dto.Role;
using ProductMarket.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Application.Mapping
{
    public class RoleMapping : Profile
    {
        public RoleMapping()
        {
            CreateMap<Role, RoleDto>()
                .ForCtorParam(ctorParamName: "Id", m => m.MapFrom(p => p.Id))
                .ForCtorParam(ctorParamName: "Name", m => m.MapFrom(p => p.Name))
                .ReverseMap();
        }
    }
}
