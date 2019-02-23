using Honeycomb.AspNetCore.Hosting;
using Honeycomb.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Honeycomb.AspNetCore.Middleware
{
    public static class HoneycombMiddlewareExtensions
    {

        public static IServiceCollection AddHoneycomb(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<HoneycombApiSettings>(o => configuration.GetSection("HoneycombSettings").Bind(o));
            return AddHoneycomb(serviceCollection);
        }

        public static IServiceCollection AddHoneycomb(this IServiceCollection serviceCollection, HoneycombApiSettings honeycombApiSettings)
        {
            serviceCollection.Configure<HoneycombApiSettings>(o => o = honeycombApiSettings);
            return AddHoneycomb(serviceCollection);
        }

        public static IServiceCollection AddHoneycomb(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient("honeycomb");
            serviceCollection.AddHttpContextAccessor();
            serviceCollection.TryAddSingleton<IHoneycombService, HoneycombService>();
            serviceCollection.TryAddSingleton<IHoneycombEventManager, HoneycombEventManager>();
            serviceCollection.AddHostedService<HoneycombBackgroundService>();
            return serviceCollection;
        }

        public static IApplicationBuilder UseHoneycomb(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<HoneycombMiddleware>();
        }
}
}