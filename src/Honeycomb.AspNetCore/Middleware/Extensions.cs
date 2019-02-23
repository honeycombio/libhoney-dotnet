using Honeycomb.AspNetCore.Hosting;
using Honeycomb.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Honeycomb.AspNetCore.Middleware
{
    public static class HoneycombMiddlewareExtensions
    {
        public static IServiceCollection AddHoneycomb(this IServiceCollection serviceCollection, HoneycombApiSettings settings)
        {
            serviceCollection.AddHttpClient("honeycomb", client =>
            {
                client.DefaultRequestHeaders.Add("X-Honeycomb-Team", settings.TeamId);
            });
            serviceCollection.TryAddSingleton<HoneycombApiSettings>(settings);
            serviceCollection.TryAddSingleton<IHoneycombService, HoneycombService>();
            serviceCollection.TryAddSingleton<IHoneycombEventManager, HoneycombEventManager>();
            serviceCollection.TryAddSingleton<HoneycombBackgroundService>();
            return serviceCollection;
        }

        public static IApplicationBuilder UseHoneycomb(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<HoneycombMiddleware>();
        }
}
}