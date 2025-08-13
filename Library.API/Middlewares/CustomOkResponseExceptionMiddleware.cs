using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Library.API.Middlewares
{
    public class CustomOkErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomOkErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                
                if (context.Request.Path.StartsWithSegments("/api/user") ||
                    context.Request.Path.StartsWithSegments("/api/bookrental"))
                {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    context.Response.ContentType = "application/json";

                    var errorResponse = new
                    {
                        success = false,
                        message = ex.Message
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                }
                else
                {
                    throw; 
                }
            }
        }
    }
}
