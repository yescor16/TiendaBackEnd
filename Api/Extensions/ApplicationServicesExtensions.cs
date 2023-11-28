using Api.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Api.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<StoreContext>();

           

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext => {
                    var errorList = actionContext.ModelState.Where(e => e.Value.Errors.Count > 0).SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errorList
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });
          

            services.AddSingleton<IConnectionMultiplexer>(c => {
                var configuration = ConfigurationOptions.Parse(config.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });
            services.AddScoped<IBasketRepository, BasketRepository>();


            return services;



        }
    }
}
