using AutoMapper;
using ProductMarket.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductMarket.Domain.Dto.Product;

namespace ProductMarket.Domain.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping() {
            CreateMap<Product, ProductDto>()
                .ForCtorParam(ctorParamName: "Id", m => m.MapFrom(s => s.Id))
                .ForCtorParam(ctorParamName: "Name", m => m.MapFrom(s => s.Name))
                .ForCtorParam(ctorParamName: "Description", m => m.MapFrom(s => s.Description))
                .ForCtorParam(ctorParamName: "CreateAt", m => m.MapFrom(s => s.CreatedAt))
                .ReverseMap();
        }
    }
}
