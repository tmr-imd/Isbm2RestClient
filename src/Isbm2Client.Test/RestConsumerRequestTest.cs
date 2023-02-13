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
    public async Task OpenAndCloseSession()
    {
        var request = new RestConsumerRequest( fixture.Config );
        var session = await request.OpenSession( fixture.RequestChannel );

        Assert.NotNull( session );

        await request.CloseSession( session );
    }

    [Fact]
    public async Task PostRequest()
    {
        var request = new RestConsumerRequest(fixture.Config);
        var session = await request.OpenSession(fixture.RequestChannel);

        Assert.NotNull(session);

        var message = await request.PostRequest( session, "Yo!", new[] { "yo" } );

        Assert.NotNull( message );

        await request.CloseSession( session );
    }
}