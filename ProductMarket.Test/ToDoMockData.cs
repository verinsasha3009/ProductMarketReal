using ProductMarket.Domain.Dto.Product;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Test
{
    public static class ToDoMockData
    {
        public static BaseResult<ProductDto> GetTodos()
        {
            return new BaseResult<ProductDto>() { Data = new ProductDto(1, "", "", DateTime.UtcNow.ToString()) };
            //{
            //    Id = 1,
            //    //       Name="",
            //    //       Description="",
            //    //       CreatedAt = DateTime.Now,
            //    //       CreatedBy = 58585,
            //    //       Images = new List<Image>() {},
            //    //       Carts = new()
            //};
        }
    }
}
