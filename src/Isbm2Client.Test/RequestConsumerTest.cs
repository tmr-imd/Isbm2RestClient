using Isbm2Client.Extensions;
using Isbm2Client.Model;
using Isbm2Client.Service;
using Moq;
using RestApi = Isbm2RestClient.Api;
using RestModel = Isbm2RestClient.Model;

namespace Isbm2Client.Test;

public class RequestResponseConsumerTest
{
    private static readonly string CHANNEL_URI = "/request/consumer";
    private static readonly string CARROTS = "Carrots!";
    private static readonly string BOO = "Boo!";
    private static readonly string BOO_TOPIC = "Boo Topic!";
    private static readonly string EXPIRY = "P1D";

    [Fact]
    public async Task OpenSession()
    {
        var apiSession = new RestModel.Session
        (
            Guid.NewGuid().ToString(), 
            RestModel.SessionType.RequestConsumer
        );

        var mock = new Mock<RestApi.IConsumerRequestServiceApi>();
        mock.Setup(api => api.OpenConsumerRequestSessionAsync(CHANNEL_URI, It.IsAny<RestModel.Session>(), 0, default))
            .ReturnsAsync(apiSession);

        var consumer = new RestConsumerRequest( mock.Object );
        RequestConsumerSession session = await consumer.OpenSession(CHANNEL_URI);

        Assert.True(!string.IsNullOrEmpty(session.Id));
        Assert.Equal(apiSession.SessionId, session.Id);
    }

    [Fact]
    public async Task CloseSession()
    {
        var mock = new Mock<RestApi.IConsumerRequestServiceApi>();
        var consumer = new RestConsumerRequest(mock.Object);

        var sessionId = Guid.NewGuid().ToString();
        await consumer.CloseSession(sessionId);

        mock.Verify(api => api.CloseSessionAsync(sessionId, 0, default), Times.Exactly(1));
    }

    [Fact]
    public async Task PostRequest()
    {
        var sessionId = Guid.NewGuid().ToString();

        var apiMessageBoo = new RestModel.Message
        (
            messageId: Guid.NewGuid().ToString(),
            messageContent: MessageContent.From(BOO).ToRestMessageContent(),
            topics: new List<string>() { BOO_TOPIC },
            expiry: EXPIRY
        );

        var mock = new Mock<RestApi.IConsumerRequestServiceApi>();
        mock.Setup(api => api.PostRequestAsync(sessionId, It.IsAny<RestModel.Message>(), 0, default))
            .ReturnsAsync(apiMessageBoo);

        var consumer = new RestConsumerRequest(mock.Object);

        RequestMessage request = await consumer.PostRequest(sessionId, BOO, BOO_TOPIC, EXPIRY);

        Assert.True(!string.IsNullOrEmpty(request.Id));
        Assert.Equal(apiMessageBoo.MessageId, request.Id);

        var content = request.MessageContent.Deserialise<string>();

        Assert.NotNull(content);
        Assert.Contains(BOO, content);
    }

    [Fact]
    public async Task ReadResponse()
    {
        var sessionId = Guid.NewGuid().ToString();
        var requestId = Guid.NewGuid().ToString();

        var apiMessageCarrot = new RestModel.Message
        (
            messageId: Guid.NewGuid().ToString(),
            messageContent: MessageContent.From(CARROTS).ToRestMessageContent(),
            topics: new List<string>() { BOO_TOPIC },
            expiry: EXPIRY
        );

        var mock = new Mock<RestApi.IConsumerRequestServiceApi>();
        mock.Setup(api => api.ReadResponseAsync(sessionId, requestId, 0, default))
            .ReturnsAsync(apiMessageCarrot);

        var consumer = new RestConsumerRequest(mock.Object);

        ResponseMessage response = await consumer.ReadResponse(sessionId, requestId);

        Assert.True(!string.IsNullOrEmpty(response.Id));
        Assert.Equal( apiMessageCarrot.MessageId, response.Id );
        Assert.Equal( requestId, response.RequestMessageId );

        var content = response.MessageContent.Deserialise<string>();

        Assert.NotNull(content);
        Assert.Contains(CARROTS, content);
    }

    [Fact]
    public async Task RemoveResponse()
    {
        var sessionId = Guid.NewGuid().ToString();
        var requestId = Guid.NewGuid().ToString();

        var mock = new Mock<RestApi.IConsumerRequestServiceApi>();
        var consumer = new RestConsumerRequest(mock.Object);

        await consumer.RemoveResponse(sessionId, requestId);

        mock.Verify(api => api.RemoveResponseAsync(sessionId, requestId, 0, default), Times.Exactly(1));
    }

    [Fact]
    public async Task ExpirePublication()
    {
        var sessionId = Guid.NewGuid().ToString();
        var messageId = Guid.NewGuid().ToString();

        var mock = new Mock<RestApi.IConsumerRequestServiceApi>();

        var consumer = new RestConsumerRequest(mock.Object);
        await consumer.ExpireRequest(sessionId, messageId);

        mock.Verify(api => api.ExpireRequestAsync(sessionId, messageId, 0, default), Times.Exactly(1));
    }
}
