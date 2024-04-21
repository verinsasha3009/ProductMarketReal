using AutoMapper;
using ProductMarket.Domain.Dto.User;
using ProductMarket.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Application.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User,UserDto>()
                .ForCtorParam(ctorParamName:"Login",m=>m.MapFrom(p=>p.Login))
                .ForCtorParam(ctorParamName:"Password",m=>m.MapFrom(m=>m.Password))
                .ReverseMap();
        }
    }
}
