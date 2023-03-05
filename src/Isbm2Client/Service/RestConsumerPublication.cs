using Isbm2Client.Interface;
using Isbm2Client.Model;

using RestApi = Isbm2RestClient.Api;
using RestModel = Isbm2RestClient.Model;
using RestClient = Isbm2RestClient.Client;
using Microsoft.Extensions.Options;
using Isbm2Client.Extensions;

namespace Isbm2Client.Service;

public class RestConsumerPublication : IConsumerPublication
{
    private readonly RestApi.ConsumerPublicationServiceApi _publicationApi;

    public RestConsumerPublication(IOptions<ClientConfig> options)
    {
        RestClient.Configuration apiConfig = new()
        {
            BasePath = options.Value.EndPoint
        };

        // TODO: proper configuration

        _publicationApi = new RestApi.ConsumerPublicationServiceApi(apiConfig);
    }

    public async Task<PublicationConsumerSession> OpenSession(string channelUri, string topic, string? listenerUri = null)
    {
        var topics = new[] { topic };

        return await OpenSession( channelUri, topics, listenerUri );
    }

    public async Task<PublicationConsumerSession> OpenSession(string channelUri, IEnumerable<string> topics, string? listenerUri = null)
    {
        var sessionParams = new RestModel.Session()
        {
            SessionType = RestModel.SessionType.PublicationConsumer,
            Topics = topics.ToList(),
            ListenerUrl = listenerUri,
        };

        var session = await _publicationApi.OpenSubscriptionSessionAsync( channelUri, sessionParams );

        if ( session is null ) throw new Exception( "Uh oh" );

        return new PublicationConsumerSession( session.SessionId, sessionParams.ListenerUrl, sessionParams.Topics.ToArray(), Array.Empty<string>() );
    }

    public async Task<PublicationMessage> ReadPublication(string sessionId)
    {
        var response = await _publicationApi.ReadPublicationAsync( sessionId );
        var content = response.MessageContent.Content.ActualInstance;
        var messageContent = Model.MessageContent.From( content );

        return new PublicationMessage( response.MessageId, messageContent, response.Topics.ToArray(), null );
    }

    public async Task RemovePublication(string sessionId)
    {
        await _publicationApi.RemovePublicationAsync(sessionId);
    }

    public async Task CloseSession(string sessionId)
    {
        await _publicationApi.CloseSessionAsync(sessionId);
    }
}