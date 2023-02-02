using Isbm2Client.Model;
using Isbm2Client.Service;

namespace Isbm2Client.Test;

public class ChannelManagementTest : IClassFixture<ConfigFixture>
{
    private const string channelUri = "/example/test_channel/publish";

    private readonly ConfigFixture fixture;

    public ChannelManagementTest( ConfigFixture fixture )
    {
        this.fixture = fixture;
    }

    [Fact]
    public void CreateAndDeleteChannel()
    {
        var manager = new RestChannelManagement( fixture.Config );

        Assert.NotNull( manager );

        var channel = manager.CreateChannel<RequestChannel>(channelUri, "fred");

        Assert.NotNull( channel );

        manager.DeleteChannel(channelUri);
    }
}