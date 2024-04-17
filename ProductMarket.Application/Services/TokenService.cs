using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductMarket.Domain.Resources.Errors;
using ProductMarket.Domain.Dto;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Enum.Errors;
using ProductMarket.Domain.Interfaces.Repository;
using ProductMarket.Domain.Interfaces.Services;
using ProductMarket.Domain.Result;
using ProductMarket.Domain.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProductMarket.Domain.Services
{
    public class TokenService : IUserTokenService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly string _jwtKey;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenService(IOptions<JwtSettings> options, IBaseRepository<User> userRepo)
        {
            _jwtKey = options.Value.JwtKey;
            _issuer = options.Value.Issuer;
            _audience = options.Value.Audience;
            _userRepository = userRepo;
        }
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentails = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var securityToken =
                new JwtSecurityToken(_issuer, _audience, claims, null, DateTime.UtcNow.AddMinutes(10), credentails);
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }

        public string GenerateRefreshToken()
        {
            var randomNumbers = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumbers);
            return Convert.ToBase64String(randomNumbers);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey)),
                ValidateLifetime = true,
                ValidAudience = _audience,
                ValidIssuer = _issuer,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.CurrentCultureIgnoreCase))
                throw new SecurityTokenException(ErrorMessage.InvalidToken);
            return claimsPrincipal;
        }

        public async Task<BaseResult<TokenDto>> RefreshToken(TokenDto dto)
        {
            string accessToken = dto.AccessToken;
            string refreshToken = dto.RefreshToken;

            var claimsPrincipal = GetPrincipalFromExpiredToken(accessToken);
            var userName = claimsPrincipal.Identity?.Name;

            var user = await _userRepository.GetAll().Include(p => p.UserToken).FirstOrDefaultAsync(p => p.Login == userName);

            if (user == null)
            {
                if (user.UserToken.RefreshToken != refreshToken)
                {
                    if (user.UserToken.RefreshTokenExpiryTime <= DateTime.UtcNow)
                    {
                        return new BaseResult<TokenDto>
                        {
                            ErrorMessage = ErrorMessage.InvalidClientRequrest,
                            ErrorCode = (int)ErrorCode.InvalidClientRequrest,
                        };
                    }
                }
            }

            var newAccessToken = GenerateAccessToken(claimsPrincipal.Claims);
            var newRefreshToken = GenerateRefreshToken();

            user.UserToken.RefreshToken = newRefreshToken;
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
            return new BaseResult<TokenDto>()
            {
                Data = new TokenDto()
                {
                    AccessToken = accessToken,
                    RefreshToken = newRefreshToken,
                }
            };
        }
    }
}
