using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2RestClient.Client;

namespace Isbm2Client.Integration.Test;

[Collection("Request Consumer collection")]
public class RestConsumerRequestTest
{
    private readonly RequestChannel channel;
    private readonly IConsumerRequest consumer;
    private readonly IProviderRequest provider;

    private static readonly string BOO = "Boo!";
    private static readonly string BOO_TOPIC = "Boo Topic!";
    private static readonly string EXPIRY = "P1D";

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

        await Assert.ThrowsAsync<IsbmFault>(closeAgain);
    }

    [Fact]
    public async Task PostRequest()
    {
        RequestConsumerSession session = await consumer.OpenSession(channel.Uri);

        _ = await consumer.PostRequest( session.Id, BOO, BOO_TOPIC, EXPIRY );

        await consumer.CloseSession( session.Id );
    }

    [Fact]
    public async Task PostRequestReadResponse()
    {
        var providerSession = await provider.OpenSession(channel.Uri, BOO_TOPIC);
        var consumerSession = await consumer.OpenSession(channel.Uri);

        try
        {
            Assert.Null(await provider.ReadRequest(providerSession.Id));
            await consumer.PostRequest(consumerSession.Id, BOO, BOO_TOPIC, EXPIRY);

            var requestMessage = await provider.ReadRequest(providerSession.Id);
            await provider.RemoveRequest( providerSession.Id );

            Assert.NotNull(requestMessage);
            var requestMessageId = requestMessage?.Id ?? "NotNull asserted above. This is to avoid null warnings.";

            var requestContent = requestMessage?.MessageContent.Deserialise<string>();

            Assert.True(requestContent == BOO);

            var responseMessage = await provider.PostResponse(providerSession.Id, requestMessageId, "Carrots!");

            Assert.NotNull( responseMessage );
            Assert.Contains("Carrots", responseMessage.MessageContent.Deserialise<string>());

            var message = await consumer.ReadResponse( consumerSession.Id, requestMessageId );
            await consumer.RemoveResponse( consumerSession.Id, requestMessageId );

            Assert.NotNull(message);
            var content = message?.MessageContent.Deserialise<string>();

            Assert.NotNull(content);
            Assert.Contains("Carrots!", content);
        }
        finally
        {
            await consumer.CloseSession(consumerSession.Id);
            await provider.CloseSession(providerSession.Id);
        }
    }

    [Fact]
    public async Task ExpirePublication()
    {
        RequestConsumerSession session = await consumer.OpenSession(channel.Uri);

        Message message = await consumer.PostRequest(session.Id, BOO, BOO_TOPIC);

        Assert.NotEmpty(message.Id);

        await consumer.ExpireRequest( session.Id, message.Id );

        await provider.CloseSession( session.Id );
    }
}