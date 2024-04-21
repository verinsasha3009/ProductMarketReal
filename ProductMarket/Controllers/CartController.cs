using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ProductMarket.Domain.Dto.Product;
using ProductMarket.Domain.Dto.ProductCart;
using ProductMarket.Domain.Interfaces.Services;
using ProductMarket.Domain.Result;

namespace ProductMarket.Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CartController : Controller
    {
        private readonly IProductCartService _productCartService;
        public CartController(IProductCartService productCartService)
        {
            _productCartService = productCartService;
        }

        /// <summary>
        /// Добавление продукта в корзину пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductDto>>> AddProductInCart(ProductCartDto dto)
        {
            var i = await _productCartService.AddCartProductAsync(dto);
            if (i.IsSucces)
            {
                return Ok(i);
            }
            return BadRequest(i);
        }
        /// <summary>
        /// Удаление продукта из корзины пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{userId}/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductDto>>> RemoveProductInCart(int userId, int id)
        {
            var i = await _productCartService.RemoveCartProductAsync(userId, id);
            if (i.IsSucces)
            {
                return Ok(i);
            }
            return BadRequest(i);
        }
    }
}
