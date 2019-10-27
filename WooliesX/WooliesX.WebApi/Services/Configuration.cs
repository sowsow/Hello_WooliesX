using Microsoft.Extensions.DependencyInjection;

namespace WooliesX.WebApi.Services
{
    public static class Configuration
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IProductService, ProductService>()
                .AddTransient<ITrolleyService, TrolleyService>();
        }
    }
}
