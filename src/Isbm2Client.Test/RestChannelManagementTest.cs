using Isbm2Client.Model;
using Isbm2Client.Service;

namespace Isbm2Client.Test;

public class RestChannelManagementTest : IClassFixture<ConfigFixture>
{
    public const string CHANNEL_URI = "/pittsh/test/general";
    public const string CHANNEL_DESCRIPTION = "fred";

    private readonly ConfigFixture fixture;

    public RestChannelManagementTest( ConfigFixture fixture )
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task CreateAndDeleteChannel()
    {
        var manager = new RestChannelManagement( fixture.Config );

        Assert.NotNull( manager );

        var channel = await manager.CreateChannel<RequestChannel>(CHANNEL_URI, CHANNEL_DESCRIPTION);

        Assert.NotNull(channel);

        await manager.DeleteChannel(CHANNEL_URI);
    }
}