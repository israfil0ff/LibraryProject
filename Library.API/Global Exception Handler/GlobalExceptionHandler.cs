using Library.BLL.Exceptions;
using Library.Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
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

        // ✅ Enum-based custom exception
        if (exception is AppException appEx)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return context.Response.WriteAsJsonAsync(new
            {
                errorCode = (int)appEx.Code,
                message = appEx.Message
            });
        }

        // ✅ ArgumentException → 400 BadRequest
        if (exception is ArgumentException argEx)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return context.Response.WriteAsJsonAsync(new
            {
                message = argEx.Message
            });
        }

        // ✅ InvalidOperationException → 404 NotFound
        if (exception is InvalidOperationException invOpEx)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            return context.Response.WriteAsJsonAsync(new
            {
                message = invOpEx.Message
            });
        }

        // ✅ Digər xətalar üçün generic 500
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return context.Response.WriteAsJsonAsync(new
        {
            errorCode = 9999,
            message = "Serverdə gözlənilməz xəta baş verdi."
        });
    }
}
