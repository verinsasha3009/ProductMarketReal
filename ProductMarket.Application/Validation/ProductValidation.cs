using ProductMarket.Application.Resources.Errors;
using ProductMarket.Domain.Dto.Product;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Enum.Errors;
using ProductMarket.Domain.Interfaces.Validation;
using ProductMarket.Domain.Result;

namespace ProductMarket.Domain.Validation
{
    public class ProductValidation : IProductValidation
    {
        public BaseResult CreateReportValidator(User user,Product product)
        {
            if (product != null)
            {
                return new BaseResult<ProductDto>
                {
                    ErrorMessage = ErrorMessage.ProductAlreadyExists,
                    ErrorCode = (int)ErrorCode.ProductAlreadyExists,
                };
            }
            if (user == null)
            {
                return new BaseResult<ProductDto>
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCode.UserNotFound
                };
            }
            return new BaseResult();
        }
        public BaseResult EntityNullReference(Product product)
        {
            if (product == null)
            {
                return new BaseResult<ProductDto>
                {
                    ErrorMessage = ErrorMessage.ProductIsNotFound,
                    ErrorCode = (int)ErrorCode.ProductIsNotFound,
                };
            }
            return new BaseResult<ProductDto>();
        }
    }
}
