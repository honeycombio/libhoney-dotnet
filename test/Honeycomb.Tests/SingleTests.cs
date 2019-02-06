using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Honeycomb.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using Shouldly;
using System.Linq;

namespace Honeycomb.Tests
{
    public class SingleTests
    {
        private readonly HoneycombApiSettings _settings;
        private readonly Mock<IHttpClientFactory> _factory;
        private readonly MyMessageHandler _handler;
        private readonly HoneycombService _honeycombService;

        public SingleTests()
        {
            _settings = new HoneycombApiSettings {
                BatchSize = 1
            };
            _factory = new Mock<IHttpClientFactory>();

            _handler = new MyMessageHandler();

            var client = new HttpClient(_handler);
            _factory.Setup(f => f.CreateClient(It.IsAny<string>()))
                .Returns(client);

            var logger = new Mock<ILogger<HoneycombService>>();
            var options = new Mock<IOptions<HoneycombApiSettings>>();
            options.Setup(o => o.Value)
                .Returns(_settings);
            _honeycombService = new HoneycombService(_factory.Object, logger.Object, options.Object);
        }

        [Fact]
        public async Task Simple()
        {
            _handler.ResponseMessages.Enqueue(new HttpResponseMessage {
                Content = new StringContent("")
            });

            await _honeycombService.SendSingleAsync(new HoneycombEvent {
                Data = new Dictionary<string, object> {
                    { "test", 2 }
                },
                EventTime = new DateTime(2010, 10,10),
                DataSetName = "blah"
            });

            _handler.Messages.Count().ShouldBe(1);
            var message = _handler.Messages[0];
            message.RequestUri.AbsoluteUri.ShouldBe("https://api.honeycomb.io/1/events/blah");
        }
    }

    public class MyMessageHandler : HttpMessageHandler
    {
        public List<HttpRequestMessage> Messages { get; set; } = new List<HttpRequestMessage>();
        public Queue<HttpResponseMessage> ResponseMessages { get; set; } = new Queue<HttpResponseMessage>();

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Messages.Add(request);
            return Task.FromResult(ResponseMessages.Dequeue());
        }
    }
}
