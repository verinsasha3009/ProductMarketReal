using ProductMarket.Domain.DependencyInjection;
using ProductMarket.DAL.DependencyInjection;
using ProductMarket.Domain.Settings;
using ProductMarket.Presentation.Middleware;
using ProductMarket.Presentation;
using Serilog;


var builder = WebApplication.CreateBuilder();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.Defaultsection));

builder.Services.AddControllers();

builder.Services.AddAuthenticationAndAuthorization(builder);
builder.Services.AddSwagger();
builder.Services.AddRedis();

builder.Services.AddDataAccessLaoyer(builder.Configuration);

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.File("log.txt")
    .WriteTo.Seq("http://localhost:5109"));

builder.Services.AddApplication();

var app = builder.Build();
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FonTech Swagger v 1.0");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "FonTech Swagger v 2.0");
        c.RoutePrefix = string.Empty;
    });
}
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
