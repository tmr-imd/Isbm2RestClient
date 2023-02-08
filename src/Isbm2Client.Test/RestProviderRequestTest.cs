using Isbm2Client.Model;
using Isbm2Client.Service;
using Xunit;

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
    public async Task OpenSession()
    {
        var request = new RestProviderRequest( fixture.Config );
        var session = await request.OpenProviderRequestSession( fixture.RequestChannel, topics );

        Assert.NotNull( session );
    }
}