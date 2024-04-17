using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Interfaces.Validation
{
    public interface IProductValidation : IReferenceOnNull<Product> 
    {
        BaseResult CreateReportValidator(User user, Product product);
    }
}
