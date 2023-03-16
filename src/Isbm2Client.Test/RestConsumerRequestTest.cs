using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2Client.Service;
using Isbm2RestClient.Api;
using RestModel = Isbm2RestClient.Model;
using Moq;
using Isbm2RestClient.Model;
using Isbm2Client.Extensions;

namespace Isbm2Client.Test;

public class RestConsumerRequestTest
{
    private readonly string sessionId = Guid.NewGuid().ToString();
    private readonly IConsumerRequest consumer;

    private static string CHANNEL_URI = "/fred";
    private static readonly string BOO = "Boo!";
    private static readonly string BOO_TOPIC = "Boo Topic!";
    private static readonly string EXPIRY = "P1D";

    public RestConsumerRequestTest()
    {
        var sessionParams = new RestModel.Session(sessionType: SessionType.RequestConsumer);

        var expectedSession = new RestModel.Session
        (
            sessionId,
            SessionType.RequestConsumer,
            null,
            new List<string>(),
            new List<FilterExpression>()
        );

        var mock = new Mock<IConsumerRequestServiceApi>();

        mock.Setup(api => api.OpenConsumerRequestSessionAsync(CHANNEL_URI, sessionParams, 0, default))
            .ReturnsAsync(expectedSession);

        var message = new RestModel.Message
        (
            messageId: Guid.NewGuid().ToString(),
            messageContent: Model.MessageContent.From(BOO).ToRestMessageContent(),
            topics: new List<string>() { BOO_TOPIC },
            expiry: EXPIRY
        );

        mock.Setup(api => api.PostRequestAsync(sessionId, message, 0, default))
            .ReturnsAsync(message);

        consumer = new RestConsumerRequest(mock.Object);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task OpenAndCloseSession()
    {
        var session = await consumer.OpenSession(CHANNEL_URI);

        await consumer.CloseSession(session.Id);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task PostRequest()
    {
        RequestConsumerSession session = await consumer.OpenSession(CHANNEL_URI);

        _ = await consumer.PostRequest(session.Id, BOO, BOO_TOPIC, EXPIRY);

        await consumer.CloseSession(session.Id);
    }
}
