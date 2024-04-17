using ProductMarket.Domain.Dto;
using ProductMarket.Domain.Dto.User;
using ProductMarket.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<BaseResult<UserDto>> RegistrationNewUserAsync(RegistrUserDto dto);
        Task<BaseResult<TokenDto>> LoginUserAsync(UserDto dto);
    }
}
