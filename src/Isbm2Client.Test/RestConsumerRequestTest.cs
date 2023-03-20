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
    private static readonly string CHANNEL_URI = "/request/consumer";
    private static readonly string CARROTS = "Carrots!";
    private static readonly string BOO = "Boo!";
    private static readonly string BOO_TOPIC = "Boo Topic!";
    private static readonly string EXPIRY = "P1D";

    private readonly RestModel.Session apiSession = new(Guid.NewGuid().ToString(), RestModel.SessionType.RequestConsumer);

    private readonly RestModel.Message apiMessageBoo = new(
        messageId: Guid.NewGuid().ToString(),
        messageContent: MessageContent.From(BOO).ToRestMessageContent(),
        topics: new List<string>() { BOO_TOPIC },
        expiry: EXPIRY
    );

    private readonly RestModel.Message apiMessageCarrot = new(
        messageId: Guid.NewGuid().ToString(),
        messageContent: MessageContent.From(CARROTS).ToRestMessageContent(),
        topics: new List<string>() { BOO_TOPIC },
        expiry: EXPIRY
    );

    [Fact]
    public async Task OpenSession()
    {
        var mock = new Mock<IConsumerRequestServiceApi>();
        mock.Setup(api => api.OpenConsumerRequestSessionAsync(CHANNEL_URI, It.IsAny<RestModel.Session>(), 0, default))
            .ReturnsAsync(apiSession);

        var consumer = new RestConsumerRequest( mock.Object );
        var session = await consumer.OpenSession(CHANNEL_URI);

        Assert.True(!string.IsNullOrEmpty(session.Id));
        Assert.Equal(apiSession.SessionId, session.Id);
        Assert.IsType<RequestConsumerSession>(session);
    }

    [Fact]
    public async Task CloseSession()
    {
        var mock = new Mock<IConsumerRequestServiceApi>();
        var consumer = new RestConsumerRequest(mock.Object);

        var sessionId = Guid.NewGuid().ToString();
        await consumer.CloseSession(sessionId);

        mock.Verify(api => api.CloseSessionAsync(sessionId, 0, default), Times.Exactly(1));
    }

    [Fact]
    public async Task PostRequest()
    {
        var sessionId = Guid.NewGuid().ToString();

        var mock = new Mock<IConsumerRequestServiceApi>();
        mock.Setup(api => api.PostRequestAsync(sessionId, It.IsAny<RestModel.Message>(), 0, default))
            .ReturnsAsync(apiMessageBoo);

        var consumer = new RestConsumerRequest(mock.Object);

        var request = await consumer.PostRequest(sessionId, BOO, BOO_TOPIC, EXPIRY);

        Assert.True(!string.IsNullOrEmpty(request.Id));
        Assert.IsType<RequestMessage>(request);
    }

    [Fact]
    public async Task ReadResponse()
    {
        var sessionId = Guid.NewGuid().ToString();
        var requestId = Guid.NewGuid().ToString();

        var mock = new Mock<IConsumerRequestServiceApi>();
        mock.Setup(api => api.ReadResponseAsync(sessionId, requestId, 0, default))
            .ReturnsAsync(apiMessageCarrot);

        var consumer = new RestConsumerRequest(mock.Object);

        var response = await consumer.ReadResponse(sessionId, requestId);

        Assert.True(!string.IsNullOrEmpty(response.Id));
        Assert.IsType<ResponseMessage>(response);
        Assert.Equal( response.RequestMessageId, requestId );

        await consumer.RemoveResponse(sessionId, requestId);

        mock.Verify( api => api.RemoveResponseAsync(sessionId, requestId, 0, default), Times.Exactly(1));

        var content = response.MessageContent.Deserialise<string>();

        Assert.NotNull(content);
        Assert.Contains(CARROTS, content);
    }

    [Fact]
    public async Task ExpirePublication()
    {
        var sessionId = Guid.NewGuid().ToString();
        var messageId = Guid.NewGuid().ToString();

        var mock = new Mock<IConsumerRequestServiceApi>();

        var consumer = new RestConsumerRequest(mock.Object);
        await consumer.ExpireRequest(sessionId, messageId);

        mock.Verify(api => api.ExpireRequestAsync(sessionId, messageId, 0, default), Times.Exactly(1));
    }
}
