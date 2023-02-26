using Isbm2Client.Interface;
using Isbm2Client.Model;

using RestApi = Isbm2RestClient.Api;
using RestModel = Isbm2RestClient.Model;
using RestClient = Isbm2RestClient.Client;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Isbm2Client.Service
{
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

            MessageContent messageContent = response.MessageContent.Content.ActualInstance switch
            {
                string x => 
                    new MessageString( x),

                JsonDocument x => 
                    new MessageJsonDocument( x ),

                _ => 
                    throw new Exception( "Uh oh" )
            };

            return new RequestMessage( response.MessageId, messageContent, response.Topics.ToArray(), "" );
        }

        public async Task RemoveRequest(string sessionId)
        {
            await _requestApi.RemoveRequestAsync( sessionId );
        }

        public async Task<ResponseMessage> PostResponse<T>( string sessionId, string requestMessageId, T content ) where T : notnull
        {
            var inputMessageContent = content switch
            {
                string x =>
                    new RestModel.MessageContent("text/plain", content: new RestModel.MessageContentContent(x)),

                JsonDocument x =>
                    new RestModel.MessageContent(
                        "application/json", 
                        content: new RestModel.MessageContentContent(x)
                    ),

                T x =>
                    new RestModel.MessageContent(
                        "application/json",
                        content: new RestModel.MessageContentContent(JsonSerializer.SerializeToDocument(x))
                    ),

                _ =>
                    throw new ArgumentException("Invalid content found. Must be the following types: Dictionary<string, object>, string")
            };

            var inputMessage = new RestModel.Message( messageContent: inputMessageContent );

            var message = await _requestApi.PostResponseAsync( sessionId, requestMessageId, inputMessage );

            Model.MessageContent messageContent = content switch
            {
                string x => 
                    new MessageString(x),

                JsonDocument x => 
                    new MessageJsonDocument(x),

                T x =>
                    new MessageJsonDocument(JsonSerializer.SerializeToDocument(x)),

                _ => 
                    throw new Exception("Uh oh")
            };

            return new ResponseMessage( message.MessageId, messageContent, Array.Empty<string>(), "" );
        }

        public async Task CloseSession( string sessionId )
        {
            await _requestApi.CloseSessionAsync( sessionId );
        }
    }
}
