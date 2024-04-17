using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductMarket.DAL.Interseptors;
using ProductMarket.DAL.Repository;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.DAL.DependencyInjection
{
    public static class DepensyInjection
    {
        public static void AddDataAccessLaoyer(this IServiceCollection services,IConfiguration configuration )
        {
            var connectionString = configuration.GetConnectionString("DataBaseConnection");
            services.AddSingleton<DateInterseptor>();
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseNpgsql(connectionString));
            services.Interseptor();
        }
        public static void Interseptor(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<Product>, BaseRepository<Product>>();
            services.AddScoped<IBaseRepository<Image>, BaseRepository<Image>>();
            services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
            services.AddScoped<IBaseRepository<Role>, BaseRepository<Role>>();
            services.AddScoped<IBaseRepository<UserRole>, BaseRepository<UserRole>>();
            services.AddScoped<IBaseRepository<UserToken>, BaseRepository<UserToken>>();
            services.AddScoped<IBaseRepository<Cart>, BaseRepository<Cart>>();
        }
    }
}
