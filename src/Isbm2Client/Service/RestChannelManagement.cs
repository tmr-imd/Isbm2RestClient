using Isbm2Client.Model;
using Isbm2Client.Interface;
using Isbm2Client.Extensions;

using RestApi = Isbm2RestClient.Api;
using RestModel = Isbm2RestClient.Model;

namespace Isbm2Client.Service;

public class RestChannelManagement : AbstractRestService, IChannelManagement
{
    private readonly RestApi.IChannelManagementApi _channelApi;

    public RestChannelManagement( RestApi.IChannelManagementApi channelApi ) 
    {
        _channelApi = channelApi;
        _channelApi.ExceptionFactory = IsbmFaultRestExtensions.IsbmFaultFactory;
    } 

    public async Task<T> CreateChannel<T>(string channelUri, string description) where T : Channel 
    {
        // TODO: error handling
        var channelType = typeof(T) == typeof(PublicationChannel) ? RestModel.ChannelType.Publication : RestModel.ChannelType.Request;
        var toBeChannel = new RestModel.Channel(channelUri, channelType, description);
        Console.WriteLine("    {0}", toBeChannel.ToJson().ReplaceLineEndings("\n    ")); // TODO: Convert to Logging

        var createdChannel = await ProtectedApiCallAsync( async () => await _channelApi.CreateChannelAsync(toBeChannel));
        var instance = CreateInstance<T>(typeof(T), channelUri, createdChannel.Description);

        return instance;
    }

    public async Task DeleteChannel(string channelUri) {
        Console.WriteLine("Deleting channel {0}", channelUri); // TODO: Convert to logging
        await ProtectedApiCallAsync( async () => await _channelApi.DeleteChannelAsync(channelUri) );
    }

    public async Task<Channel> GetChannel(string channelUri) {
        var channel = await ProtectedApiCallAsync( async () => await _channelApi.GetChannelAsync(channelUri) );
        var type = channel.ChannelType == RestModel.ChannelType.Publication ? typeof(PublicationChannel) : typeof(RequestChannel);
        var instance = CreateInstance<Channel>(type, channelUri, channel.Description);

        return instance;
    }
}
