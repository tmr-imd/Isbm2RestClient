using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2Client.Service;
using Isbm2RestClient.Client;

namespace Isbm2Client.Test;

[Collection("Request Channel collection")]
public class RestProviderRequestTest
{
    private readonly RequestChannel channel;
    private readonly IConsumerRequest consumer;
    private readonly IProviderRequest provider;

    private readonly string[] topics = { "topic!" };

    public RestProviderRequestTest( RequestChannelFixture fixture )
    {
        channel = fixture.RequestChannel;
        provider = fixture.Provider;
        consumer = fixture.Consumer;
    }

    [Fact]
    public async Task OpenAndCloseSession()
    {
        var session = await provider.OpenSession( channel.Uri, topics );

        await provider.CloseSession( session.Id );
    }

    [Fact]
    public async Task CantCloseSessionTwice()
    {
        var session = await provider.OpenSession(channel.Uri, topics);

        await provider.CloseSession(session.Id);

        Task closeAgain() => provider.CloseSession(session.Id);

        await Assert.ThrowsAsync<ApiException>( closeAgain );
    }

    [Fact]
    public async Task ReadStringRequest()
    {
        var providerSession = await provider.OpenSession( channel.Uri, topics );
        var consumerSession = await consumer.OpenSession( channel.Uri );

        await consumer.PostRequest(consumerSession.Id, "Yo!", topics);

        try
        {
            var message = await provider.ReadRequest( providerSession.Id );

            Assert.IsType<MessageContent<string>>( message.MessageContent );

            var content = message.MessageContent.GetContent<string>();

            Assert.NotNull(content);
            Assert.Contains("Yo!", content);
        }
        finally
        {
            await consumer.CloseSession( consumerSession.Id );
            await provider.CloseSession( providerSession.Id );
        }
    }

    [Fact]
    public async Task ReadDictionaryRequest()
    {
        var providerSession = await provider.OpenSession( channel.Uri, topics );
        var consumerSession = await consumer.OpenSession( channel.Uri );

        var inputContent = new Dictionary<string, object>()
        {
            { "fred", "barney" },
            { "wilma", "betty" }
        };

        await consumer.PostRequest(consumerSession.Id, inputContent, topics);

        try
        {
            var message = await provider.ReadRequest( providerSession.Id );

            Assert.IsType<MessageContent<Dictionary<string, object>>>( message.MessageContent );

            var content = message.MessageContent.GetContent<Dictionary<string, object>>();

            Assert.NotNull(content);
            Assert.Contains("barney", (string)content["fred"]);
        }
        finally
        {
            await consumer.CloseSession( consumerSession.Id );
            await provider.CloseSession( providerSession.Id );
        }
    }

    [Fact]
    public async Task ReadRequestPostResponse()
    {
        var providerSession = await provider.OpenSession(channel.Uri, topics);
        var consumerSession = await consumer.OpenSession(channel.Uri);

        await consumer.PostRequest(consumerSession.Id, "Yo!", topics);

        try
        {
            var requestMessage = await provider.ReadRequest(providerSession.Id);

            Assert.IsType<MessageContent<string>>(requestMessage.MessageContent);

            var content = requestMessage.MessageContent.GetContent<string>();

            Assert.True( content == "Yo!" );

            var message = await provider.PostResponse( providerSession.Id, requestMessage.Id, "Carrots!" );

            Assert.NotNull( message );
            Assert.Contains( "Carrots", message.MessageContent.GetContent<string>() );
        }
        finally
        {
            await consumer.CloseSession(consumerSession.Id);
            await provider.CloseSession(providerSession.Id);
        }
    }
}