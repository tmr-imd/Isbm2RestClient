using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2RestClient.Client;

namespace Isbm2Client.Test;

[Collection("Request Consumer collection")]
public class RestConsumerRequestTest
{
    private readonly RequestChannel channel;
    private readonly IConsumerRequest consumer;
    private readonly IProviderRequest provider;

    private static readonly string BOO = "Boo!";
    private static readonly string BOO_TOPIC = "Boo Topic!";

    public RestConsumerRequestTest( RequestConsumerFixture fixture )
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

        _ = await consumer.PostRequest( session.Id, BOO, BOO_TOPIC );

        await consumer.CloseSession( session.Id );
    }

    [Fact]
    public async Task PostRequestReadResponse()
    {
        var providerSession = await provider.OpenSession(channel.Uri, BOO_TOPIC);
        var consumerSession = await consumer.OpenSession(channel.Uri);

        try
        {
            await consumer.PostRequest(consumerSession.Id, BOO, BOO_TOPIC);

            var requestMessage = await provider.ReadRequest(providerSession.Id);

            Assert.IsType<MessageString>(requestMessage.MessageContent);

            var requestContent = requestMessage.MessageContent.GetContent<string>();

            Assert.True(requestContent == BOO);

            var responseMessage = await provider.PostResponse(providerSession.Id, requestMessage.Id, "Carrots!");

            Assert.NotNull( responseMessage );
            Assert.Contains("Carrots", responseMessage.MessageContent.GetContent<string>());

            var message = await consumer.ReadResponse( consumerSession.Id, requestMessage.Id );

            Assert.IsType<MessageString>(message.MessageContent);

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