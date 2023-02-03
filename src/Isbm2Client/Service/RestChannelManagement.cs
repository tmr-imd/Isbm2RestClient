using Isbm2Client.Model;
using Isbm2Client.Interface;

using RestApi = Isbm2RestClient.Api;
using RestModel = Isbm2RestClient.Model;
using RestClient = Isbm2RestClient.Client;
using Microsoft.Extensions.Options;

namespace Isbm2Client.Service; 

public class RestChannelManagement : IChannelManagement
{
    private readonly RestApi.ChannelManagementApi _channelApi;

    public RestChannelManagement(IOptions<ClientConfig> options) {
        RestClient.Configuration apiConfig = new()
        {
            BasePath = options.Value.EndPoint
        };

        // TODO: proper configuration

        _channelApi = new RestApi.ChannelManagementApi( apiConfig );
    } 

    public Channel CreateChannel<T>(string channelUri, string description) where T : Channel {
        // TODO: error handling
        var channelType = typeof(T) == typeof(PublicationChannel) ? RestModel.ChannelType.Publication : RestModel.ChannelType.Request;
        var toBeChannel = new RestModel.Channel(channelUri, channelType, description);
        Console.WriteLine("    {0}", toBeChannel.ToJson().ReplaceLineEndings("\n    "));

        var createdChannel = _channelApi.CreateChannel(toBeChannel);
        var instance = Activator.CreateInstance(typeof(T), createdChannel.Uri, createdChannel.Description) as T;

        if ( instance is null ) throw new Exception("Uh oh");

        return instance;
    }

    public void DeleteChannel(string channelUri) {
        // TODO: error handling
        Console.WriteLine("Deleting channel {0}", channelUri);
        _channelApi.DeleteChannel(channelUri);
    }

    public Channel GetChannel(string channelUri) {
        // TODO: error handling
        var channel = _channelApi.GetChannel(channelUri);
        var type = channel.ChannelType == RestModel.ChannelType.Publication ? typeof(PublicationChannel) : typeof(RequestChannel);
        var instance = Activator.CreateInstance(type, channel.Uri, channel.Description) as Channel;

        if ( instance is null ) throw new Exception( "Uh oh" );

        return instance;
    }
}
