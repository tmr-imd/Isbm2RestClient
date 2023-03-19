using Isbm2Client.Model;

namespace Isbm2Client.Interface; 

public interface IConsumerPublication {
    Task<PublicationConsumerSession> OpenSession( string channelUri, string topic, string? listenerUri = null );
    Task<PublicationConsumerSession> OpenSession( string channelUri, IEnumerable<string> topics, string? listenerUri = null );

    Task<PublicationMessage?> ReadPublication( string sessionId );
    Task RemovePublication( string sessionId );

    Task CloseSession( string sessionId );
}
