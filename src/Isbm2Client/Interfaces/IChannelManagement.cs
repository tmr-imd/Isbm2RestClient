using Isbm2Client.Model;

namespace Isbm2Client.Service; 

interface IChannelManagement {
    //static IChannelManagement GetService(Dictionary<string, object> config);

    Channel? CreateChannel<T>(string channelUri, string description) where T : Channel, new();

    void DeleteChannel(string channelUri);
    Channel? GetChannel(string channelUri);
}
