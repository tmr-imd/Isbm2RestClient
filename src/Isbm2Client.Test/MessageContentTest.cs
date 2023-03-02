using Isbm2Client.Model;
using System.Text.Json;

namespace Isbm2Client.Test
{
    public class MessageContentTest
    {
        private readonly TestObject complexObject = new()
        {
            Numbers = new[] { 1.0, 2, 3, 4 },
            Text = "Fred",
            Weather = new Dictionary<string, double>()
            {
                {"Devonport", 12.6},
                {"Launceston", 25}
            }
        };

        [Fact]
        public void StringContent()
        {
            var messageString = MessageContent.From("fred");

            var content = messageString.Deserialise<string>();

            Assert.True( content == "fred");
        }

        [Fact]
        public void JsonDocumentContent()
        {
            var inputDocument = JsonSerializer.SerializeToDocument( complexObject );
            var messageContent = new MessageContent(inputDocument, "application/json");

            var document = messageContent.Content;

            Assert.NotNull( document );

            if ( document is not null )
            {
                var weather = document.RootElement.GetProperty("Weather");

                Assert.True(weather.GetProperty("Devonport").GetDouble() == 12.6);
            }
        }

        [Fact]
        public void ComplexContent()
        {
            var messageContent = MessageContent.From(complexObject);

            var content = messageContent.Deserialise<TestObject>();

            Assert.True(content.Weather["Devonport"] == 12.6);
        }

        [Fact]
        public void InvalidCast()
        {
            var messageContent = MessageContent.From(complexObject);

            void badCast() => messageContent.Deserialise<string>();

            Assert.Throws<InvalidCastException>(badCast);
        }
    }
}
