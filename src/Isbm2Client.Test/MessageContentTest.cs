using Isbm2Client.Extensions;
using Isbm2Client.Model;
using System.Text.Json;
using System.Text.Json.Nodes;

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

        private readonly string id = Guid.NewGuid().ToString();

        [Fact]
        public void StringContent()
        {
            var messageString = new MessageString( id, "fred");

            var content = messageString.GetContent<string>();

            Assert.True( content == "fred");
        }

        [Fact]
        public void JsonDocumentContent()
        {
            var inputDocument = JsonSerializer.SerializeToDocument( complexObject );
            var messageContent = new MessageJsonDocument(id, inputDocument);

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
            var messageContent = new MessageJsonDocument(id, document);

            var content = messageContent.Deserialise<TestObject>();

            Assert.True(content.Weather["Devonport"] == 12.6);
        }

        [Fact]
        public void ComplexContentTheWrongWay()
        {
            var document = JsonSerializer.SerializeToDocument(complexObject);
            var messageContent = new MessageJsonDocument(id, document);

            void wrongBet() => messageContent.GetContent<TestObject>();

            Assert.Throws<ArgumentException>(wrongBet);
        }

        [Fact]
        public void InvalidCast()
        {
            var document = JsonSerializer.SerializeToDocument(complexObject);
            MessageJsonDocument messageContent = new(id, document);

            void badCast() => messageContent.GetContent<string>();

            Assert.Throws<InvalidCastException>(badCast);
        }
    }
}
