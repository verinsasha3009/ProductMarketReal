using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ProductMarket.Domain.Dto.Product;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Interfaces.Services;
using ProductMarket.Domain.Result;

namespace ProductMarket.Presentation.Controllers
{
    /// <summary>
    /// Контроллер по управлению Продуктами
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService) { 
            _productService = productService;
        }
        /// <summary>
        /// Созданеие Продукта
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductDto>>> CreateProduct([FromBody]CreateProductDto dto)
        {
            var i = await _productService.CreateProductAsync(dto);
            if (i.IsSucces)
            {
                return Ok(i);
            }
            return BadRequest(i);
        }
        /// <summary>
        /// Считать все продукты у пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("GetAll/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CollectResult<Product>>> GetAll(int userId)
        {
            var i = await _productService.GetAllAsync(userId);
            if (i.IsSucces)
            {
                return Ok(i);
            }
            return BadRequest(i);
        }
        /// <summary>
        /// Удаление продукта 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductDto>>> DeleteProduct(int Id)
        {
            var i = await _productService.DeleteProductAsync(Id);
            if (i.IsSucces)
            {
                return Ok(i);
            }
            return BadRequest(i);
        }
        /// <summary>
        /// Обновление параметров продукта
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductDto>>> UpdateProduct([FromBody] UpdateProductDto dto)
        {
            var i = await _productService.UpdateProductAsync(dto);
            if (i.IsSucces)
            {
                return Ok(i);
            }
            return BadRequest(i);
        }
        /// <summary>
        /// Поиск продукта в бд
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductDto>>> GetProduct(int Id)
        {
            var i = await _productService.GetProductAsync(Id);
            if (i.IsSucces)
            {
                return Ok(i);
            }
            return BadRequest(i);
        }
    }
}
