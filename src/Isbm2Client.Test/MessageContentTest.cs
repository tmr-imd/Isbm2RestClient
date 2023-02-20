using Isbm2Client.Extensions;
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

        private readonly string id = Guid.NewGuid().ToString();

        [Fact]
        public void StringContent()
        {
            var messageString = new MessageString( id, "fred");

            var content = messageString.GetContent<string>();

            Assert.True( content == "fred");
        }

        [Fact]
        public void DictionaryContent()
        {
            var dictionary = ObjectExtensions.AsDictionary(complexObject);
            var messageContent = new MessageDictionary(id, dictionary);

            var content = messageContent.GetContent<Dictionary<string, object>>();

            Assert.IsType<Dictionary<string, double>>( content["Weather"] );

            var subContent = (Dictionary<string, double>)content["Weather"];

            Assert.True(subContent["Devonport"] == 12.6);
        }

        [Fact]
        public void ComplexContent()
        {
            var dictionary = ObjectExtensions.AsDictionary( complexObject );
            var messageContent = new MessageDictionary(id, dictionary);

            var content = messageContent.Deserialise<TestObject>();

            Assert.True( content.Weather["Devonport"] == 12.6 );
        }

        [Fact]
        public void ComplexContentTheWrongWay()
        {
            var dictionary = ObjectExtensions.AsDictionary(complexObject);
            var messageContent = new MessageDictionary(id, dictionary);

            void wrongBet() => messageContent.GetContent<TestObject>();

            Assert.Throws<ArgumentException>( wrongBet );
        }

        [Fact]
        public void InvalidCast()
        {
            var dictionary = ObjectExtensions.AsDictionary(complexObject);
            MessageDictionary messageContent = new(id, dictionary);

            void badCast() => messageContent.GetContent<string>();

            Assert.Throws<InvalidCastException>(badCast);
        }
    }
}
