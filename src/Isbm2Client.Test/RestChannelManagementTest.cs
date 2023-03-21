using Isbm2Client.Model;
using Isbm2Client.Service;
using Moq;
using RestApi = Isbm2RestClient.Api;
using RestModel = Isbm2RestClient.Model;

namespace Isbm2Client.Test;

public class RestChannelManagementTest
{
    public const string CHANNEL_URI = "/channel/request";
    public const string CHANNEL_DESCRIPTION = "fred";

    private readonly RestModel.Channel apiChannel = new(CHANNEL_URI, RestModel.ChannelType.Request);

    [Fact]
    public async Task CreateChannel()
    {
        var mock = new Mock<RestApi.IChannelManagementApi>();

        mock.Setup(api => api.CreateChannelAsync(It.IsAny<RestModel.Channel>(), 0, default))
            .ReturnsAsync(apiChannel);

        var manager = new RestChannelManagement(mock.Object);

        var channel = await manager.CreateChannel<RequestChannel>(CHANNEL_URI, CHANNEL_DESCRIPTION);

        Assert.IsType<RequestChannel>(channel);
        Assert.Equal(CHANNEL_URI, channel.Uri);
        Assert.Equal(CHANNEL_DESCRIPTION, channel.Description);
    }

    [Fact]
    public async Task DeleteChannel()
    {
        var mock = new Mock<RestApi.IChannelManagementApi>();

        var manager = new RestChannelManagement(mock.Object);

        await manager.DeleteChannel(CHANNEL_URI);

        mock.Verify( api => api.DeleteChannelAsync(CHANNEL_URI, 0, default), Times.Once);
    }
}
