using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace WooliesX.Common.ExternalResources
{
    public static class Configuration
    {
        public static IServiceCollection AddExternalResourcesApi(this IServiceCollection service)
        {
            return service
                .AddTransient<IExerciseResourceApi, ExerciseResourceApi>()
                .AddTransient(serviceProvider =>
                {
                    var httpClient = new HttpClient
                    {
                        BaseAddress = new Uri(ResourceUrl.BaseAddress),
                        Timeout = TimeSpan.FromSeconds(60),
                    };

                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    
                    return httpClient;
                });
        }
    }
}
