using Isbm2Client.Model;

namespace Isbm2Client.Interface; 

public interface IChannelManagement {
    Channel CreateChannel<T>(string channelUri, string description) where T : Channel;

    void DeleteChannel(string channelUri);
    Channel GetChannel(string channelUri);
}
