using Isbm2Client.Extensions;
using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2Client.Service;
using Isbm2RestClient.Api;
using Moq;
using System.Text.Json;
using RestModel = Isbm2RestClient.Model;

namespace Isbm2Client.Test;

public class RestProviderRequestTest
{
    private static string CHANNEL_URI = "/fred";
    private static readonly string YO_TOPIC = "Yo!";
    private static readonly string YO = "Yo!";
    private static readonly string CARROTS = "Carrots!";

    private readonly IProviderRequest provider;

    public RestProviderRequestTest()
    {
        provider = SetupDefault();
    }

    private static IProviderRequest SetupDefault()
    {
        var session = new RestModel.Session(Guid.NewGuid().ToString(), RestModel.SessionType.RequestProvider);

        var yoMessage = new RestModel.Message(
            messageId: Guid.NewGuid().ToString(),
            messageContent: MessageContent.From(YO).ToRestMessageContent(),
            topics: new List<string>() { YO_TOPIC }
        );

        var carrotMessage = new RestModel.Message(
            messageId: Guid.NewGuid().ToString(),
            messageContent: MessageContent.From(CARROTS).ToRestMessageContent(),
            topics: new List<string>() { YO_TOPIC }
        );

        var mock = new Mock<IProviderRequestServiceApi>();

        mock.Setup(api => api.OpenProviderRequestSessionAsync(CHANNEL_URI, It.IsAny<RestModel.Session>(), 0, default))
            .ReturnsAsync(session);

        mock.Setup(api => api.ReadRequestAsync(session.SessionId, 0, default))
            .ReturnsAsync(yoMessage);

        mock.Setup(api => api.PostResponseAsync(session.SessionId, yoMessage.MessageId, It.IsAny<RestModel.Message>(), 0, default))
            .ReturnsAsync(carrotMessage);

        return new RestProviderRequest(mock.Object);
    }
    private static IProviderRequest SetupJsonDocument()
    {
        var session = new RestModel.Session(Guid.NewGuid().ToString(), RestModel.SessionType.RequestProvider);

        var inputContent = new Dictionary<string, object>()
        {
            { "fred", "barney" },
            { "wilma", "betty" }
        };

        var document = JsonSerializer.SerializeToDocument(inputContent);

        var jsonMessage = new RestModel.Message(
            messageId: Guid.NewGuid().ToString(),
            messageContent: MessageContent.From(document).ToRestMessageContent(),
            topics: new List<string>() { YO_TOPIC }
        );

        var mock = new Mock<IProviderRequestServiceApi>();

        mock.Setup(api => api.OpenProviderRequestSessionAsync(CHANNEL_URI, It.IsAny<RestModel.Session>(), 0, default))
            .ReturnsAsync(session);

        mock.Setup(api => api.ReadRequestAsync(session.SessionId, 0, default))
            .ReturnsAsync(jsonMessage);

        return new RestProviderRequest(mock.Object);
    }
    private static IProviderRequest SetupComplexObject()
    {
        var session = new RestModel.Session(Guid.NewGuid().ToString(), RestModel.SessionType.RequestProvider);

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

        var jsonMessage = new RestModel.Message(
            messageId: Guid.NewGuid().ToString(),
            messageContent: MessageContent.From(complexObject).ToRestMessageContent(),
            topics: new List<string>() { YO_TOPIC }
        );

        var mock = new Mock<IProviderRequestServiceApi>();

        mock.Setup(api => api.OpenProviderRequestSessionAsync(CHANNEL_URI, It.IsAny<RestModel.Session>(), 0, default))
            .ReturnsAsync(session);

        mock.Setup(api => api.ReadRequestAsync(session.SessionId, 0, default))
            .ReturnsAsync(jsonMessage);

        return new RestProviderRequest(mock.Object);
    }

    [Fact]
    public async Task OpenAndCloseSession()
    {
        var session = await provider.OpenSession( CHANNEL_URI, YO_TOPIC );

        Assert.True(!string.IsNullOrEmpty(session.Id));

        await provider.CloseSession( session.Id );
    }

    [Fact]
    public async Task ReadStringRequest()
    {
        var session = await provider.OpenSession(CHANNEL_URI, YO_TOPIC);

        var request = await provider.ReadRequest(session.Id);
        await provider.RemoveRequest(session.Id);

        Assert.True(!string.IsNullOrEmpty(request.Id));

        var content = request.MessageContent.Deserialise<string>();

        Assert.NotNull(content);
        Assert.Contains(YO, content);
    }

    [Fact]
    public async Task ReadJsonDocumentRequest()
    {
        // Bypass default provider
        var provider = SetupJsonDocument();

        var session = await provider.OpenSession(CHANNEL_URI, YO_TOPIC);

        var request = await provider.ReadRequest(session.Id);
        await provider.RemoveRequest(session.Id);

        Assert.True(!string.IsNullOrEmpty(request.Id));

        var content = request.MessageContent.Content;

        Assert.NotNull(content);

        if (content is not null)
            Assert.Contains("barney", content.RootElement.GetProperty("fred").GetString());
    }

    [Fact]
    public async Task ReadComplexObjectRequest()
    {
        // Bypass default provider
        var provider = SetupComplexObject();

        var session = await provider.OpenSession(CHANNEL_URI, YO_TOPIC);

        var request = await provider.ReadRequest(session.Id);
        await provider.RemoveRequest(session.Id);

        Assert.True(!string.IsNullOrEmpty(request.Id));

        var content = request.MessageContent.Deserialise<TestObject>();

        Assert.True(content.Weather["Hobart"] == 2);
    }

    [Fact]
    public async Task ReadRequestPostResponse()
    {
        var session = await provider.OpenSession(CHANNEL_URI, YO_TOPIC);

        var request = await provider.ReadRequest(session.Id);
        await provider.RemoveRequest(session.Id);

        var content = request.MessageContent.Deserialise<string>();

        Assert.True(content == YO);

        var message = await provider.PostResponse(session.Id, request.Id, CARROTS);

        Assert.True( !string.IsNullOrEmpty(message.Id) );
        Assert.Contains(CARROTS, message.MessageContent.Deserialise<string>());
    }
}
