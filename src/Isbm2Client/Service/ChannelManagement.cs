using Isbm2Client.Model;

namespace Isbm2Client {
namespace Service {

public class ChannelManagement {
    public Dictionary<string, object> ClientConfig { get; private set; }

    private Isbm2RestClient.Api.ChannelManagementApi _channelApi;

    public static ChannelManagement GetService(Dictionary<string, object> config) {
        return new ChannelManagement(config);
    }

    protected ChannelManagement(Dictionary<string, object> config) {
        ClientConfig = config;
        var apiConfig = new Isbm2RestClient.Client.Configuration();
        apiConfig.BasePath = config.GetValueOrDefault("endpoint")?.ToString() ?? "http://localhost/rest";
        // TODO: proper configuration
        _channelApi = new Isbm2RestClient.Api.ChannelManagementApi();
    } 

    public Channel CreateChannel<T>(string channelUri, string description) where T : Channel, new() {
        // TODO: error handling
        var channelType = typeof(T) == typeof(Channel) ? Isbm2RestClient.Model.ChannelType.Publication : Isbm2RestClient.Model.ChannelType.Request;
        var toBeChannel = new Isbm2RestClient.Model.Channel(channelUri, channelType, description);
        Console.WriteLine("    {0}", toBeChannel.ToJson().ReplaceLineEndings("\n    "));
        var createdChannel = _channelApi.CreateChannel(toBeChannel);
        return Activator.CreateInstance(typeof(T), createdChannel.Uri, createdChannel.Description) as T;
    }

    public void DeleteChannel(string channelUri) {
        // TODO: error handling
        Console.WriteLine("Deleting channel {0}", channelUri);
        _channelApi.DeleteChannel(channelUri);
    }

    public Channel? GetChannel(string channelUri) {
        // TODO: error handling
        var channel = _channelApi.GetChannel(channelUri);
        var type = channel.ChannelType == Isbm2RestClient.Model.ChannelType.Publication ? typeof(PublicationChannel) : typeof(RequestChannel);
        return Activator.CreateInstance(type, channel.Uri, channel.Description) as Channel;
    }
}

} // Service
} // Isbm2Client