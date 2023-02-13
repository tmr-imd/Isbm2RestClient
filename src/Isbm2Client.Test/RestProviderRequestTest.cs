using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2Client.Service;

namespace Isbm2Client.Test;

[Collection("Request Channel collection")]
public class RestProviderRequestTest
{
    private readonly RequestChannelFixture fixture;

    private readonly string[] topics = { "topic!" };

    public RestProviderRequestTest( RequestChannelFixture fixture )
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task OpenAndCloseSession()
    {
        var request = new RestProviderRequest( fixture.Config );
        var session = await request.OpenSession( fixture.RequestChannel, topics );

        Assert.NotNull( session );

        await request.CloseSession( session );
    }

    [Fact]
    public async Task ReadRequest()
    {
        var channel = fixture.RequestChannel;
        var consumer = fixture.Consumer;
        var provider = fixture.Provider;

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
}