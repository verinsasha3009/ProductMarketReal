using ProductMarket.Domain.Dto.Product;
using ProductMarket.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Interfaces.Validation
{
    public interface IReferenceOnNull<T> where T : class
    {
         BaseResult EntityNullReference(T entity);
    }
}
