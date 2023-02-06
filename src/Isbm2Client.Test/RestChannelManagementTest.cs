using Isbm2Client.Model;
using Isbm2Client.Service;

namespace Isbm2Client.Test;

public class RestChannelManagementTest : IClassFixture<ServiceFixture>
{
    private readonly ServiceFixture fixture;

    public RestChannelManagementTest( ServiceFixture fixture )
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task CreateAndDeleteChannel()
    {
        var manager = new RestChannelManagement( fixture.Config );

        Assert.NotNull( manager );

        var channel = await manager.CreateChannel<RequestChannel>(ServiceFixture.CHANNEL_URI, ServiceFixture.CHANNEL_DESCRIPTION);

        Assert.NotNull(channel);

        await manager.DeleteChannel(ServiceFixture.CHANNEL_URI);
    }
}