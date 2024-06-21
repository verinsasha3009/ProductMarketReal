using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductMarket.DAL.Repository;
using ProductMarket.DAL;
using ProductMarket.Domain.Dto;
using ProductMarket.Domain.Dto.Product;
using ProductMarket.Domain.Dto.User;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Interfaces.Services;
using ProductMarket.Domain.Result;
using ProductMarket.Domain.Services;
using ProductMarket.Domain.Settings;
using ProductMarket.Presentation.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ProductMarket.Application.Mapping;

namespace ProductMarket.Test
{
    public class UserControllerAndTokenControllerTest
    {
        private readonly TokenController _controller;
        private readonly UserController _userController;
        public  UserControllerAndTokenControllerTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=ProductMarket.Tests;User Id=postgres;Password=qwerpoiu")
                .Options;
            var DbContext = new ApplicationDbContext(options);
            var userRep = new BaseRepository<User>(DbContext);
            var userTokenRep = new BaseRepository<UserToken>(DbContext);

            var jwtSettings = new JwtSettings()
            {
                JwtKey = "VDdYF0TsFr2zAIMuNAzEgIDxaEXuO7bm",
                Audience = "PetStore",
                Authority = "PetStore",
                Issuer = "PetStore",
                RefreshTokenValidityInDays = 7,
                Lifitime = 15
            };
            var opt = Options.Create<JwtSettings>(jwtSettings);
            IUserTokenService tokenService = new TokenService(opt, userRep);
            _controller = new TokenController(tokenService);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();

            var userService = new UserService(userRep, tokenService,userTokenRep,mapper);
            _userController = new UserController(userService);
        }
        
        [Fact]
        public async Task Test()
        {
            var rand = new Random();
            _controller.ModelState.AddModelError("Error", "Required");
            var userName = $"{rand.Next(110000)}--userName";
            var dtoReg = new RegistrUserDto(userName,"qwertyuiop","qwertyuiop");
            var resultReg = await _userController.Registration(dtoReg);
            var actionResultReg = Assert
            .IsType<ActionResult<BaseResult<UserDto>>>(resultReg);
            var badRequestResultReg = Assert.IsType<OkObjectResult>(actionResultReg.Result);
            Assert.IsType<BaseResult<UserDto>>(badRequestResultReg.Value);

            var dtoLog = new UserDto(userName, "qwertyuiop");
            var resultLog = await _userController.Login(dtoLog);
            var actionResultLog = Assert
            .IsType<ActionResult<BaseResult<TokenDto>>>(resultLog);
            var badRequestResultLog = Assert.IsType<OkObjectResult>(actionResultLog.Result);
            var Data = Assert.IsType<BaseResult<TokenDto>>(badRequestResultLog.Value);
            Assert.NotNull(Data);

            var dto = new TokenDto()
            {
                AccessToken = Data.Data.AccessToken,
                RefreshToken = Data.Data.RefreshToken,
            };
            var result = await _controller.RefreshTokenAsync(dto);
            var actionResult = Assert
            .IsType<ActionResult<BaseResult<TokenDto>>>(result);
            var badRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<TokenDto>>(badRequestResult.Value);
        }
    }
}
