using System.Diagnostics;
using System.Threading.Tasks;
using Honeycomb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Honeycomb.AspNetCore.Middleware
{
    public class HoneycombMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HoneycombMiddleware> _logger;
        private readonly IHoneycombService _service;
        private readonly IOptions<HoneycombApiSettings> _settings;
        public HoneycombMiddleware(RequestDelegate next, 
            IHoneycombService service,
            IOptions<HoneycombApiSettings> settings,
            ILogger<HoneycombMiddleware> logger)
        {
            _next = next;
            _service = service;
            _settings = settings;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            var ev = new HoneycombEvent
            {
                DataSetName = _settings.Value.DefaultDataSet
            };
            context.Items.Add(HoneycombEventManager.ContextItemName, ev);

            ev.Data.Add("path", context.Request.Path.Value);
            ev.Data.Add("method", context.Request.Method);
            ev.Data.Add("protocol", context.Request.Protocol);
            ev.Data.Add("req_size", context.Request.ContentLength);
            ev.Data.Add("scheme", context.Request.Scheme);

            await _next.Invoke(context);
            
            ev.Data.TryAdd("action", context.GetRouteValue("action"));
            ev.Data.TryAdd("controller", context.GetRouteValue("controller"));
            ev.Data.TryAdd("resp_size", context.Response.ContentLength);
            ev.Data.TryAdd("req_ms", stopwatch.ElapsedMilliseconds);

            _service.QueueEvent(ev);
        }
}
}