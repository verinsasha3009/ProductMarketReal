using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using ProductMarket.DAL.Repository;
using ProductMarket.DAL;
using ProductMarket.Domain.Dto.Role;
using ProductMarket.Domain.Dto.User;
using ProductMarket.Domain.Dto.UserRole;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Interfaces.Services;
using ProductMarket.Domain.Result;
using ProductMarket.Domain.Services;
using ProductMarket.Domain.Validation;
using ProductMarket.Presentation.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using ProductMarket.Domain.Dto.Product;

namespace ProductMarket.Test
{
    public class RoleControllerTest
    {
        private readonly RoleController _controller;
        public RoleControllerTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=ProductMarket.Tests;User Id=postgres;Password=qwerpoiu")
                .Options;
            var DbContext = new ApplicationDbContext(options);
            var repositoryMockProduct = new BaseRepository<Role>(DbContext);
            var repositoryMockUser = new BaseRepository<User>(DbContext);
            var repositoryMockUserRole = new BaseRepository<UserRole>(DbContext);
            var mapper = new Mock<IMapper>();
            var validation = new RoleValidation();
            IRoleService prodService = new RoleService(repositoryMockProduct
                , repositoryMockUser,repositoryMockUserRole, validation, mapper.Object);
            _controller = new(prodService);
        }
        Random rand = new Random();
        [Fact]
        public async Task UserRoleTest()
        {
            var dto = new UserRole()
            {
                RoleId = 2,
                UserId = 1,
            };
            var p = $"FirstName_{rand.Next(121212)}";
            _controller.ModelState.AddModelError(p, "Required");
            var result = await _controller.AddUserRoleAsync(dto);
            var actionResult = Assert
            .IsType<ActionResult<BaseResult<UserRoleDto>>>(result);
            var badRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<UserRoleDto>>(badRequestResult.Value);

            var dtoUpdate = new UpdateUserRoleDto(2,3,1);
            _controller.ModelState.AddModelError("FirstName", "Required");
            var resultUpdate = await _controller.UpdateUserRoleAsync(dtoUpdate);
            var actionResultUpdate = Assert
            .IsType<ActionResult<BaseResult<UserRoleDto>>>(resultUpdate);
            var badRequestResultUpdate = Assert.IsType<OkObjectResult>(actionResultUpdate.Result);
            Assert.IsType<BaseResult<UserRoleDto>>(badRequestResultUpdate.Value);


            _controller.ModelState.AddModelError("FirstName", "Required");
            var resultDelete = await _controller.DeleteUserRoleAsync("qwerwer", "Role90842");
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<UserRoleDto>>>(resultDelete);
            var badRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<UserRoleDto>>(badRequestResultDelete.Value);

        }
        [Fact]
        public async Task RoleTest()
        {
            var dto = new CreateRoleDto($"Role{rand.Next(121212)}");
            _controller.ModelState.AddModelError("FirstName", "Required");
            var result = await _controller.AddRoleAsync(dto);
            var actionResult = Assert
            .IsType<ActionResult<BaseResult<RoleDto>>>(result);
            var badRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<RoleDto>>(badRequestResult.Value);

            var dtoUpdate = new RoleDto(1, $"Role{rand.Next(121212)}");
            _controller.ModelState.AddModelError("FirstName", "Required");
            var resultUpdate = await _controller.UpdateRoleAsync(dtoUpdate);
            var actionResultUpdate = Assert
            .IsType<ActionResult<BaseResult<RoleDto>>>(resultUpdate);
            var badRequestResultUpdate = Assert.IsType<OkObjectResult>(actionResultUpdate.Result);
            Assert.IsType<BaseResult<RoleDto>>(badRequestResultUpdate.Value);

            _controller.ModelState.AddModelError("FirstName", "Required");
            var resultDelete = await _controller.DeleteRoleAsync(1);
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<RoleDto>>>(resultDelete);
            var badRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<RoleDto>>(badRequestResultDelete.Value);
        }
    }
}
