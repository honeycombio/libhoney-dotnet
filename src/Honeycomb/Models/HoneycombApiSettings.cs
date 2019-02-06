namespace Honeycomb.Models
{
    public class HoneycombApiSettings
    {
        public string TeamId { get; set; }
        public string DefaultDataSet { get; set; }
        public int BatchSize { get; set; }
        public int SendFrequency { get; set; }
    }
}