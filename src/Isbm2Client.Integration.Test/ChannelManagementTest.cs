using Isbm2Client.Model;
using Isbm2Client.Service;
using Isbm2RestClient.Api;

namespace Isbm2Client.Integration.Test;

public class ChannelManagementTest : IClassFixture<ConfigFixture>
{
    public const string CHANNEL_URI = "/pittsh/test/general";
    public const string CHANNEL_DESCRIPTION = "fred";

    private readonly ConfigFixture fixture;

    public ChannelManagementTest( ConfigFixture fixture )
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task CreateAndDeleteChannel()
    {

        var channelApi = new ChannelManagementApi( fixture.ApiConfig );
        var manager = new RestChannelManagement( channelApi );

        Assert.NotNull( manager );

        var channel = await manager.CreateChannel<RequestChannel>(CHANNEL_URI, CHANNEL_DESCRIPTION);

        Assert.NotNull(channel);

        await manager.DeleteChannel(CHANNEL_URI);
    }
}