using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Interfaces
{
    public interface IDataTimeSourse
    {
        DateTime CreatedAt { get; set; }
        long CreatedBy { get; set; }

        DateTime UpdatedAt { get; set; }
        long UpdatedBy { get; set; }
    }
}
