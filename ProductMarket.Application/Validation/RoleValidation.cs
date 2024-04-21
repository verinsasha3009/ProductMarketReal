using ProductMarket.Application.Resources.Errors;
using ProductMarket.Domain.Dto.Product;
using ProductMarket.Domain.Dto.Role;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Enum.Errors;
using ProductMarket.Domain.Interfaces.Validation;
using ProductMarket.Domain.Result;

namespace ProductMarket.Domain.Validation
{
    public class RoleValidation : IRoleValidation
    {
        public BaseResult EntityNullReference(Role role)
        {
            if (role == null)
            {
                return new BaseResult<ProductDto>
                {
                    ErrorMessage = ErrorMessage.RoleNotFound,
                    ErrorCode = (int)ErrorCode.RoleNotFound,
                };
            }
            return new BaseResult<ProductDto>();
        }

        public BaseResult<RoleDto> Validate(User user,Role role)
        {
            if (role == null)
            {
                return new BaseResult<RoleDto>
                {
                    ErrorMessage = ErrorMessage.RoleNotFound,
                    ErrorCode = (int)ErrorCode.RoleNotFound,
                };
            }
            if (user == null)
            {
                return new BaseResult<RoleDto>
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCode.UserNotFound,
                };
            }
            return new BaseResult<RoleDto>();
        }
    }
}
