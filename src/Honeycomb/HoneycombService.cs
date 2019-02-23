using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Honeycomb.Models;
using Honeycomb.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;

namespace Honeycomb
{
    public class HoneycombService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HoneycombService> _logger;
        private readonly IOptions<HoneycombApiSettings> _settings;
        private readonly ConcurrentQueue<HoneycombEvent> events = new ConcurrentQueue<HoneycombEvent>();

        public HoneycombService(IHttpClientFactory httpClientFactory,
            ILogger<HoneycombService> logger,
            IOptions<HoneycombApiSettings> settings)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _settings = settings;
        }

        public void QueueEvent(HoneycombEvent ev)
        {
            events.Enqueue(ev);
        }

        public async Task Flush()
        {
            while (true)
            {
                var dequeueSize = events.Count < _settings.Value.BatchSize ?
                    events.Count :
                    _settings.Value.BatchSize;

                var chunk = events
                    .DequeueChunk(dequeueSize)
                    .ToList();

                if (!chunk.Any())
                    break;

                await SendBatchAsync(chunk);
            }
        }

        public async Task SendSingleAsync(HoneycombEvent ev)
        {
            _logger.LogInformation("Sending Honeycomb Data");

            var client = _httpClientFactory.CreateClient("honeycomb");

            var message = new HttpRequestMessage();
            var content = JsonConvert.SerializeObject(ev.Data);
            message.Content = new StringContent(content, Encoding.UTF8, "application/json");
            message.Method = HttpMethod.Post;
            message.RequestUri = new Uri($"https://api.honeycomb.io/1/events/{ev.DataSetName}");
            message.Headers.Add("X-Honeycomb-Event-Time",
                ev.EventTime.ToUniversalTime().ToString(@"{0:yyyy-MM-ddTHH\:mm\:ss.fffK}"));

            var resp = await client.SendAsync(message);
            if (!resp.IsSuccessStatusCode)
                _logger.LogWarning("Error sending information to honeycomb status:{StatusCode}", resp.StatusCode);
        }

        public async Task SendBatchAsync(IEnumerable<HoneycombEvent> items)
        {
            _logger.LogInformation("Sending Honeycomb Data");

            foreach (var group in items.GroupBy(i => i.DataSetName))
            {
                await SendBatchAsync(group, group.Key);
            }
        }

        public async Task SendBatchAsync(IEnumerable<HoneycombEvent> items, string dataSetName)
        {
            _logger.LogInformation("Sending Honeycomb Data for {dataSetName} with {items} items", dataSetName, items.Count());

            var client = _httpClientFactory.CreateClient("honeycomb");

            var message = new HttpRequestMessage();

            var sendItems = items.Select(i => 
                new { 
                    time = i.EventTime.ToUniversalTime().ToString(@"{0:yyyy-MM-ddTHH\:mm\:ss.fffK}"),
                    data = i.Data
                    });

            var content = JsonConvert.SerializeObject(sendItems);
            message.Content = new StringContent(content, Encoding.UTF8, "application/json");
            message.Method = HttpMethod.Post;
            message.RequestUri = new Uri($"https://api.honeycomb.io/1/batch/{dataSetName}");

            var resp = await client.SendAsync(message);
            if (!resp.IsSuccessStatusCode)
                _logger.LogWarning("Error sending information to honeycomb status:{StatusCode}", resp.StatusCode);
        }
    }
}
