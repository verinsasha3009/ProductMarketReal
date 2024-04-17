using Microsoft.EntityFrameworkCore;
using ProductMarket.Domain.Resources.Errors;
using ProductMarket.DAL.Repository;
using ProductMarket.Domain.Dto.Role;
using ProductMarket.Domain.Dto.UserRole;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Enum.Errors;
using ProductMarket.Domain.Interfaces.Services;
using ProductMarket.Domain.Interfaces.Validation;
using ProductMarket.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductMarket.Domain.Interfaces.Repository;

namespace ProductMarket.Domain.Services
{
    public class RoleService : IRoleService
    {
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<UserRole> _userRoleRepository;
        private readonly IRoleValidation _roleValidation;
        public RoleService(IBaseRepository<Role> roleRepository, IBaseRepository<User> userRepository,
            IBaseRepository<UserRole> userRoleRepository,IRoleValidation roleValidation)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleValidation = roleValidation;
        }
        public async Task<BaseResult<RoleDto>> CreateRoleAsync(CreateRoleDto dto)
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(p=>p.Name == dto.Name);
            if(role != null)
            {
                return new BaseResult<RoleDto>()
                { 
                    ErrorMessage = ErrorMessage.RoleAlreadyExists,
                    ErrorCode = (int)ErrorCode.RoleAlreadyExists
                };
            }
            role = new()
            {
                Name = dto.Name
            };
            await _roleRepository.CreateAsync(role);
            return new BaseResult<RoleDto>() 
            {
                Data=new RoleDto(role.Id,role.Name)
            };
        }
        public async Task<BaseResult<UserRoleDto>> AddUserRoleAsync(UserRole dto)
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(p => p.Id == dto.RoleId);
            var user = await _userRepository.GetAll().Include(p=>p.Roles).FirstOrDefaultAsync(p => p.Id == dto.UserId);

            var result = _roleValidation.Validate(user, role);
            if (!result.IsSucces)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorMessage= result.ErrorMessage,
                    ErrorCode = result.ErrorCode,
                };
            }
            if(_roleValidation.EntityNullReference(user.Roles.FirstOrDefault(p => p.Id == role.Id)).IsSucces)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorMessage = ErrorMessage.RoleHasAlreadyBeenAdded,
                    ErrorCode = (int)ErrorCode.RoleHasAlreadyBeenAdded,
                };
            }

            await _userRoleRepository.CreateAsync(new UserRole() { RoleId = role.Id, UserId = user.Id });

            return new BaseResult<UserRoleDto>()
            {
                Data = new UserRoleDto(user.Login, role.Name)
            };
        }

        public async Task<BaseResult<RoleDto>> DeleteRoleAsync(int roleId)
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(p=>p.Id == roleId);

            var result = _roleValidation.EntityNullReference(role);
            if(!result.IsSucces)
            {
                return new BaseResult<RoleDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode,
                };
            }
            _roleRepository.Remove(role);
            return new BaseResult<RoleDto>()
            {
                Data = new RoleDto(role.Id, role.Name)
            };
        }

        public async Task<BaseResult<UserRoleDto>> DeleteUserRoleAsync(UserRoleDto dto)
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(p=>p.Name == dto.RoleName);
            var user = await _userRepository.GetAll().Include(p=>p.Roles).FirstOrDefaultAsync(p => p.Login == dto.Login);
            var result = _roleValidation.Validate(user, role);
            if(!result.IsSucces)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode,
                };
            }
            var userRole = await _userRoleRepository.GetAll().Where(p=>p.UserId == user.Id).FirstOrDefaultAsync(p => p.RoleId == role.Id);
            _userRoleRepository.Remove(userRole);
            await _userRoleRepository.SaveChangesAsync();
            return new BaseResult<UserRoleDto>()
            {
                Data= new UserRoleDto(dto.Login,dto.RoleName)
            };
        }

        public async Task<BaseResult<RoleDto>> UpdateRoleAsync(RoleDto roleDto)
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(p=>p.Id==roleDto.Id);

            var result =_roleValidation.EntityNullReference(role);
            if (!result.IsSucces)
            {
                return new BaseResult<RoleDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode,
                };
            }
            role.Name = roleDto.Name;
            _roleRepository.Update(role);

            return new BaseResult<RoleDto>()
            {
                Data = new RoleDto(role.Id, role.Name)
            };
        }

        public async Task<BaseResult<UserRoleDto>> UpdateUserRoleAsync(UpdateUserRoleDto dto)
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(p => p.Id == dto.RoleNameId);
            var user = await _userRepository.GetAll().Include(p => p.Roles).FirstOrDefaultAsync(p => p.Id == dto.UserId);
            var result = _roleValidation.Validate(user, role);
            if (!result.IsSucces)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode,
                };
            }
            role = await _roleRepository.GetAll().FirstOrDefaultAsync(p => p.Id == dto.NewRoleNameId);
            var newResult =_roleValidation.EntityNullReference(role);
            if (!newResult.IsSucces)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorMessage = newResult.ErrorMessage,
                    ErrorCode = newResult.ErrorCode,
                };
            }
            var userRole = await _userRoleRepository.GetAll().Where(p => p.UserId == dto.UserId).FirstOrDefaultAsync(p => p.RoleId == dto.RoleNameId);
            userRole.RoleId = role.Id;
            _userRoleRepository.Update(userRole);
            await _userRoleRepository.SaveChangesAsync();
            //using (var transaction = await _unitOFWork.BeginTransitionAsync())
            //{
            //    try
            //    {
            //        var userRole = await _unitOFWork.UserRoles.GetAll()
            //        .Where(p => p.RoleId == dto.RoleNameId)
            //        .FirstOrDefaultAsync(p => p.UserId == user.Id);

            //        userRole.RoleId = dto.NewRoleNameId;

            //        _unitOFWork.UserRoles.Update(userRole);
            //        await _unitOFWork.SaveChangesAsync();
            //        var newUserRole = new UserRole()
            //        {
            //            UserId = user.Id,
            //            RoleId = dto.NewRoleNameId,
            //        };
            //        await _unitOFWork.UserRoles.CreateAsync(newUserRole);
            //        await _unitOFWork.SaveChangesAsync();
            //        await transaction.CommitAsync();

            //    }
            //    catch (Exception ex)
            //    {
            //        await transaction.RollbackAsync();
            //    }

            //}
            return new BaseResult<UserRoleDto>()
            {
                Data = new UserRoleDto(user.Login, role.Name)
            };
        }
    }
}
