using Isbm2Client.Model;

namespace Isbm2Client.Interface; 

public interface IProviderPublication {
    Task<PublicationProviderSession> OpenSession( string channelUri );

    Task<PublicationMessage> PostPublication<T>( string sessionId, T content, string topic ) where T : notnull;
    Task<PublicationMessage> PostPublication<T>( string sessionId, T content, IEnumerable<string> topics ) where T : notnull;

    Task ExpirePublication( string sessionId, string messageId );

    Task CloseSession( string sessionId );
}
