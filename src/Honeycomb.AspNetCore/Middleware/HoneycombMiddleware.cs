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
                stopwatch.Stop();
                ev.Data.Add("name", $"{context.GetRouteValue("controller")}#{context.GetRouteValue("action")}");
                ev.Data.Add("action", context.GetRouteValue("action"));
                ev.Data.Add("controller", context.GetRouteValue("controller"));
                ev.Data.Add("response.content_length", context.Response.ContentLength);
                ev.Data.Add("response.status_code", context.Response.StatusCode);
                ev.Data.Add("duration_ms", stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                ev.Data.Add("request.error", ex.Source);
                ev.Data.Add("request.error_detail", ex.Message);
                throw;
            }
            finally
            {
                _service.QueueEvent(ev);
            }
        }
    }
}
