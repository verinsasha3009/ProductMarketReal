using Microsoft.EntityFrameworkCore;
using ProductMarket.Domain.Dto;
using ProductMarket.Domain.Dto.User;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Enum.Errors;
using ProductMarket.Domain.Interfaces.Repository;
using ProductMarket.Domain.Interfaces.Services;
using ProductMarket.Domain.Result;
using Serilog;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using ProductMarket.Application.Resources.Errors;

namespace ProductMarket.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<UserToken> _tokenRepository;
        private readonly IUserTokenService _tokenService;
        private readonly IMapper _mapper;
        public UserService(IBaseRepository<User> userRepository,IUserTokenService tokenService
            ,IBaseRepository<UserToken> tokenReopository,IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _tokenRepository = tokenReopository;
            _mapper = mapper;
        }
        public async Task<BaseResult<TokenDto>> LoginUserAsync(UserDto dto)
        {
            var user = await _userRepository.GetAll().Include(p=>p.Roles).FirstOrDefaultAsync(p => p.Login == dto.Login);
            if (user == null)
            {
                return new BaseResult<TokenDto>()
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCode.UserNotFound,
                };
            }
            if (!IsVerifyPassword(user.Password, dto.Password))
            {
                return new BaseResult<TokenDto>()
                {
                    ErrorMessage = ErrorMessage.PasswordIsWrong,
                    ErrorCode = (int)ErrorCode.PasswordIsWrong,
                };
            }
            var userToken = await _tokenRepository.GetAll().FirstOrDefaultAsync(p => p.UserId == user.Id);

            var userRoles = user.Roles;
            var claims = userRoles.Select(p => new Claim(ClaimTypes.Role, p.Name)).ToList();
            claims.Add(new Claim(ClaimTypes.Name, user.Login));

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();
            if (userToken == null)
            {
                try
                {
                    userToken = new UserToken()
                    {
                        UserId = user.Id,
                        RefreshToken = refreshToken,
                        RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7),
                    };
                    await _tokenRepository.CreateAsync(userToken);
                }
                catch (Exception ex)
                {
                    return new BaseResult<TokenDto>()
                    {
                        ErrorMessage = ErrorMessage.UserTokenCreationError,
                        ErrorCode = (int)ErrorCode.UserTokenCreationError
                    };
                }
            }
            else
            {
                try
                {
                    userToken.RefreshToken = refreshToken;
                    userToken.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                    _tokenRepository.Update(userToken);
                    await _tokenRepository.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return new BaseResult<TokenDto>()
                    {
                        ErrorMessage=ErrorMessage.UserTokenUpdateError,
                        ErrorCode = (int)ErrorCode.UserTokenUpdateError
                    };
                }
            }
            return new BaseResult<TokenDto>()
            {
                Data = new TokenDto()
                {
                    RefreshToken = refreshToken,
                    AccessToken = accessToken
                }
            };
        }

        public async Task<BaseResult<UserDto>> RegistrationNewUserAsync(RegistrUserDto dto)
        {
            if(dto.Password!= dto.LastPassword)
            {
                return new BaseResult<UserDto>()
                {
                    ErrorMessage = ErrorMessage.PasswordsDontMatch,
                    ErrorCode = (int)ErrorCode.PasswordsDontMatch,
                };
            }
            var usernull = await _userRepository.GetAll().FirstOrDefaultAsync(p=>p.Login == dto.Login);
            if (usernull != null)
            {
                return new BaseResult<UserDto>()
                {
                    ErrorMessage = ErrorMessage.UserAlreadyExists,
                    ErrorCode = (int)ErrorCode.UserAlreadyExists,
                };
            }
            try
            {
                var hashUserPassword = HashPassword(dto.Password);
                User user = new User()
                {
                    Login = dto.Login,
                    Password = hashUserPassword,
                    Roles = new List<Role> { new Role() { Name="User"} },
                };
                await _userRepository.CreateAsync(user);
                return new BaseResult<UserDto>()
                {
                     Data = _mapper.Map<UserDto>(user),
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<UserDto>()
                {
                    ErrorMessage = ErrorMessage.UserRegistrationUnexpectedError,
                    ErrorCode = (int)ErrorCode.UserRegistrationUnexpectedError,
                };
            }
        }
        private string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).ToLower();
        }
        private bool IsVerifyPassword(string userPasswordHash, string userPassword)
        {
            var hash = HashPassword(userPassword);
            return hash == userPasswordHash;
        }
    }
}
