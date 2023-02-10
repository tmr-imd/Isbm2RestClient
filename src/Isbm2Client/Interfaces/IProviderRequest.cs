using Isbm2Client.Model;

namespace Isbm2Client.Interface; 

public interface IProviderRequest {
    Task<RequestProviderSession> OpenProviderRequestSession( RequestChannel channel, IEnumerable<string> topics );
    Task CloseProviderRequestSession( RequestProviderSession session );

    Task<object> ReadRequest(RequestProviderSession session);
}
