﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Dto.User
{
    public record RegistrUserDto(string Login,string Password,string LastPassword);
}
