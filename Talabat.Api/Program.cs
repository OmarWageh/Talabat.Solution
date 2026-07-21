 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Text.Json;
using Talabat.Api.CustomMiddleware;
using Talabat.Api.Errors;
using Talabat.Api.Helper;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Repository.Repository;
using Talabat.Repository.Repository.OrderRepository;
using Talabat.Repository.Repository.ProductRepositry;
using Talabat.Service;

namespace Talabat.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Do Register to AppDbContext to create database (ConnectionString)
            builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            #endregion
            #region Do Register to AppIdentityDbContext to create database Securty
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));
            #endregion
            #region Do Register IConnectionMultiplexer to create Memorydatabase (ConnectionString)
            builder.Services.AddSingleton<IConnectionMultiplexer>((ServiceProvider) =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });
                 builder.Services.AddIdentity<AppUser, IdentityRole>()
                 .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddSingleton<IBasketRepository, BasketRepository>();



            #endregion
            #region Do register to (SpacificProduct,unitofwork,SpacificOrder,GenericRepository,OrderService,AuthService,AutoMapper)
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped(typeof(IOrderService), typeof(OrderService));
            builder.Services.AddAutoMapper(typeof(MappingProfiles));
            #endregion
            builder.Services.AddControllers()
            #region  This code changes the default validation error response format in ASP.NET Core,It makes the validation errors return in the same format defined in the ApiResponseToValidationError class,It also collects all validation error messages into a single Errors[] array.
        .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = actionContext =>
        {
            var errors = actionContext.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage)
                .ToArray();

            var errorResponse = new ApiResponseToValidationError()
            {
                Errors = errors
            };

            return new BadRequestObjectResult(errorResponse);
        };
    });
            #endregion
            #region DoUpdatedatabase & Send Data in DataBase From JsonFile
            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _dbContext = services.GetRequiredService<AppDbContext>();
            var Identity= services.GetRequiredService<AppIdentityDbContext>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _dbContext.Database.MigrateAsync();//update to Migration Entity database automatic
                await Identity.Database.MigrateAsync();//update to Migration IdentitySecutry database automatic
                await StoreDataInDataBase.SeedingDataAsync(_dbContext);//send data in database from file 

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error has been occured during apply the migration ");

            }
            #endregion
            #region this custom middleware to handle servererror
            app.UseMiddleware<HandleTheServerErrorMiddleware>();
            #endregion
            #region Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }  
            #endregion
            #region this request Delegate middleware to handle notfoundendpoint
            app.Use(async (context, next) =>
               {
                   await next();

                   if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
                   {
                       context.Response.ContentType = "application/json";

                       var response = new ApiResponseToNotFound_Badrequest_Unauthorized(404, "Endpoint not found");

                       var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                       var json = JsonSerializer.Serialize(response, options);

                       await context.Response.WriteAsync(json);
                   }
               });
            #endregion
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapControllers();
            app.UseAuthentication();
            app.UseAuthorization();

            app.Run();
        }
     
    }
  
}
