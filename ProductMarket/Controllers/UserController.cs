using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ProductMarket.Domain.Dto;
using ProductMarket.Domain.Dto.User;
using ProductMarket.Domain.Interfaces.Services;
using ProductMarket.Domain.Result;

namespace ProductMarket.Presentation.Controllers
{
    /// <summary>
    /// Контроллер для работы с пользователем
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("registration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<UserDto>>> Registration([FromBody] RegistrUserDto dto)
        {
            var i = await _userService.RegistrationNewUserAsync(dto);
            if (i.IsSucces)
            {
                return Ok(i);
            }
            return BadRequest(i);
        }
        /// <summary>
        /// Вход пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<TokenDto>>> Login([FromBody] UserDto dto)
        {
            var i = await _userService.LoginUserAsync(dto);
            if (i.IsSucces)
            {
                return Ok(i);
            }
            return BadRequest(i);
        }
    }
}
