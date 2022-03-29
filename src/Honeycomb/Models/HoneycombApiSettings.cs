using System;
using Microsoft.Extensions.Logging;

namespace Honeycomb.Models
{
    public class HoneycombApiSettings
    {
        private const string DefaultDatasetValue = "unknown_dataset";

        private readonly ILogger<HoneycombApiSettings> _logger;
        private string _dataset;

        public HoneycombApiSettings()
            : this(LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<HoneycombApiSettings>())
        { }

        public HoneycombApiSettings(ILogger<HoneycombApiSettings> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// The TeamId within Honeycomb.
        /// </summary>
        /// <value></value>
        [ObsoleteAttribute("This property is obsolete. Use WriteKey instead.", false)]
        public string TeamId
        {
            get { return WriteKey; }
            set { WriteKey = value; }
        }

        /// <summary>
        /// The WriteKey within Honeycomb.
        /// </summary>
        /// <value></value>
        public string WriteKey { get; set; }

        /// <summary>
        /// The default dataset to apply to each event.
        /// </summary>
        /// <value></value>
        public string DefaultDataSet
        {
            get
            {
                if (IsClassic) {
                    return _dataset;
                }
                if (string.IsNullOrWhiteSpace(_dataset))
                {
                    _logger.LogWarning($"WARN: Dataset is empty or only whitespace, using default: '{DefaultDatasetValue}'");
                    return DefaultDatasetValue;
                }
                var trimmed = _dataset.Trim();
                if (_dataset != trimmed)
                {
                    _logger.LogWarning($"WARN: Dataset has unexpected whitespace, using trimmed version: '{trimmed}'");
                }
                return trimmed;
            }
            set { _dataset = value; }
        }

        /// <summary>
        /// The default size of each push to the API.
        /// </summary>
        /// <value></value>
        public int BatchSize { get; set; } = 100;

        /// <summary>
        /// The time between sending event batches. Expressed in milliseconds.
        /// </summary>
        /// <value></value>
        public int SendFrequency { get; set; } = 10000;

        /// <summary>
        /// The base URL to send event data to
        /// </summary>
        public string ApiHost { get; set; } = "https://api.honeycomb.io";

        private bool IsClassic => string.IsNullOrWhiteSpace(WriteKey) || WriteKey.Length == 32;
    }
}
