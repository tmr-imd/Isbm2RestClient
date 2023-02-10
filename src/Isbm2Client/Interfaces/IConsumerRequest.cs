using Isbm2Client.Model;

namespace Isbm2Client.Interface; 

public interface IConsumerRequest {
    Task<RequestConsumerSession> OpenSession( RequestChannel channel );
    Task<RequestConsumerSession> OpenSession( RequestChannel channel, string listenerUri );
    Task CloseSession(RequestConsumerSession session);

    Task<object> PostRequest<T>( RequestConsumerSession session, T content, IEnumerable<string> topics );
}
