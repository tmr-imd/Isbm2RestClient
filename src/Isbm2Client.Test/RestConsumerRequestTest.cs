using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2Client.Service;
using Isbm2RestClient.Client;

namespace Isbm2Client.Test;

[Collection("Request Channel collection")]
public class RestConsumerRequestTest
{
    private readonly RequestChannel channel;
    private readonly IConsumerRequest consumer;
    private readonly IProviderRequest provider;

    private readonly string[] topics = { "topic!" };

    public RestConsumerRequestTest( RequestChannelFixture fixture )
    {
        channel = fixture.RequestChannel;
        provider = fixture.Provider;
        consumer = fixture.Consumer;
    }

    [Fact]
    public async Task OpenAndCloseSession()
    {
        RequestConsumerSession session = await consumer.OpenSession( channel.Uri );

        await consumer.CloseSession( session.Id );
    }

    [Fact]
    public async Task CantCloseSessionTwice()
    {
        RequestConsumerSession session = await consumer.OpenSession( channel.Uri);

        await consumer.CloseSession(session.Id);

        Task closeAgain() => consumer.CloseSession(session.Id);

        await Assert.ThrowsAsync<ApiException>(closeAgain);
    }

    [Fact]
    public async Task PostRequest()
    {
        RequestConsumerSession session = await consumer.OpenSession(channel.Uri);

        _ = await consumer.PostRequest( session.Id, "Yo!", topics );

        await consumer.CloseSession( session.Id );
    }

    [Fact]
    public async Task PostRequestReadResponse()
    {
        var providerSession = await provider.OpenSession(channel.Uri, topics);
        var consumerSession = await consumer.OpenSession(channel.Uri);

        await consumer.PostRequest(consumerSession.Id, "Yo!", topics);

        try
        {
            var requestMessage = await provider.ReadRequest(providerSession.Id);

            Assert.IsType<MessageContent<string>>(requestMessage.MessageContent);

            var requestContent = requestMessage.MessageContent.GetContent<string>();

            Assert.True(requestContent == "Yo!");

            var responseMessage = await provider.PostResponse(providerSession.Id, requestMessage.Id, "Carrots!");

            Assert.NotNull( responseMessage );
            Assert.Contains("Carrots", responseMessage.MessageContent.GetContent<string>());

            var message = await consumer.ReadResponse( consumerSession.Id, requestMessage.Id );

            Assert.IsType<MessageContent<string>>(message.MessageContent);

            var content = message.MessageContent.GetContent<string>();

            Assert.NotNull(content);
            Assert.Contains("Carrots!", content);
        }
        finally
        {
            await consumer.CloseSession(consumerSession.Id);
            await provider.CloseSession(providerSession.Id);
        }
    }
}