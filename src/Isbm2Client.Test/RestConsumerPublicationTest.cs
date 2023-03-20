using Isbm2Client.Service;
using RestApi = Isbm2RestClient.Api;
using RestModel = Isbm2RestClient.Model;
using Moq;
using System.Text.Json;
using Isbm2Client.Model;
using Isbm2Client.Extensions;

namespace Isbm2Client.Test;

public class RestConsumerPublicationTest
{
    public const string CHANNEL_URI = "/pubsub/consumer";

    private static readonly string YO = "Yo!";
    private static readonly string YO_TOPIC = "Yo Topic!";

    private RestModel.Session apiSession => new
    (
        Guid.NewGuid().ToString(), 
        RestModel.SessionType.PublicationConsumer
    );

    [Fact]
    public async Task OpenSession()
    {
        var mock = new Mock<RestApi.IConsumerPublicationServiceApi>();
        mock.Setup( api => api.OpenSubscriptionSessionAsync(CHANNEL_URI, It.IsAny<RestModel.Session>(), 0, default) )
            .ReturnsAsync( apiSession );

        var consumer = new RestConsumerPublication( mock.Object );

        var session = await consumer.OpenSession( CHANNEL_URI, YO_TOPIC );

        Assert.True(!string.IsNullOrEmpty(session.Id));
        Assert.IsType<PublicationConsumerSession>(session);
    }

    [Fact]
    public async Task CloseSession()
    {
        var mock = new Mock<RestApi.IConsumerPublicationServiceApi>();
        var consumer = new RestConsumerPublication(mock.Object);

        var sessionId = Guid.NewGuid().ToString();
        await consumer.CloseSession(sessionId);

        mock.Verify( api => api.CloseSessionAsync(sessionId, 0, default), Times.Once );
    }

    [Fact]
    public async Task ReadStringPublication()
    {
        var sessionId = Guid.NewGuid().ToString();

        var apiMessageYo = new RestModel.Message
        (
            messageId: Guid.NewGuid().ToString(),
            messageContent: MessageContent.From(YO).ToRestMessageContent(),
            topics: new List<string>() { YO_TOPIC }
        );
        
        var mock = new Mock<RestApi.IConsumerPublicationServiceApi>();
        mock.Setup( api => api.ReadPublicationAsync(sessionId, 0, default) )
            .ReturnsAsync( apiMessageYo );

        var consumer = new RestConsumerPublication(mock.Object);

        var message = await consumer.ReadPublication(sessionId);
        Assert.True(!string.IsNullOrEmpty(message.Id));
        Assert.IsType<PublicationMessage>(message);

        var content = message.MessageContent.Deserialise<string>();

        Assert.NotNull(content);
        Assert.Contains(YO, content);
    }

    [Fact]
    public async Task ReadJsonDocumentPublication()
    {
        var sessionId = Guid.NewGuid().ToString();

        var document = JsonSerializer.SerializeToDocument
        (
            new Dictionary<string, object>()
            {
                { "fred", "barney" },
                { "wilma", "betty" }
            }
        );

        var apiMessageJson = new RestModel.Message
        (
            messageId: Guid.NewGuid().ToString(),
            messageContent: MessageContent.From(document).ToRestMessageContent(),
            topics: new List<string>() { YO_TOPIC }
        );

        var mock = new Mock<RestApi.IConsumerPublicationServiceApi>();
        mock.Setup(api => api.ReadPublicationAsync(sessionId, 0, default))
            .ReturnsAsync(apiMessageJson);

        var consumer = new RestConsumerPublication(mock.Object);

        var message = await consumer.ReadPublication(sessionId);
        Assert.True(!string.IsNullOrEmpty(message.Id));
        Assert.IsType<PublicationMessage>(message);

        var content = message.MessageContent.Content;

        Assert.NotNull(content);

        if (content is not null)
            Assert.Contains("barney", content.RootElement.GetProperty("fred").GetString());
    }

    [Fact]
    public async Task ReadComplexObjectRequest()
    {
        var sessionId = Guid.NewGuid().ToString();

        var document = JsonSerializer.SerializeToDocument
        (
            new TestObject()
            {
                Numbers = new[] { 23.0, 45, 100 },
                Text = "Hello",
                Weather = new Dictionary<string, double>()
                {
                    {"Devonport", 14.0 },
                    {"Hobart", 2.0 }
                }
            }
        );

        var apiMessageTestObject = new RestModel.Message
        (
            messageId: Guid.NewGuid().ToString(),
            messageContent: MessageContent.From(document).ToRestMessageContent(),
            topics: new List<string>() { YO_TOPIC }
        );

        var mock = new Mock<RestApi.IConsumerPublicationServiceApi>();
        mock.Setup(api => api.ReadPublicationAsync(sessionId, 0, default))
            .ReturnsAsync(apiMessageTestObject);

        var consumer = new RestConsumerPublication(mock.Object);
        var message = await consumer.ReadPublication(sessionId);
        await consumer.RemovePublication(sessionId);

        Assert.True(!string.IsNullOrEmpty(message.Id));
        Assert.IsType<PublicationMessage>(message);

        var content = message.MessageContent.Deserialise<TestObject>();

        Assert.True(content.Weather["Hobart"] == 2);
    }

    [Fact]
    public async Task RemovePublication()
    {
        var sessionId = Guid.NewGuid().ToString();

        var mock = new Mock<RestApi.IConsumerPublicationServiceApi>();

        var consumer = new RestConsumerPublication(mock.Object);

        // Hmm, should be testing that ReadPublication was called first?
        await consumer.RemovePublication(sessionId);
        mock.Verify(api => api.RemovePublicationAsync(sessionId, 0, default), Times.Once());
    }
}