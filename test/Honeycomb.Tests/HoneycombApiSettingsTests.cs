using System;
using Xunit;
using Shouldly;

namespace Honeycomb.Models
{
    public class HoneycombApiSettingsTests
    {
        [Theory]
        // defaults to empty string and does not trim dataset if set
        [InlineData(null, "", "")]
        [InlineData(null, "my-service", "my-service")]
        [InlineData(null, " my-service ", " my-service ")]
        [InlineData("", "", "")]
        [InlineData("", "my-service", "my-service")]
        [InlineData("", " my-service ", " my-service ")]
        [InlineData("c1a551c000d68f9ed1e96432ac1a3380", "", "")]
        [InlineData("c1a551c000d68f9ed1e96432ac1a3380", "my-service", "my-service")]
        [InlineData("c1a551c000d68f9ed1e96432ac1a3380", " my-service ", " my-service ")]
        // defaults to unknown_service and trims dataset if set
        [InlineData("d68f9ed1e96432ac1a3380", "", "unknown_service")]
        [InlineData("d68f9ed1e96432ac1a3380", "my-service", "my-service")]
        [InlineData("d68f9ed1e96432ac1a3380", " my-service ", "my-service")]
        public void Dataset(string writekey, string dataset, string expected)
        {
            var settings = new HoneycombApiSettings
            {
                WriteKey = writekey,
                DefaultDataSet = dataset
            };
            settings.DefaultDataSet.ShouldBe(expected);
        }
   }
}