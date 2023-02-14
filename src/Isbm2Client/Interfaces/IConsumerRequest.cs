using Isbm2Client.Model;

namespace Isbm2Client.Interface; 

public interface IConsumerRequest {
    Task<RequestConsumerSession> OpenSession( RequestChannel channel );
    Task<RequestConsumerSession> OpenSession( RequestChannel channel, string listenerUri );

    Task<RequestMessage> PostRequest<T>( RequestConsumerSession session, T content, IEnumerable<string> topics );
    Task<ResponseMessage> ReadResponse( RequestConsumerSession session, RequestMessage requestMessage );

    Task CloseSession(RequestConsumerSession session);
}
