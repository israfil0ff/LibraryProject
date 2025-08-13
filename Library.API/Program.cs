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


AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
{
    Console.WriteLine("UNHANDLED EXCEPTION:");
    Console.WriteLine(e.ExceptionObject.ToString());
};


var columnOptions = new ColumnOptions
{
    AdditionalColumns = new Collection<SqlColumn>
    {
        new SqlColumn { ColumnName = "UserName", DataType = System.Data.SqlDbType.NVarChar, DataLength = 100 }
    }
};


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


builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    })
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AuthorCreateDto>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAutoMapper(typeof(MappingProfile));


builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookRentalService, BookRentalService>();
builder.Services.AddScoped<ICategoryService, CategoryService>(); // Yeni Category Service


builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseMiddleware<SimpleRequestLoggingMiddleware>();
app.UseMiddleware<Library.API.Middlewares.CustomOkErrorMiddleware>();
app.UseAuthorization();


app.MapControllers();


app.Run();
