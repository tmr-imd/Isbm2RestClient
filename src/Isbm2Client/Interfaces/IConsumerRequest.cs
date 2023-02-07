using Isbm2Client.Model;

namespace Isbm2Client.Interface; 

public interface IConsumerRequest {
    Task<RequestConsumerSession> OpenConsumerRequestSession( Channel channel );
    Task<RequestConsumerSession> OpenConsumerRequestSession( Channel channel, string listenerUri );
}
