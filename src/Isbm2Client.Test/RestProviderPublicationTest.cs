using Isbm2Client.Model;
using Isbm2Client.Service;
using RestModel = Isbm2RestClient.Model;
using RestApi = Isbm2RestClient.Api;
using Moq;
using Isbm2Client.Extensions;

namespace Isbm2Client.Test;

public class RestProviderPublicationTest
{
    public const string CHANNEL_URI = "/pubsub/provider";

    private static readonly string BOO = "Boo!";
    private static readonly string BOO_TOPIC = "Boo Topic!";
    private static readonly string EXPIRY = "P1D";

    [Fact]
    public async Task OpenSession()
    {
        var apiSession = new RestModel.Session
        (
            Guid.NewGuid().ToString(),
            RestModel.SessionType.PublicationConsumer
        );

        var mock = new Mock<RestApi.IProviderPublicationServiceApi>();
        mock.Setup(api => api.OpenPublicationSessionAsync(CHANNEL_URI, 0, default))
            .ReturnsAsync(apiSession);

        var provider = new RestProviderPublication(mock.Object);
        PublicationProviderSession session = await provider.OpenSession( CHANNEL_URI );

        Assert.True(!string.IsNullOrEmpty(session.Id));
        Assert.Equal(apiSession.SessionId, session.Id);
    }

    [Fact]
    public async Task CloseSession()
    {
        var mock = new Mock<RestApi.IProviderPublicationServiceApi>();
        var provider = new RestProviderPublication(mock.Object);

        var sessionId = Guid.NewGuid().ToString();
        await provider.CloseSession(sessionId);

        mock.Verify(api => api.CloseSessionAsync(sessionId, 0, default), Times.Once);
    }

    [Fact]
    public async Task PostPublication()
    {
        var sessionId = Guid.NewGuid().ToString();

        var apiMessageBoo = new RestModel.Message
        (
            messageId: Guid.NewGuid().ToString(),
            messageContent: MessageContent.From(BOO).ToRestMessageContent(),
            topics: new List<string>() { BOO_TOPIC },
            expiry: EXPIRY
        );

        var mock = new Mock<RestApi.IProviderPublicationServiceApi>();
        mock.Setup(api => api.PostPublicationAsync(sessionId, It.IsAny<RestModel.Message>(), 0, default))
            .ReturnsAsync(apiMessageBoo);

        var provider = new RestProviderPublication(mock.Object);

        PublicationMessage message = await provider.PostPublication(sessionId, BOO, BOO_TOPIC, EXPIRY);

        Assert.True(!string.IsNullOrEmpty(message.Id));

        var content = message.MessageContent.Deserialise<string>();

        Assert.NotNull(content);
        Assert.Contains(BOO, content);
    }

    [Fact]
    public async Task ExpirePublication()
    {
        var sessionId = Guid.NewGuid().ToString();
        var messageId = Guid.NewGuid().ToString();

        var mock = new Mock<RestApi.IProviderPublicationServiceApi>();
        var provider = new RestProviderPublication(mock.Object);

        await provider.ExpirePublication(sessionId, messageId);

        mock.Verify(api => api.ExpirePublicationAsync(sessionId, messageId, 0, default), Times.Once);
    }
}