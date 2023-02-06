using Isbm2Client.Model;
using Isbm2Client.Service;
using Xunit;

namespace Isbm2Client.Test;

public class RestProviderRequestTest : IClassFixture<ServiceFixture>, IAsyncLifetime
{
    public const string CHANNEL_URI = "/example/test/fixture/publish";
    public const string CHANNEL_DESCRIPTION = "fred";

    private readonly ServiceFixture fixture;

    private readonly string[] topics = { "topic!" };
    private readonly string[] filters = Array.Empty<string>();

    private RequestChannel requestChannel = null!;

    public RestProviderRequestTest( ServiceFixture fixture )
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
        var request = new RestProviderRequest( fixture.Config );
        var session = await request.OpenProviderRequestSession(requestChannel, topics);

        Assert.NotNull( session );
    }
}