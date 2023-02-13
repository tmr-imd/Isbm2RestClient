using Isbm2Client.Model;

namespace Isbm2Client.Interface; 

public interface IProviderRequest {
    Task<RequestProviderSession> OpenSession( RequestChannel channel, IEnumerable<string> topics );

    Task<RequestMessage> ReadRequest(RequestProviderSession session);
    Task<ResponseMessage> PostResponse<T>(RequestProviderSession session, RequestMessage requestMessage, T content);

    Task CloseSession(RequestProviderSession session);
}
