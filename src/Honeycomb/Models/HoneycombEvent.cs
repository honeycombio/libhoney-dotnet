using System;
using System.Collections.Generic;

namespace Honeycomb.Models
{
    public class HoneycombEvent
    {
        public double SampleRate { get; set; }
        public string DataSetName { get; set; }
        public DateTime EventTime { get; set; } = DateTime.UtcNow;
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
    }
}