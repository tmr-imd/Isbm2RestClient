using Isbm2Client.Model;

namespace Isbm2Client.Interface; 

public interface IConsumerRequest {
    Task<RequestConsumerSession> OpenConsumerRequestSession( RequestChannel channel );
    Task<RequestConsumerSession> OpenConsumerRequestSession( RequestChannel channel, string listenerUri );
    Task CloseConsumerRequestSession(RequestConsumerSession session);

    Task<object> PostRequest<T>( RequestConsumerSession session, T content, IEnumerable<string> topics );
}
