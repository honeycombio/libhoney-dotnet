namespace Honeycomb.Models
{
    public class HoneycombApiSettings
    {
        /// <summary>
        /// The TeamId within Honeycomb.
        /// </summary>
        /// <value></value>
        public string TeamId { get; set; }

        /// <summary>
        /// The default dataset to apply to each event.
        /// </summary>
        /// <value></value>
        public string DefaultDataSet { get; set; }

        /// <summary>
        /// The default size of each push to the API.
        /// </summary>
        /// <value></value>
        public int BatchSize { get; set; } = 100;

        /// <summary>
        /// The time between sending event batches.
        /// </summary>
        /// <value></value>
        public int SendFrequency { get; set; } = 60;

        /// <summary>
        /// The base URL to send event data to
        /// </summary>
        public string ApiHost { get; set; } = "https://api.honeycomb.io";
    }
}
