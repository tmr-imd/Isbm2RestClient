using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2Client.Service;

namespace Isbm2Client.Test;

[Collection("Request Channel collection")]
public class RestProviderRequestTest
{
    private readonly RequestChannelFixture fixture;

    private readonly RequestChannel channel;
    private readonly IConsumerRequest consumer;
    private readonly IProviderRequest provider;

    private readonly string[] topics = { "topic!" };

    public RestProviderRequestTest( RequestChannelFixture fixture )
    {
        this.fixture = fixture;

        channel = fixture.RequestChannel;
        provider = fixture.Provider;
        consumer = fixture.Consumer;
    }

    [Fact]
    public async Task OpenAndCloseSession()
    {
        var request = new RestProviderRequest( fixture.Config );
        var session = await request.OpenSession( channel, topics );

        Assert.NotNull( session );

        await request.CloseSession( session );
    }

    [Fact]
    public async Task ReadStringRequest()
    {
        var providerSession = await provider.OpenSession( channel, topics );
        var consumerSession = await consumer.OpenSession( channel );

        await consumer.PostRequest(consumerSession, "Yo!", topics);

        try
        {
            var message = await provider.ReadRequest( providerSession );

            Assert.IsType<MessageContent<string>>( message.MessageContent );

            var content = message.MessageContent.GetContent<string>();

            Assert.NotNull(content);
            Assert.Contains("Yo!", content);
        }
        finally
        {
            await consumer.CloseSession( consumerSession );
            await provider.CloseSession( providerSession );
        }
    }

    [Fact]
    public async Task ReadDictionaryRequest()
    {
        var providerSession = await provider.OpenSession( channel, topics );
        var consumerSession = await consumer.OpenSession( channel );

        var inputContent = new Dictionary<string, object>()
        {
            { "fred", "barney" },
            { "wilma", "betty" }
        };

        await consumer.PostRequest(consumerSession, inputContent, topics);

        try
        {
            var message = await provider.ReadRequest( providerSession );

            Assert.IsType<MessageContent<Dictionary<string, object>>>( message.MessageContent );

            var content = message.MessageContent.GetContent<Dictionary<string, object>>();

            Assert.NotNull(content);
            Assert.Contains("barney", (string)content["fred"]);
        }
        finally
        {
            await consumer.CloseSession( consumerSession );
            await provider.CloseSession( providerSession );
        }
    }

    [Fact]
    public async Task ReadRequestPostResponse()
    {
        var providerSession = await provider.OpenSession(channel, topics);
        var consumerSession = await consumer.OpenSession(channel);

        await consumer.PostRequest(consumerSession, "Yo!", topics);

        try
        {
            var requestMessage = await provider.ReadRequest(providerSession);

            Assert.IsType<MessageContent<string>>(requestMessage.MessageContent);

            var content = requestMessage.MessageContent.GetContent<string>();

            Assert.True( content == "Yo!" );

            var message = await provider.PostResponse( providerSession, requestMessage, "Carrots!" );

            Assert.NotNull( message );
            Assert.Contains( "Carrots", message.MessageContent.GetContent<string>() );
        }
        finally
        {
            await consumer.CloseSession(consumerSession);
            await provider.CloseSession(providerSession);
        }
    }
}