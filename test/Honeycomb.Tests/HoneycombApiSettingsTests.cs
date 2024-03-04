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
        [InlineData("hcaic_1234567890123456789012345678901234567890123456789012345678", "", "")]
        [InlineData("c1a551c000d68f9ed1e96432ac1a3380", "my-service", "my-service")]
        [InlineData("hcaic_1234567890123456789012345678901234567890123456789012345678", "my-service", "my-service")]
        [InlineData("c1a551c000d68f9ed1e96432ac1a3380", " my-service ", " my-service ")]
        [InlineData("hcaic_1234567890123456789012345678901234567890123456789012345678", " my-service ", " my-service ")]
        // defaults to unknown_dataset and trims dataset if set
        [InlineData("d68f9ed1e96432ac1a3380", "", "unknown_dataset")]
        [InlineData("hcxik_01hqk4k20cjeh63wca8vva5stw70nft6m5n8wr8f5mjx3762s8269j50wc", "", "unknown_dataset")]
        [InlineData("d68f9ed1e96432ac1a3380", "my-service", "my-service")]
        [InlineData("hcxik_01hqk4k20cjeh63wca8vva5stw70nft6m5n8wr8f5mjx3762s8269j50wc", "my-service", "my-service")]
        [InlineData("d68f9ed1e96432ac1a3380", " my-service ", "my-service")]
        [InlineData("hcxik_01hqk4k20cjeh63wca8vva5stw70nft6m5n8wr8f5mjx3762s8269j50wc", " my-service ", "my-service")]
        public void Dataset(string writekey, string dataset, string expected)
        {
            var settings = new HoneycombApiSettings
            {
                WriteKey = writekey,
                DefaultDataSet = dataset
            };
            settings.DefaultDataSet.ShouldBe(expected);
        }


        [Theory]
        [InlineData("", true)]
        [InlineData("12345678901234567890123456789012", true)]
        [InlineData("hcaic_1234567890123456789012345678901234567890123456789012345678", true)]
        [InlineData("kgvSpPwegJshQkuowXReLD", false)]
        [InlineData("hcaic_12345678901234567890123456", false)]
        [InlineData("hcxik_01hqk4k20cjeh63wca8vva5stw70nft6m5n8wr8f5mjx3762s8269j50wc", false)]
        public void IsClassic(string key, bool expected)
        {
            var settings = new HoneycombApiSettings
            {
                WriteKey = key,
                DefaultDataSet = "my-dataset"
            };
            settings.IsClassic().ShouldBe(expected);
        }
   }
}
