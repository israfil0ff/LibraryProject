using Library.BLL.Exceptions;
using Library.Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        if (exception is AppException appEx)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return context.Response.WriteAsJsonAsync(new
            {
                errorCode = (int)appEx.Code,
                message = appEx.Message
            });
        }

        // Digər xətalar üçün generic
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return context.Response.WriteAsJsonAsync(new
        {
            errorCode = 9999,
            message = "Serverdə gözlənilməz xəta baş verdi."
        });
    }
}
