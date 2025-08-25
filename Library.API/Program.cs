using FluentValidation.AspNetCore;
using Library.BLL;
using Library.BLL.Interfaces;
using Library.DAL.Context;
using Library.DBO;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Filters;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

// 🔹 Global unhandled exception logger
AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
{
    Console.WriteLine("UNHANDLED EXCEPTION:");
    Console.WriteLine(e.ExceptionObject.ToString());
};

// 🔹 Serilog SQL Column Options
var columnOptions = new ColumnOptions
{
    AdditionalColumns = new Collection<SqlColumn>
    {
        new SqlColumn { ColumnName = "UserName", DataType = System.Data.SqlDbType.NVarChar, DataLength = 100 }
    }
};

// 🔹 Serilog configuration
Log.Logger = new LoggerConfiguration()
    .Filter.ByIncludingOnly(Matching.WithProperty<string>("SourceContext", sc => sc == "SimpleRequestLoggingMiddleware"))
    .WriteTo.MSSqlServer(
        connectionString: "Server=ITMVOL112;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True;",
        sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
        columnOptions: columnOptions
    )
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// 🔹 Services registration
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// 🔹 Middleware pipeline
ConfigureMiddleware(app);

app.Run();

// ==========================
// 🔽 Helpers (Extension Style)
// ==========================
static void ConfigureServices(IServiceCollection services, IConfiguration config)
{
    // Controllers + FluentValidation
    services.AddControllers()
        .AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AuthorCreateDto>());

    // Swagger
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    // AutoMapper
    services.AddAutoMapper(typeof(MappingProfile));

    // DbContext
    services.AddDbContext<LibraryDbContext>(options =>
        options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

    // Services
    services.AddScoped<IAuthorService, AuthorService>();
    services.AddScoped<IBookService, BookService>();
    services.AddScoped<IBookRentalService, BookRentalService>();
    services.AddScoped<ICategoryService, CategoryService>();

    // 🔹 Feedback servisini qeydiyyata al
    services.AddScoped<IFeedbackService, FeedbackService>();

    // ProblemDetails
    services.AddProblemDetails();

    // Rate Limiting
    services.AddRateLimiter(options =>
    {
        options.AddFixedWindowLimiter("fixed", opt =>
        {
            opt.PermitLimit = 10;
            opt.Window = TimeSpan.FromSeconds(30);
            opt.QueueLimit = 0;
        });

        options.AddPolicy("per-token", httpContext =>
        {
            var key = GetTokenOrIp(httpContext);
            return RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: key,
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 5,
                    Window = TimeSpan.FromMinutes(5),
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                });
        });

        options.OnRejected = async (context, token) =>
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.HttpContext.Response.WriteAsync(
                "Too many requests. Please try again later.", token);
        };
    });
}

static void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // 🔹 Global Exception Middleware
    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

    app.UseHttpsRedirection();
    app.UseRateLimiter();

    app.UseMiddleware<SimpleRequestLoggingMiddleware>();
    app.UseMiddleware<Library.API.Middlewares.CustomOkErrorMiddleware>();

    app.UseAuthorization();

    // Custom Author Route
    app.MapControllerRoute(
        name: "author",
        pattern: "api/author/{action=Get}/{id?}",
        defaults: new { controller = "Author" }
    ).RequireRateLimiting("per-token");

    app.MapControllers();
}

// 🔹 Token və ya IP helper
static string GetTokenOrIp(HttpContext ctx)
{
    if (ctx.Request.Headers.TryGetValue("Authorization", out var auth) &&
        auth.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
    {
        var token = auth.ToString().Substring("Bearer ".Length).Trim();
        if (!string.IsNullOrWhiteSpace(token))
            return "t:" + token;
    }

    return "ip:" + (ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown");
}
