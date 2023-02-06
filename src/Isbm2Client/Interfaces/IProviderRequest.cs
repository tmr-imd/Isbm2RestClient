using Isbm2Client.Model;

namespace Isbm2Client.Interface; 

public interface IProviderRequest {
    Task<RequestProviderSession> OpenProviderRequestSession( Channel channel, IEnumerable<string> topics );
}
