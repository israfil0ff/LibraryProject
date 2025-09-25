using Library.BLL.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Threading.Tasks;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
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

        // ✅ Development environment → həmişə real error mesajı
        if (_env.IsDevelopment())
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsJsonAsync(new
            {
                errorCode = 5000,
                message = exception.Message
            });
        }

        // ✅ Production-da File endpoint → real error mesajı
        if (context.Request.Path.StartsWithSegments("/api/file"))
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsJsonAsync(new
            {
                errorCode = 5000,
                message = exception.Message
            });
        }

        // ✅ AppException → 400 BadRequest
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
