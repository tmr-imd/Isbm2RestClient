using Isbm2Client.Model;

using RestApi = Isbm2RestClient.Api;
using RestModel = Isbm2RestClient.Model;
using RestClient = Isbm2RestClient.Client;

namespace Isbm2Client.Service; 

public class ChannelManagement : IChannelManagement
{
    //public Dictionary<string, object> ClientConfig { get; private set; }

    private readonly RestApi.ChannelManagementApi _channelApi;

    public static ChannelManagement GetService(Dictionary<string, object> config) {
        return new ChannelManagement(config);
    }

    protected ChannelManagement(Dictionary<string, object> config) {
        //ClientConfig = config;

        RestClient.Configuration apiConfig = new()
        {
            BasePath = config.GetValueOrDefault("endpoint")?.ToString() ?? "http://localhost/rest"
        };

        // TODO: proper configuration

        _channelApi = new RestApi.ChannelManagementApi( apiConfig );
    } 

    public Channel? CreateChannel<T>(string channelUri, string description) where T : Channel {
        // TODO: error handling
        var channelType = typeof(T) == typeof(PublicationChannel) ? RestModel.ChannelType.Publication : RestModel.ChannelType.Request;
        var toBeChannel = new RestModel.Channel(channelUri, channelType, description);
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
        var type = channel.ChannelType == RestModel.ChannelType.Publication ? typeof(PublicationChannel) : typeof(RequestChannel);
        return Activator.CreateInstance(type, channel.Uri, channel.Description) as Channel;
    }
}
