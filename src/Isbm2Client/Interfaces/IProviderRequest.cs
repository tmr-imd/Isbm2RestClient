using Isbm2Client.Model;

namespace Isbm2Client.Interface; 

public interface IProviderRequest {
    Session OpenProviderRequestSession( string description, Channel channel, string[] topics, Uri listenerUrl, string[] filterExpressions);
}
