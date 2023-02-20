using Isbm2Client.Model;

namespace Isbm2Client.Interface; 

public interface IConsumerRequest {
    Task<RequestConsumerSession> OpenSession( string channelUri );
    Task<RequestConsumerSession> OpenSession( string channelUri, string listenerUri );

    Task<RequestMessage> PostRequest<T>( string sessionId, T content, string topic ) where T : notnull;
    Task<RequestMessage> PostRequest<T>( string sessionId, T content, IEnumerable<string> topics ) where T : notnull;
    Task<ResponseMessage> ReadResponse( string sessionId, string requestMessageId );

    Task CloseSession( string sessionId );
}
