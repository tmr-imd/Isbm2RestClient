using Isbm2Client.Interface;
using Isbm2Client.Model;

using RestApi = Isbm2RestClient.Api;
using RestModel = Isbm2RestClient.Model;
using RestClient = Isbm2RestClient.Client;
using Microsoft.Extensions.Options;
using Isbm2Client.Extensions;

namespace Isbm2Client.Service;

public class RestProviderRequest : IProviderRequest
{
    private readonly RestApi.ProviderRequestServiceApi _requestApi;

    public RestProviderRequest(IOptions<ClientConfig> options)
    {
        RestClient.Configuration apiConfig = new()
        {
            BasePath = options.Value.EndPoint
        };

        // TODO: proper configuration

        _requestApi = new RestApi.ProviderRequestServiceApi(apiConfig);
    }

    public Task<RequestProviderSession> OpenSession(string channelUrl, string topic)
    {
        var topics = new[] { topic };

        return OpenSession( channelUrl, topics );
    }

    public async Task<RequestProviderSession> OpenSession(string channelUrl, IEnumerable<string> topics) 
    {
        var sessionParams = new RestModel.Session()
        {
            SessionType = RestModel.SessionType.RequestProvider,
            Topics = topics.ToList(),
            FilterExpressions = new List<RestModel.FilterExpression>()
        };

        var session = await _requestApi.OpenProviderRequestSessionAsync( channelUrl, sessionParams );

        if ( session is null ) throw new Exception( "Uh oh" );

        return new RequestProviderSession( session.SessionId, null, sessionParams.Topics.ToArray(), Array.Empty<string>() );
    }

    public async Task<RequestMessage> ReadRequest(string sessionId)
    {
        var response = await _requestApi.ReadRequestAsync( sessionId );

        var content = response.MessageContent.Content.ActualInstance;
        var messageContent = MessageContent.From( content );

        return new RequestMessage( response.MessageId, messageContent, response.Topics.ToArray(), "" );
    }

    public async Task RemoveRequest(string sessionId)
    {
        await _requestApi.RemoveRequestAsync( sessionId );
    }

    public async Task<ResponseMessage> PostResponse<T>( string sessionId, string requestMessageId, T content ) where T : notnull
    {
        var messageContent = MessageContent.From(content);
        var restMessageContent = messageContent.ToRestMessageContent();
        var restMessage = new RestModel.Message( messageContent: restMessageContent );

        var message = await _requestApi.PostResponseAsync( sessionId, requestMessageId, restMessage );

        return new ResponseMessage( message.MessageId, messageContent, requestMessageId );
    }

    public async Task CloseSession( string sessionId )
    {
        await _requestApi.CloseSessionAsync( sessionId );
    }
}
