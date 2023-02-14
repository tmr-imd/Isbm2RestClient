using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2Client.Service;
using Isbm2RestClient.Client;

namespace Isbm2Client.Test;

[Collection("Request Channel collection")]
public class RestConsumerRequestTest
{
    private readonly RequestChannelFixture fixture;

    private readonly RequestChannel channel;
    private readonly IConsumerRequest consumer;
    private readonly IProviderRequest provider;

    private readonly string[] topics = { "topic!" };

    public RestConsumerRequestTest( RequestChannelFixture fixture )
    {
        this.fixture = fixture;

        channel = fixture.RequestChannel;
        provider = fixture.Provider;
        consumer = fixture.Consumer;
    }

    [Fact]
    public async Task OpenAndCloseSession()
    {
        var request = new RestConsumerRequest( fixture.Config );
        var session = await request.OpenSession( fixture.RequestChannel );

        Assert.NotNull( session );

        await request.CloseSession( session );
    }

    [Fact]
    public async Task CantCloseSessionTwice()
    {
        var request = new RestConsumerRequest(fixture.Config);
        var session = await request.OpenSession( channel);

        Assert.NotNull(session);

        await request.CloseSession(session);

        Task closeAgain() => request.CloseSession(session);

        await Assert.ThrowsAsync<ApiException>(closeAgain);
    }

    [Fact]
    public async Task PostRequest()
    {
        var request = new RestConsumerRequest(fixture.Config);
        var session = await request.OpenSession(fixture.RequestChannel);

        Assert.NotNull(session);

        var message = await request.PostRequest( session, "Yo!", topics );

        Assert.NotNull( message );

        await request.CloseSession( session );
    }

    [Fact]
    public async Task PostRequestReadResponse()
    {
        var providerSession = await provider.OpenSession(channel, topics);
        var consumerSession = await consumer.OpenSession(channel);

        await consumer.PostRequest(consumerSession, "Yo!", topics);

        try
        {
            var requestMessage = await provider.ReadRequest(providerSession);

            Assert.IsType<MessageContent<string>>(requestMessage.MessageContent);

            var requestContent = requestMessage.MessageContent.GetContent<string>();

            Assert.True(requestContent == "Yo!");

            var responseMessage = await provider.PostResponse(providerSession, requestMessage, "Carrots!");

            Assert.NotNull( responseMessage );
            Assert.Contains("Carrots", responseMessage.MessageContent.GetContent<string>());

            var message = await consumer.ReadResponse( consumerSession, requestMessage );

            Assert.IsType<MessageContent<string>>(message.MessageContent);

            var content = message.MessageContent.GetContent<string>();

            Assert.NotNull(content);
            Assert.Contains("Carrots!", content);
        }
        finally
        {
            await consumer.CloseSession(consumerSession);
            await provider.CloseSession(providerSession);
        }
    }
}