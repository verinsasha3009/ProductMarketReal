using Microsoft.AspNetCore.Mvc;
using ProductMarket.Domain.Services;
using ProductMarket.Domain.Dto.User;
using ProductMarket.Domain.Dto;
using ProductMarket.Domain.Interfaces.Services;
using ProductMarket.Domain.Result;
using ProductMarket.Domain.Dto.UserRole;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Dto.Role;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace ProductMarket.Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Authorize(Roles = "Admin")]
    public class RoleController: Controller
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService) 
        {
            _roleService = roleService;
        }
        /// <summary>
        /// Добавление роли пользователю
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("AddUserRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<UserRoleDto>>> AddUserRoleAsync([FromBody] UserRole dto)
        {
            var i = await _roleService.AddUserRoleAsync(dto);
            if (i.IsSucces)
            {
                return Ok(i);
            }
            return BadRequest(i);
        }
        /// <summary>
        /// Создание новой роли
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("AddNewRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<UserRoleDto>>> AddRoleAsync([FromBody] CreateRoleDto dto)
        {
            var i = await _roleService.CreateRoleAsync(dto);
            if (i.IsSucces)
            {
                return Ok(i);
            }
            return BadRequest(i);
        }
        /// <summary>
        /// Удаление роли
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteRole/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<UserRoleDto>>> DeleteRoleAsync(int id)
        {
            var i = await _roleService.DeleteRoleAsync(id);
            if (i.IsSucces)
            {
                return Ok(i);
            }
            return BadRequest(i);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Login"></param>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        [HttpDelete("DeleteUserRole/{Login}/{RoleName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<UserRoleDto>>> DeleteUserRoleAsync( string Login, string RoleName)
        {
            var i = await _roleService.DeleteUserRoleAsync(Login,RoleName);
            if (i.IsSucces)
            {
                return Ok(i);
            }
            return BadRequest(i);
        }
        /// <summary>
        /// Обновление роли
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("UpdateRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<UserRoleDto>>> UpdateRoleAsync(RoleDto dto)
        {
            var i = await _roleService.UpdateRoleAsync(dto);
            if (i.IsSucces)
            {
                return Ok(i);
            }
            return BadRequest(i);
        }
        /// <summary>
        /// Обновление роли у пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("UpdateUserRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<UserRoleDto>>> UpdateUserRoleAsync([FromBody] UpdateUserRoleDto dto)
        {
            var i = await _roleService.UpdateUserRoleAsync(dto);
            if (i.IsSucces)
            {
                return Ok(i);
            }
            return BadRequest(i);
        }
    }
}
