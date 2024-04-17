using ProductMarket.Domain.Dto;
using ProductMarket.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Interfaces.Services
{
    public interface IUserTokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task<BaseResult<TokenDto>> RefreshToken(TokenDto dto);
    }
}
