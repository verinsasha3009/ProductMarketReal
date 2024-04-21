using ProductMarket.Domain.Dto.Role;
using ProductMarket.Domain.Dto.User;
using ProductMarket.Domain.Dto.UserRole;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Test
{
    public class RoleControllerTest
    {
        Random rand = new Random();
        [Fact]
        public async Task AddUserRoleTest()
        {
            var httpClient = new HttpClient();
            var userRole = new UserRole()
            {
                UserId = 6,
                RoleId = 2,
            };
            var result = await httpClient.PostAsJsonAsync("https://localhost:7147/api/v1/Role/AddUserRole", userRole);
            var data = result.Content.ReadFromJsonAsync<BaseResult<UserRoleDto>>();
            var q = (int?)HttpStatusCode.OK;
            var e = (int?)result.StatusCode;
            Assert.Equal((int?)HttpStatusCode.OK, (int?)result.StatusCode);
            Assert.NotNull(data);
            Assert.IsType<BaseResult<UserRoleDto>>(data?.Result);

            var resultNew = await httpClient.DeleteAsync
                ($"https://localhost:7147/api/v1/Role/DeleteUserRole/{data.Result.Data.Login}/{data.Result.Data.RoleName}" );
            var dataNew = resultNew.Content.ReadFromJsonAsync<BaseResult<UserRoleDto>>();
            var qw = (int?)HttpStatusCode.OK;
            var ew = (int?)result.StatusCode;
            Assert.Equal((int?)HttpStatusCode.OK, (int?)resultNew.StatusCode);
            Assert.NotNull(dataNew);
        }
        [Fact]
        public async Task AddRoleTest()
        {
            var httpClient = new HttpClient();
            var role = new CreateRoleDto($"{rand.Next(12121)}RoleTest");
            var result = await httpClient.PostAsJsonAsync("https://localhost:7147/api/v1/Role/AddNewRole", role);
            var data = result.Content.ReadFromJsonAsync<BaseResult<UserRoleDto>>();

            Assert.Equal((int?)HttpStatusCode.OK, (int?)result.StatusCode);
            Assert.NotNull(data);
            Assert.IsType<BaseResult<UserRoleDto>>(data?.Result);
        }
        [Fact]
        public async Task UpdateRoleTest()
        {
            var httpClient = new HttpClient();
            var role = new RoleDto(2, "Test");
            var result = await httpClient.PutAsJsonAsync("https://localhost:7147/api/v1/Role/UpdateRole", role);
            var data = result.Content.ReadFromJsonAsync<BaseResult<UserRoleDto>>();

            Assert.Equal((int?)HttpStatusCode.OK, (int?)result.StatusCode);
            Assert.NotNull(data);
            Assert.IsType<BaseResult<UserRoleDto>>(data?.Result);
        }
        [Fact]
        public async Task DeleteRoleTest()
        {
            var httpClient = new HttpClient();
            var result = await httpClient.DeleteAsync("https://localhost:7147/api/v1/Role/Deleterole/9");
            var data = result.Content.ReadFromJsonAsync<BaseResult<UserRoleDto>>();

            Assert.Equal((int?)HttpStatusCode.OK, (int?)result.StatusCode);
            Assert.NotNull(data);
            Assert.IsType<BaseResult<UserRoleDto>>(data?.Result);
        }

    }
}
