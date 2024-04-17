using ProductMarket.Domain.Dto.Role;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Interfaces.Validation
{
    public interface IRoleValidation : IReferenceOnNull<Role>
    {
        BaseResult<RoleDto> Validate(User user,Role role);
    }
}
