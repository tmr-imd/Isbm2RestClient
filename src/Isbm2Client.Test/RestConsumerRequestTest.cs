using Isbm2Client.Model;
using Isbm2Client.Service;

namespace Isbm2Client.Test;

[Collection("Request Channel collection")]
public class RestConsumerRequestTest
{
    private readonly RequestChannelFixture fixture;

    public RestConsumerRequestTest( RequestChannelFixture fixture )
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task OpenSession()
    {
        var request = new RestConsumerRequest( fixture.Config );
        var session = await request.OpenConsumerRequestSession( fixture.RequestChannel );

        Assert.NotNull( session );
    }
}