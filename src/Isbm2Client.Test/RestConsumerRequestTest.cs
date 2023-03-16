using Isbm2Client.Extensions;
using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2Client.Service;
using Isbm2RestClient.Api;
using Moq;
using RestModel = Isbm2RestClient.Model;

namespace Isbm2Client.Test;

public class RestConsumerRequestTest
{
    private static string CHANNEL_URI = "/fred";
    private static readonly string CARROTS = "Carrots!";
    private static readonly string BOO = "Boo!";
    private static readonly string BOO_TOPIC = "Boo Topic!";
    private static readonly string EXPIRY = "P1D";

    private readonly IConsumerRequest consumer;

    public RestConsumerRequestTest()
    {
        var session = new RestModel.Session(Guid.NewGuid().ToString(), RestModel.SessionType.RequestConsumer);

        var booMessage = new RestModel.Message(
            messageId: Guid.NewGuid().ToString(),
            messageContent: MessageContent.From(BOO).ToRestMessageContent(),
            topics: new List<string>() { BOO_TOPIC },
            expiry: EXPIRY
        );

        var carrotMessage = new RestModel.Message(
            messageId: Guid.NewGuid().ToString(),
            messageContent: MessageContent.From(CARROTS).ToRestMessageContent(),
            topics: new List<string>() { BOO_TOPIC },
            expiry: EXPIRY
        );

        var mock = new Mock<IConsumerRequestServiceApi>();

        mock.Setup(api => api.OpenConsumerRequestSessionAsync(CHANNEL_URI, It.IsAny<RestModel.Session>(), 0, default))
            .ReturnsAsync(session);

        mock.Setup(api => api.PostRequestAsync(session.SessionId, It.IsAny<RestModel.Message>(), 0, default))
            .ReturnsAsync(booMessage);

        mock.Setup(api => api.ReadResponseAsync(session.SessionId, booMessage.MessageId, 0, default))
            .ReturnsAsync(carrotMessage);

        consumer = new RestConsumerRequest(mock.Object);
    }

    [Fact]
    public async Task OpenAndCloseSession()
    {
        var session = await consumer.OpenSession(CHANNEL_URI);

        Assert.True(!string.IsNullOrEmpty(session.Id));

        await consumer.CloseSession(session.Id);
    }

    [Fact]
    public async Task PostRequest()
    {
        RequestConsumerSession session = await consumer.OpenSession(CHANNEL_URI);

        var request = await consumer.PostRequest(session.Id, BOO, BOO_TOPIC, EXPIRY);

        Assert.True(!string.IsNullOrEmpty(request.Id));

        await consumer.CloseSession(session.Id);
    }

    [Fact]
    public async Task ReadResponse()
    {
        RequestConsumerSession session = await consumer.OpenSession(CHANNEL_URI);

        var request = await consumer.PostRequest(session.Id, BOO, BOO_TOPIC, EXPIRY);

        var response = await consumer.ReadResponse(session.Id, request.Id);
        await consumer.RemoveResponse(session.Id, request.Id);

        Assert.True(!string.IsNullOrEmpty(response.Id));

        var content = response.MessageContent.Deserialise<string>();

        Assert.NotNull(content);
        Assert.Contains(CARROTS, content);
    }

    [Fact]
    public async Task ExpirePublication()
    {
        RequestConsumerSession session = await consumer.OpenSession(CHANNEL_URI);

        Message message = await consumer.PostRequest(session.Id, BOO, BOO_TOPIC);

        Assert.True(!string.IsNullOrEmpty(message.Id));

        await consumer.ExpireRequest(session.Id, message.Id);
    }
}
