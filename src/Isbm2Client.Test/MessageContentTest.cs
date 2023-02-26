using Isbm2Client.Model;
using System.Text.Json;

namespace Isbm2Client.Test
{
    public class MessageContentTest
    {
        private readonly TestObject complexObject = new()
        {
            Numbers = new[] { 1, 2, 3, 4 },
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
            var messageString = new MessageString("fred");

            var content = messageString.GetContent<string>();

            Assert.True( content == "fred");
        }

        [Fact]
        public void JsonDocumentContent()
        {
            var inputDocument = JsonSerializer.SerializeToDocument( complexObject );
            var messageContent = new MessageJsonDocument(inputDocument);

            var document = messageContent.GetContent<JsonDocument>();

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
            var document = JsonSerializer.SerializeToDocument(complexObject);
            var messageContent = new MessageJsonDocument(document);

            var content = messageContent.Deserialise<TestObject>();

            Assert.True(content.Weather["Devonport"] == 12.6);
        }

        [Fact]
        public void ComplexContentTheWrongWay()
        {
            var document = JsonSerializer.SerializeToDocument(complexObject);
            var messageContent = new MessageJsonDocument(document);

            void wrongBet() => messageContent.GetContent<TestObject>();

            Assert.Throws<ArgumentException>(wrongBet);
        }

        [Fact]
        public void InvalidCast()
        {
            var document = JsonSerializer.SerializeToDocument(complexObject);
            MessageJsonDocument messageContent = new(document);

            void badCast() => messageContent.GetContent<string>();

            Assert.Throws<InvalidCastException>(badCast);
        }
    }
}
