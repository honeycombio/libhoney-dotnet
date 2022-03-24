using System;
using Xunit;
using Shouldly;

namespace Honeycomb.Models
{
    public class HoneycombApiSettingsTests
    {
        private const string ClassicWriteKey = "c1a551c000d68f9ed1e96432ac1a3380";
        private const string ModernWriteKey = "d68f9ed1e96432ac1a3380";

        [Theory]
        [InlineData(null, "", "")]
        [InlineData(null, "my-service", "my-service")]
        [InlineData(null, " my-service ", " my-service ")]
        [InlineData("", "", "")]
        [InlineData("", "my-service", "my-service")]
        [InlineData("", " my-service ", " my-service ")]
        [InlineData(ClassicWriteKey, "", "")]
        [InlineData(ClassicWriteKey, "my-service", "my-service")]
        [InlineData(ClassicWriteKey, " my-service ", " my-service ")]
        [InlineData(ModernWriteKey, "", "unknown_service")]
        [InlineData(ModernWriteKey, "my-service", "my-service")]
        [InlineData(ModernWriteKey, " my-service ", "my-service")]
        public void Dataset(string writekey, string dataset, string expected)
        {
            var settings = new HoneycombApiSettings
            {
                WriteKey = writekey,
                DefaultDataSet = dataset
            };
            settings.DefaultDataSet.ShouldBe(expected);
        }

        // [Theory]
        // [InlineData("", "")]
        // [InlineData("my-service", "my-service")]
        // [InlineData(" my-service ", " my-service ")]
        // public void NoWriteKey(string dataset, string expected)
        // {
        //     var settings = new HoneycombApiSettings
        //     {
        //         WriteKey = "",
        //         DefaultDataSet = dataset
        //     };
        //     settings.DefaultDataSet.ShouldBe(expected);
        // }

        // [Theory]
        // [InlineData("", "")]
        // [InlineData("my-service", "my-service")]
        // [InlineData(" my-service ", " my-service ")]
        // public void ClassicWriteKey(string dataset, string expected)
        // {
        //     var settings = new HoneycombApiSettings
        //     {
        //         WriteKey = "c1a551c000d68f9ed1e96432ac1a3380",
        //         DefaultDataSet = dataset
        //     };
        //     settings.DefaultDataSet.ShouldBe(expected);
        // }

        // [Theory]
        // [InlineData("", "unknown_service")]
        // [InlineData("my-service", "my-service")]
        // [InlineData("my-service ", "my-service")]
        // public void ModernWriteKey(string dataset, string expected)
        // {
        //     var settings = new HoneycombApiSettings
        //     {
        //         WriteKey = "d68f9ed1e96432ac1a3380",
        //         DefaultDataSet = dataset
        //     };
        //     settings.DefaultDataSet.ShouldBe(expected);
        // }
    }
}