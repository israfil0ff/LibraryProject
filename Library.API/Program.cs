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

AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
{
    Console.WriteLine("UNHANDLED EXCEPTION:");
    Console.WriteLine(e.ExceptionObject.ToString());
};

// Serilog SQL Column Options
var columnOptions = new ColumnOptions
{
    AdditionalColumns = new Collection<SqlColumn>
    {
        new SqlColumn { ColumnName = "UserName", DataType = System.Data.SqlDbType.NVarChar, DataLength = 100 }
    }
};

// Serilog konfiqurasiyası
Log.Logger = new LoggerConfiguration()
    .Filter.ByIncludingOnly(Matching.WithProperty<string>(
        "SourceContext", sc => sc == "SimpleRequestLoggingMiddleware"))
    .WriteTo.MSSqlServer(
        connectionString: "Server=ITMVOL112;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True;",
        sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
        columnOptions: columnOptions
    )
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// Controllers + FluentValidation
builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    })
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AuthorCreateDto>());

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// DbContext
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookRentalService, BookRentalService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 10;               
        opt.Window = TimeSpan.FromSeconds(30); 
        opt.QueueLimit = 0;                  
    });

    
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync(
            "Too many requests. Please try again later.", token);
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();


app.UseRateLimiter();

// Custom Middlewares
app.UseMiddleware<SimpleRequestLoggingMiddleware>();
app.UseMiddleware<Library.API.Middlewares.CustomOkErrorMiddleware>();

app.UseAuthorization();


app.MapControllers().RequireRateLimiting("fixed");

app.Run();
