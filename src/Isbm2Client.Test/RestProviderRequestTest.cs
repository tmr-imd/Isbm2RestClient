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
        var session = await request.OpenProviderRequestSession( fixture.RequestChannel, topics );

        Assert.NotNull( session );

        await request.CloseProviderRequestSession( session );
    }

    [Fact]
    public async Task ReadRequest()
    {
        var channel = fixture.RequestChannel;
        var consumer = fixture.Consumer;
        var provider = fixture.Provider;

        var session = await provider.OpenProviderRequestSession( channel, topics );

        var consumerSession = await consumer.OpenConsumerRequestSession( channel );

        await consumer.PostRequest(consumerSession, "Yo!", topics);

        Assert.NotNull(session);

        try
        {
            var message = await provider.ReadRequest( session );

            Assert.NotNull(message);
        }
        finally
        {
            await consumer.CloseConsumerRequestSession( consumerSession );
            await provider.CloseProviderRequestSession( session );
        }
    }
}