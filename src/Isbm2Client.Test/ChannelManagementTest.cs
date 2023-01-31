using Isbm2Client.Model;
using Isbm2Client.Service;
using static System.Net.WebRequestMethods;

namespace Isbm2Client.Test;

public class ChannelManagementTest
{
    private const string channelUri = "/example/test_channel/publish";

    private readonly Dictionary<string, object> config = new()
    {
        ["endpoint"] = "https://isbm.lab.oiiecosystem.net/rest"
    };

    [Fact]
    public void CreateAndDeleteChannel()
    {
        var manager = ChannelManagement.GetService(config);

        Assert.NotNull( manager );

        var channel = manager.CreateChannel<RequestChannel>(channelUri, "fred");

        Assert.NotNull( channel );

        manager.DeleteChannel(channelUri);
    }
}