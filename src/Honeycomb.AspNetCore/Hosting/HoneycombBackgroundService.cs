using System.Threading;
using System.Threading.Tasks;
using Honeycomb.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Honeycomb.AspNetCore.Hosting
{
    public class HoneycombBackgroundService : BackgroundService
    {
        private readonly IHoneycombService _honeycombService;
        private readonly ILogger<BackgroundService> _logger;
        private readonly IOptions<HoneycombApiSettings> _settings;

        public HoneycombBackgroundService(IHoneycombService honeycombService, 
            ILogger<BackgroundService> logger,
            IOptions<HoneycombApiSettings> settings)
        {
            _honeycombService = honeycombService;
            _logger = logger;
            _settings = settings;
        }

        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await ExecuteOnceAsync(cancellationToken);

                _logger.LogTrace($"Delaying for {_settings.Value.SendFrequency}");
                await Task.Delay(_settings.Value.SendFrequency, cancellationToken);
            }
        }

        private async Task ExecuteOnceAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace($"Starting Flush");
            await _honeycombService.Flush();
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace($"Shutting down BackgroundService for Honeycomb");
            await _honeycombService.Flush();
            await base.StopAsync(cancellationToken);
        }
    }
}