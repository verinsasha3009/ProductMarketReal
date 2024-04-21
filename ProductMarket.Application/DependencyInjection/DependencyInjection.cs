using Microsoft.Extensions.DependencyInjection;
using ProductMarket.Domain.Mapping;
using ProductMarket.Domain.Services;
using ProductMarket.Domain.Validation;
using ProductMarket.DAL.Repository;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Interfaces.Repository;
using ProductMarket.Domain.Interfaces.Services;
using ProductMarket.Domain.Interfaces.Validation;
using ProductMarket.Application.Mapping;

namespace ProductMarket.Domain.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProductMapping));
            services.AddAutoMapper(typeof(RoleMapping));
            services.AddAutoMapper(typeof(UserMapping));
            Initialize(services);
        }
        public static void Initialize(this IServiceCollection services)
        {
            services.AddScoped<IProductValidation, ProductValidation>();
            services.AddScoped<IRoleValidation, RoleValidation>();

            services.AddScoped<IProductService,ProductService>();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IProductCartService,ProductCartService>();
            services.AddScoped<IUserTokenService,TokenService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddTransient<IRedisCacheService,RedisCacheService>();
        }
    }
}
