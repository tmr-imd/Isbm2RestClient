using Isbm2Client.Model;

namespace Isbm2Client.Interface; 

public interface IProviderRequest {
    Task<RequestProviderSession> OpenSession( RequestChannel channel, IEnumerable<string> topics );
    Task CloseSession( RequestProviderSession session );

    Task<object> ReadRequest(RequestProviderSession session);
}
