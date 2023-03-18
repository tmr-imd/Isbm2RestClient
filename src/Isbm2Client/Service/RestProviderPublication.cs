using Isbm2Client.Interface;
using Isbm2Client.Model;

using RestApi = Isbm2RestClient.Api;
using RestModel = Isbm2RestClient.Model;
using RestClient = Isbm2RestClient.Client;
using Microsoft.Extensions.Options;
using Isbm2Client.Extensions;

namespace Isbm2Client.Service;

public class RestProviderPublication : IProviderPublication
{
    private readonly RestApi.ProviderPublicationServiceApi _publicationApi;

    public RestProviderPublication(IOptions<ClientConfig> options)
    {
        RestClient.Configuration apiConfig = new()
        {
            BasePath = options.Value.EndPoint
        };

        // TODO: proper configuration

        _publicationApi = new RestApi.ProviderPublicationServiceApi(apiConfig);
        _publicationApi.ExceptionFactory = IsbmFaultRestExtensions.IsbmFaultFactory;
    }

    public async Task<PublicationProviderSession> OpenSession(string channelUri)
    {
        var session = await _publicationApi.OpenPublicationSessionAsync( channelUri );

        if ( session is null ) throw new Exception( "Uh oh" );

        return new PublicationProviderSession( session.SessionId );
    }

    public async Task<PublicationMessage> PostPublication<T>(string sessionId, T content, string topic, string? expiry = null) where T : notnull
    {
        var topics = new[] { topic };

        return await PostPublication<T>(sessionId, content, topics, expiry);
    }

    public async Task<PublicationMessage> PostPublication<T>(string sessionId, T content, IEnumerable<string> topics, string? expiry = null) where T : notnull
    {
        var messageContent = Model.MessageContent.From(content);

        var restMessage = new RestModel.Message
        ( 
            messageContent: messageContent.ToRestMessageContent(), 
            topics: topics.ToList(),
            expiry: expiry
        );

        var message = await _publicationApi.PostPublicationAsync( sessionId, restMessage );

        return new PublicationMessage( message.MessageId, messageContent, topics.ToArray(), expiry );
    }

    public async Task ExpirePublication(string sessionId, string messageId)
    {
        await _publicationApi.ExpirePublicationAsync( sessionId, messageId );
    }

    public async Task CloseSession(string sessionId)
    {
        await _publicationApi.CloseSessionAsync(sessionId);
    }
}
