using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2Client.Service;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using RestModel = Isbm2RestClient.Model;
using Moq;

namespace Isbm2Client.Test.Unit_Tests;

public class RestConsumerRequestTest
{
    private readonly string channelUri = "/fred";
    private readonly string sessionId = Guid.NewGuid().ToString();
    private readonly IConsumerRequest consumer;

    public RestConsumerRequestTest() 
    {
        var sessionParams = new RestModel.Session()
        {
            SessionType = RestModel.SessionType.RequestConsumer
        };

        var expectedSession = new RestModel.Session
        (
            sessionId, 
            RestModel.SessionType.RequestConsumer, 
            null, 
            new List<string>(), 
            new List<RestModel.FilterExpression>()
        );

        var mock = new Mock<IConsumerRequestServiceApi>();

        mock.Setup( api => api.OpenConsumerRequestSessionAsync(channelUri, sessionParams, 0, default) )
            .ReturnsAsync( expectedSession );

        mock.SetupSequence(api => api.CloseSessionAsync(sessionId, 0, default))
            .Returns(Task.CompletedTask)
            .Throws(new ApiException(1, "You can't do that twice!"));

        consumer = new RestConsumerRequest( mock.Object );
    }

    [Fact]
    public async Task OpenAndCloseSession()
    {
        var session = await consumer.OpenSession(channelUri);

        await consumer.CloseSession(session.Id);
    }

    [Fact]
    public async Task CantCloseSessionTwice()
    {
        RequestConsumerSession session = await consumer.OpenSession(channelUri);

        await consumer.CloseSession(session.Id);

        Task closeAgain() => consumer.CloseSession(session.Id);

        await Assert.ThrowsAsync<ApiException>(closeAgain);
    }
}
