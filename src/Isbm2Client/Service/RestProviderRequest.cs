using Isbm2Client.Interface;
using Isbm2Client.Model;

using RestApi = Isbm2RestClient.Api;
using RestModel = Isbm2RestClient.Model;
using Isbm2Client.Extensions;

namespace Isbm2Client.Service;

public class RestProviderRequest : AbstractRestService, IProviderRequest
{
    private readonly RestApi.IProviderRequestServiceApi _requestApi;

    public RestProviderRequest(RestApi.IProviderRequestServiceApi requestApi)
    {
        _requestApi = requestApi;
        _requestApi.ExceptionFactory = IsbmFaultRestExtensions.IsbmFaultFactory;
    }

    public Task<RequestProviderSession> OpenSession(string channelUrl, string topic)
    {
        var topics = new[] { topic };

        return OpenSession( channelUrl, topics );
    }

    public Task<RequestProviderSession> OpenSession(string channelUrl, string topic, string listenerUrl)
    {
        var topics = new[] { topic };

        return OpenSession(channelUrl, topics, listenerUrl);
    }

    public Task<RequestProviderSession> OpenSession(string channelUrl, IEnumerable<string> topics)
    {
        return OpenSession( channelUrl, topics, "" );
    }

    public async Task<RequestProviderSession> OpenSession(string channelUrl, IEnumerable<string> topics, string listenerUrl) 
    {
        var sessionParams = new RestModel.Session()
        {
            SessionType = RestModel.SessionType.RequestProvider,
            Topics = topics.ToList(),
            FilterExpressions = new List<RestModel.FilterExpression>()
        };

        if ( !string.IsNullOrEmpty(listenerUrl) )
        {
            sessionParams.ListenerUrl = listenerUrl;
        }

        var session = await ProtectedApiCallAsync( async () => await _requestApi.OpenProviderRequestSessionAsync( channelUrl, sessionParams ) );

        return new RequestProviderSession( session.SessionId, null, sessionParams.Topics.ToArray(), Array.Empty<string>() );
    }

    public async Task<RequestMessage?> ReadRequest(string sessionId)
    {
        var response = await ProtectedApiCallAsync( async () => await _requestApi.ReadRequestAsync( sessionId ) );
        if (response.NotFound()) return null;

        var content = response.MessageContent.Content.ActualInstance;
        var messageContent = MessageContent.From( content, response.MessageContent.MediaType );

        return new RequestMessage( response.MessageId, messageContent, response.Topics.ToArray(), "" );
    }

    public async Task RemoveRequest(string sessionId)
    {
        await ProtectedApiCallAsync( async () => await _requestApi.RemoveRequestAsync( sessionId ) );
    }

    public async Task<ResponseMessage> PostResponse<T>( string sessionId, string requestMessageId, T content ) where T : notnull
    {
        var messageContent = MessageContent.From(content);
        var restMessageContent = messageContent.ToRestMessageContent();
        var restMessage = new RestModel.Message( messageContent: restMessageContent );

        var message = await ProtectedApiCallAsync( async () => await _requestApi.PostResponseAsync( sessionId, requestMessageId, restMessage ) );

        return new ResponseMessage( message.MessageId, messageContent, requestMessageId );
    }

    public async Task CloseSession( string sessionId )
    {
        await ProtectedApiCallAsync( async () => await _requestApi.CloseSessionAsync( sessionId ) );
    }
}
