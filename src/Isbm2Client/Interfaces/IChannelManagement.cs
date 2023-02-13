using Isbm2Client.Model;

namespace Isbm2Client.Interface; 

public interface IChannelManagement {
    Task<T> CreateChannel<T>(string channelUri, string description) where T : Channel;

    Task DeleteChannel(string channelUri);
    Task<Channel> GetChannel(string channelUri);
}
