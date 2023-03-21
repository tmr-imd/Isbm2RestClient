using Isbm2Client.Model;

namespace Isbm2Client.Interface; 

public interface IConsumerRequest {
    Task<RequestConsumerSession> OpenSession( string channelUri );
    Task<RequestConsumerSession> OpenSession( string channelUri, string listenerUri );

    Task<RequestMessage> PostRequest<T>( string sessionId, T content, string topic, string? expiry = null ) where T : notnull;
    Task<RequestMessage> PostRequest<T>( string sessionId, T content, IEnumerable<string> topics, string? expiry = null ) where T : notnull;
    Task ExpireRequest( string sessionId, string messageId );
    Task<ResponseMessage?> ReadResponse( string sessionId, string requestMessageId );
    Task RemoveResponse( string sessionId, string requestId );

    Task CloseSession( string sessionId );
}
