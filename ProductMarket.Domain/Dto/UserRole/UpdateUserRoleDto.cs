using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Dto.UserRole
{
    public record UpdateUserRoleDto(int RoleNameId, int NewRoleNameId,int UserId);
}
