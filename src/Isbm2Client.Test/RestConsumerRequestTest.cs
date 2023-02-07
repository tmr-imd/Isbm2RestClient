using Isbm2Client.Model;
using Isbm2Client.Service;

namespace Isbm2Client.Test;

public class RestConsumerRequestTest : IClassFixture<ServiceFixture>, IAsyncLifetime
{
    public const string CHANNEL_URI = "/example/test/fixture/publish";
    public const string CHANNEL_DESCRIPTION = "fred";

    private readonly ServiceFixture fixture;

    private RequestChannel requestChannel = null!;

    public RestConsumerRequestTest( ServiceFixture fixture )
    {
        this.fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        try
        {
            requestChannel = await fixture.ChannelManagement.CreateChannel<RequestChannel>( CHANNEL_URI, CHANNEL_DESCRIPTION );

        } catch
        {
            var channel = await fixture.ChannelManagement.GetChannel( CHANNEL_URI );
            requestChannel = (RequestChannel)channel;
        }
    }

    public async Task DisposeAsync()
    {
        await fixture.ChannelManagement.DeleteChannel( CHANNEL_URI );
    }

    [Fact]
    public async Task OpenSession()
    {
        var request = new RestConsumerRequest( fixture.Config );
        var session = await request.OpenConsumerRequestSession(requestChannel);

        Assert.NotNull( session );
    }
}