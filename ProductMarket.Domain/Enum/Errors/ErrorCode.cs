using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Enum.Errors
{
    public enum ErrorCode
    {
        СartNotFoundError = 0,
        UnexpectedCartError = 1,
        RemoveCartProductError = 2,

        ProductIsNotFound = 10,
        ProductErrorCreate = 11,
        ProductUpdateError = 12,
        ProductDeleteError = 13,
        ProductNotDetected = 14,
        ProductUnexpectedError = 15,
        ProductAlreadyExists = 16,
        ProductInCartAlreadyExists = 17,

        UserUnauthorizedAccess = 20,
        UserNotFound = 21,
        PasswordIsWrong = 22,
        UserRegistrationUnexpectedError = 23,
        UserAlreadyExists=24,
        PasswordsDontMatch = 25,
        UserTokenCreationError=26,
        UserTokenUpdateError=27,

        InternalServerError =44,

        InvalidClientRequrest = 51,

        RoleHasAlreadyBeenAdded =60,
        RoleAlreadyExists=61,
        RoleNotFound=62,
        UserRoleNotFound =63,
    }
}
