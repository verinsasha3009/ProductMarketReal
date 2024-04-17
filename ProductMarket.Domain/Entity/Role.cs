﻿using ProductMarket.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Entity
{
    public class Role : IEntityId<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
    }
}
