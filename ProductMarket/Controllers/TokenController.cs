using Microsoft.AspNetCore.Mvc;
using ProductMarket.Domain.Services;
using ProductMarket.Domain.Dto;
using ProductMarket.Domain.Interfaces.Repository;
using ProductMarket.Domain.Interfaces.Services;
using ProductMarket.Domain.Result;
using Asp.Versioning;

namespace ProductMarket.Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TokenController : Controller
    {
        private readonly IUserTokenService _tokenService;
        public TokenController(IUserTokenService tokenService)
        {
            _tokenService = tokenService;
        }
        /// <summary>
        /// проверка валидности токена
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<TokenDto>>> RefreshTokenAsync([FromBody]TokenDto dto)
        {
            var i = await _tokenService.RefreshToken(dto);
            if (i.IsSucces)
            {
                return Ok(i);
            }
            return BadRequest(i);
        }
    }
}
