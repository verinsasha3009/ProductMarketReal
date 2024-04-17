using ProductMarket.Domain.Enum.Errors;
using ProductMarket.Domain.Result;

namespace ProductMarket.Presentation.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task HandleExceptionAsync(HttpContext httpContext,Exception exception)
        {
            var errorMessage = exception.Message;
            var responce = exception switch
            {
                UnauthorizedAccessException _ => new BaseResult()
                { ErrorMessage = errorMessage, ErrorCode = (int)ErrorCode.UserUnauthorizedAccess },
                _ => new BaseResult()
                { ErrorMessage = "Internal Server Error. Please retry later", ErrorCode = (int)ErrorCode.InternalServerError },
            };
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = responce.ErrorCode;
            await httpContext.Response.WriteAsJsonAsync(responce);
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
    }
}
