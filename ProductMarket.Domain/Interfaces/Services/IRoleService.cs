using ProductMarket.Domain.Dto.Role;
using ProductMarket.Domain.Dto.UserRole;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Result;

namespace ProductMarket.Domain.Interfaces.Services
{
    public interface IRoleService
    {
       public Task<BaseResult<RoleDto>> CreateRoleAsync(CreateRoleDto dto);
       public Task<BaseResult<RoleDto>> UpdateRoleAsync(RoleDto roleDto);
       public Task<BaseResult<RoleDto>> DeleteRoleAsync(int roleId);
       public Task<BaseResult<UserRoleDto>>AddUserRoleAsync(UserRole dto);
       public Task<BaseResult<UserRoleDto>> UpdateUserRoleAsync(UpdateUserRoleDto dto);
       public Task<BaseResult<UserRoleDto>> DeleteUserRoleAsync(string Login, string RoleName);
    }
}
