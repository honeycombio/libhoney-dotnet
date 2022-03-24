using System;

namespace Honeycomb.Models
{
    public class HoneycombApiSettings
    {
        private string _dataset;

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
                if (!string.IsNullOrWhiteSpace(_dataset) || IsClassic)
                {
                    return _dataset;
                }
                return "unknown_service";
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

        internal bool IsClassic => string.IsNullOrWhiteSpace(WriteKey) || WriteKey.Length == 32;
    }
}
