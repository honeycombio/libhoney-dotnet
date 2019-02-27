using System;
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
            ev.Data.Add("trace.trace_id", context.TraceIdentifier);
            ev.Data.Add("request.path", context.Request.Path.Value);
            ev.Data.Add("request.method", context.Request.Method);
            ev.Data.Add("request.http_version", context.Request.Protocol);
            ev.Data.Add("request.content_length", context.Request.ContentLength);
            ev.Data.Add("request.header.x_forwarded_proto", context.Request.Scheme);
            ev.Data.Add("meta.local_hostname", Environment.MachineName);

            try
            {
                await _next.Invoke(context);
            
            	ev.Data.TryAdd("name", $"{context.GetRouteValue("controller")}#{context.GetRouteValue("action")}");
                ev.Data.TryAdd("action", context.GetRouteValue("action"));
                ev.Data.TryAdd("controller", context.GetRouteValue("controller"));
            	ev.Data.TryAdd("response.content_length", context.Response.ContentLength);
            	ev.Data.TryAdd("response.status_code", context.Response.StatusCode);
            	ev.Data.TryAdd("duration_ms", stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                ev.Data.TryAdd("request.error", ex.Source);
                ev.Data.TryAdd("request.error_detail", ex.Message);
                throw;
            }
            finally
            {
                _service.QueueEvent(ev);
            }
        }
}
}