
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.Api.Errors;
using Talabat.Api.Helpers;
using Talabat.Api.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Core.IRepository;
using Talabat.Core.Specifications;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Data.Identity;

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
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"));
            });
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            //InValidErrorResponse
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (ActionContext) =>
                {
                    var errors = ActionContext.ModelState.Where(E => E.Value.Errors.Count() > 0)
                                                            .SelectMany(E => E.Value.Errors)
                                                            .Select(E => E.ErrorMessage)
                                                            .ToArray();
                    var invalidErrorResponse = new InvalidErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(invalidErrorResponse);
                };
            });
            var app = builder.Build();
            var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var _storeContext = serviceProvider.GetRequiredService<StoreContext>();
            var _identityContext = serviceProvider.GetRequiredService<AppIdentityDbContext>();
            var _userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            try
            {
                await _storeContext.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(_storeContext);
                await _identityContext.Database.MigrateAsync();
                await AppIdentityDbContextSeed.UserDataSeedAsync(_userManager);
            }catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex,ex.Message);
            }

            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionErrorMiddleware>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStatusCodePagesWithRedirects("/errors/{0}");
            app.UseStaticFiles();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}