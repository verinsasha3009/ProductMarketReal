using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Interfaces
{
    public interface IEntityId<T>  where T : struct
    {
        T Id { get; set; }
    }
}
