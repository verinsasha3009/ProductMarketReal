using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductMarket.Domain.Settings;
using System.Reflection;
using System.Text;
namespace ProductMarket.Presentation
{
    public static class Startup
    {
        public static void AddAuthenticationAndAuthorization(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddAuthorization();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var options = builder.Configuration.GetSection(JwtSettings.Defaultsection).Get<JwtSettings>();
                var jwtKey = options.JwtKey;
                var issuer = options.Issuer;
                var audience = options.Audience;
                o.Authority = options.Authority;
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };

            });
        }
        /// <summary>
        /// Подключение swagger
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddApiVersioning()
                .AddApiExplorer(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "ProductMarket.API",
                    Description = " This is version 1.0",
                    TermsOfService = new Uri("https://yandex.ru/search/?text=переводчик&clid=2270455&banerid"
                    + "=0758005015%3A64981a68d6814de73a48d5ae&win=599&lr=12"),
                    Contact = new OpenApiContact()
                    {
                        Name = "Test contact",
                        Url = new Uri("https://yandex.ru/search/?text=переводчик&clid=2270455&banerid"
                        + "=0758005015%3A64981a68d6814de73a48d5ae&win=599&lr=12")
                    }
                });
                options.SwaggerDoc("v2", new OpenApiInfo()
                {
                    Version = "v2",
                    Title = "ProductMarket.API",
                    Description = " This is version 2.0",
                    TermsOfService = new Uri("https://yandex.ru/search/?text=переводчик&clid=2270455&banerid"
                    + "=0758005015%3A64981a68d6814de73a48d5ae&win=599&lr=12"),
                    Contact = new OpenApiContact()
                    {
                        Name = "Test contact",
                        Url = new Uri("https://yandex.ru/search/?text=переводчик&clid=2270455&banerid"
                        + "=0758005015%3A64981a68d6814de73a48d5ae&win=599&lr=12")
                    }
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Введите валидный токен ",
                    Name = "Authorize",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {

                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type =ReferenceType.SecurityScheme,
                                Id="Bearer"
                            },
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        Array.Empty<string>()
                    }

                });
                var xmlFileName = $"{Assembly.GetExecutingAssembly()
                    .GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
            });
        }
        public static void AddRedis(this IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(options => {
                options.Configuration = "localhost";
                options.InstanceName = "local";
            });
        }
    }
}