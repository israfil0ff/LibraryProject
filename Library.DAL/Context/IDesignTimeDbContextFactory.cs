using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Library.DAL.Context
{
    public class LibraryDbContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
    {
        public LibraryDbContext CreateDbContext(string[] args)
        {
           
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), @"C:\Users\volunteer12\Desktop\Omer's folder\LibraryProject-main\Library.API");

            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

           
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            
            var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            
            return new LibraryDbContext(optionsBuilder.Options);
        }
    }
}
