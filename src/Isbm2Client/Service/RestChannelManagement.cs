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

    public async Task<T> CreateChannel<T>(string channelUri, string description) where T : Channel {
        // TODO: error handling
        var channelType = typeof(T) == typeof(PublicationChannel) ? RestModel.ChannelType.Publication : RestModel.ChannelType.Request;
        var toBeChannel = new RestModel.Channel(channelUri, channelType, description);
        Console.WriteLine("    {0}", toBeChannel.ToJson().ReplaceLineEndings("\n    "));

        var createdChannel = await _channelApi.CreateChannelAsync(toBeChannel);
        var instance = Activator.CreateInstance(typeof(T), channelUri, createdChannel.Description) as T;

        if ( instance is null ) throw new Exception("Uh oh");

        return instance;
    }

    public async Task DeleteChannel(string channelUri) {
        // TODO: error handling
        Console.WriteLine("Deleting channel {0}", channelUri);
        await _channelApi.DeleteChannelAsync(channelUri);
    }

    public async Task<Channel> GetChannel(string channelUri) {
        // TODO: error handling
        var channel = await _channelApi.GetChannelAsync(channelUri);
        var type = channel.ChannelType == RestModel.ChannelType.Publication ? typeof(PublicationChannel) : typeof(RequestChannel);
        var instance = Activator.CreateInstance(type, channelUri, channel.Description) as Channel;

        if ( instance is null ) throw new Exception( "Uh oh" );

        return instance;
    }
}
