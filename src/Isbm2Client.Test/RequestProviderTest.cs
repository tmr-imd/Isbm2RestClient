using Isbm2Client.Extensions;
using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2Client.Service;
using Moq;
using System.Text.Json;
using RestModel = Isbm2RestClient.Model;
using RestApi = Isbm2RestClient.Api;

namespace Isbm2Client.Test;

public class RequestResponseProviderTest
{
    private static string CHANNEL_URI = "/request/provider";
    private static readonly string YO_TOPIC = "Yo!";
    private static readonly string YO = "Yo!";
    private static readonly string CARROTS = "Carrots!";

    [Fact]
    public async Task OpenSession()
    {
        var apiSession = new RestModel.Session
        (
            Guid.NewGuid().ToString(),
            RestModel.SessionType.RequestConsumer
        );

        var mock = new Mock<RestApi.IProviderRequestServiceApi>();
        mock.Setup(api => api.OpenProviderRequestSessionAsync(CHANNEL_URI, It.IsAny<RestModel.Session>(), 0, default))
            .ReturnsAsync(apiSession);

        var provider = new RestProviderRequest(mock.Object);

        RequestProviderSession session = await provider.OpenSession( CHANNEL_URI, YO_TOPIC );

        Assert.True(!string.IsNullOrEmpty(session.Id));
        Assert.Equal(apiSession.SessionId, session.Id);
    }

    [Fact]
    public async Task CloseSession()
    {
        var mock = new Mock<RestApi.IProviderRequestServiceApi>();
        var provider = new RestProviderRequest(mock.Object);

        var sessionId = Guid.NewGuid().ToString();
        await provider.CloseSession(sessionId);

        mock.Verify(api => api.CloseSessionAsync(sessionId, 0, default), Times.Exactly(1));
    }

    [Fact]
    public async Task ReadStringRequest()
    {
        var sessionId = Guid.NewGuid().ToString();
        var apiMessageYo = new RestModel.Message
        (
            messageId: Guid.NewGuid().ToString(),
            messageContent: MessageContent.From(YO).ToRestMessageContent(),
            topics: new List<string>() { YO_TOPIC }
        );

        var mock = new Mock<RestApi.IProviderRequestServiceApi>();
        mock.Setup( api => api.ReadRequestAsync(sessionId, 0, default))
            .ReturnsAsync( apiMessageYo );

        var provider = new RestProviderRequest(mock.Object);

        RequestMessage request = await provider.ReadRequest(sessionId);

        Assert.True(!string.IsNullOrEmpty(request.Id));
        Assert.Equal(apiMessageYo.MessageId, request.Id);

        var content = request.MessageContent.Deserialise<string>();

        Assert.NotNull(content);
        Assert.Contains(YO, content);
    }

    [Fact]
    public async Task ReadJsonDocumentRequest()
    {
        var sessionId = Guid.NewGuid().ToString();
        var inputContent = new Dictionary<string, object>()
        {
            { "fred", "barney" },
            { "wilma", "betty" }
        };

        var document = JsonSerializer.SerializeToDocument(inputContent);

        var apiMessageJson = new RestModel.Message(
            messageId: Guid.NewGuid().ToString(),
            messageContent: MessageContent.From(document).ToRestMessageContent(),
            topics: new List<string>() { YO_TOPIC }
        );

        var mock = new Mock<RestApi.IProviderRequestServiceApi>();
        mock.Setup(api => api.ReadRequestAsync(sessionId, 0, default))
            .ReturnsAsync(apiMessageJson);

        var provider = new RestProviderRequest(mock.Object);

        RequestMessage request = await provider.ReadRequest(sessionId);

        Assert.True(!string.IsNullOrEmpty(request.Id));
        Assert.Equal(apiMessageJson.MessageId, request.Id);

        var content = request.MessageContent.Content;

        Assert.NotNull(content);

        if (content is not null)
            Assert.Contains("barney", content.RootElement.GetProperty("fred").GetString());
    }

    [Fact]
    public async Task ReadComplexObjectRequest()
    {
        var sessionId = Guid.NewGuid().ToString();

        var complexObject = new TestObject()
        {
            Numbers = new[] { 23.0, 45, 100 },
            Text = "Hello",
            Weather = new Dictionary<string, double>()
            {
                {"Devonport", 14.0 },
                {"Hobart", 2.0 }
            }
        };

        var apiMessageTestObject = new RestModel.Message(
            messageId: Guid.NewGuid().ToString(),
            messageContent: MessageContent.From(complexObject).ToRestMessageContent(),
            topics: new List<string>() { YO_TOPIC }
        );


        var mock = new Mock<RestApi.IProviderRequestServiceApi>();
        mock.Setup(api => api.ReadRequestAsync(sessionId, 0, default))
            .ReturnsAsync(apiMessageTestObject);

        var provider = new RestProviderRequest(mock.Object);

        RequestMessage request = await provider.ReadRequest(sessionId);

        Assert.True(!string.IsNullOrEmpty(request.Id));
        Assert.Equal(apiMessageTestObject.MessageId, request.Id);

        var content = request.MessageContent.Deserialise<TestObject>();

        Assert.True(content.Weather["Hobart"] == 2);
    }

    [Fact]
    public async Task ReadRequestPostResponse()
    {
        var sessionId = Guid.NewGuid().ToString();
        var requestId = Guid.NewGuid().ToString();

        var apiMessageCarrot = new RestModel.Message
        (
            messageId: Guid.NewGuid().ToString(),
            messageContent: MessageContent.From(CARROTS).ToRestMessageContent(),
            topics: new List<string>() { YO_TOPIC }
        );

        var mock = new Mock<RestApi.IProviderRequestServiceApi>();
        mock.Setup(api => api.PostResponseAsync(sessionId, requestId, It.IsAny<RestModel.Message>(), 0, default))
            .ReturnsAsync(apiMessageCarrot);

        var provider = new RestProviderRequest(mock.Object);

        var message = await provider.PostResponse(sessionId, requestId, CARROTS);

        Assert.True(!string.IsNullOrEmpty(message.Id));
        Assert.Equal(apiMessageCarrot.MessageId, message.Id);
        Assert.Contains(CARROTS, message.MessageContent.Deserialise<string>());
    }

    [Fact]
    public async Task RemoveRequest()
    {
        var mock = new Mock<RestApi.IProviderRequestServiceApi>();
        var provider = new RestProviderRequest(mock.Object);

        var sessionId = Guid.NewGuid().ToString();

        await provider.RemoveRequest(sessionId);

        mock.Verify(api => api.RemoveRequestAsync(sessionId, 0, default), Times.Exactly(1));
    }
}
