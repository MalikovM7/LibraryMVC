using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryMVC.Data;
using LibraryMVC.FluentValidation;
using LibraryMVC.Services.Implementation;
using LibraryMVC.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibraryMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder); 

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }



        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            // Connection string
            var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection")
                ?? throw new InvalidOperationException("Connection string 'AppDbContextConnection' not found.");

            // Database context
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            

            
            // FluentValidation
            builder.Services.AddFluentValidationAutoValidation(options => options.DisableDataAnnotationsValidation = true);
            builder.Services.AddValidatorsFromAssemblyContaining<BookValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<ReaderValidator>();

            // Add services 
            RegisterServices(builder.Services);
            

            // Add controllers with views
            builder.Services.AddControllersWithViews();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IReaderService, ReaderService>();
            services.AddScoped<IBorrowService, BorrowService>();
        }
    }
}
