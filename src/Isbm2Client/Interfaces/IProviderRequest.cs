using Isbm2Client.Model;

namespace Isbm2Client.Interface; 

public interface IProviderRequest {
    Task<RequestProviderSession> OpenSession( string channelUri, string topic );
    Task<RequestProviderSession> OpenSession( string channelUrl, IEnumerable<string> topics );

    Task<RequestMessage> ReadRequest(string sessionId);
    Task RemoveRequest(string sessionId);
    Task<ResponseMessage> PostResponse<T>(string sessionId, string requestMessageId, T content) where T : notnull;

    Task CloseSession(string sessionId);
}
