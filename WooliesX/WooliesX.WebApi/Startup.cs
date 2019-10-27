using WooliesX.Common.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WooliesX.Common.ExternalResources;
using WooliesX.WebApi.Configuration;
using WooliesX.WebApi.Services;

namespace WooliesX.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .Configure<UserProfile>(Configuration.GetSection("UserProfile"))
                .AddExternalResourcesApi()
                .AddCoreServices()
                .AddSwagger()
                .AddMvc(options => options.RespectBrowserAcceptHeader = true)
                .AddJsonOptions(
                    options =>
                    {
                        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app
                .UseHttpsRedirection()
                .UseMvc()
                .UseSwagger()
                .UseSwaggerUi();
        }
    }
}
