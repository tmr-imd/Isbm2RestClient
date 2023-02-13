using Isbm2Client.Interface;
using Isbm2Client.Model;

using RestApi = Isbm2RestClient.Api;
using RestModel = Isbm2RestClient.Model;
using RestClient = Isbm2RestClient.Client;
using Microsoft.Extensions.Options;
using Isbm2RestClient.Model;

namespace Isbm2Client.Service
{
    public class RestConsumerRequest : IConsumerRequest
    {
        private readonly RestApi.ConsumerRequestServiceApi _requestApi;

        public RestConsumerRequest(IOptions<ClientConfig> options)
        {
            RestClient.Configuration apiConfig = new()
            {
                BasePath = options.Value.EndPoint
            };

            // TODO: proper configuration

            _requestApi = new RestApi.ConsumerRequestServiceApi(apiConfig);
        }

        public async Task<RequestConsumerSession> OpenSession(RequestChannel channel) 
        {
            var sessionParams = new RestModel.Session()
            {
                SessionType = RestModel.SessionType.RequestConsumer
            };

            var session = await _requestApi.OpenConsumerRequestSessionAsync( channel.Uri, sessionParams );

            if ( session is null ) throw new Exception( "Uh oh" );

            return new RequestConsumerSession( session.SessionId, sessionParams.ListenerUrl, Array.Empty<string>(), Array.Empty<string>() );
        }

        public async Task<RequestConsumerSession> OpenSession(RequestChannel channel, string listenerUri)
        {
            var sessionParams = new RestModel.Session()
            {
                SessionType = RestModel.SessionType.RequestConsumer,
                ListenerUrl = listenerUri
            };

            var session = await _requestApi.OpenConsumerRequestSessionAsync(channel.Uri, sessionParams);

            if (session is null) throw new Exception("Uh oh");

            return new RequestConsumerSession(session.SessionId, sessionParams.ListenerUrl, Array.Empty<string>(), Array.Empty<string>());
        }

        public async Task CloseSession(RequestConsumerSession session)
        {
            await _requestApi.CloseSessionAsync(session.Id);
        }

        public async Task<RequestMessage> PostRequest<T>( RequestConsumerSession session, T content, IEnumerable<string> topics )
        {
            var inputMessageContent = content switch
            {
                string x => 
                    new RestModel.MessageContent("text/plain", content: new MessageContentContent(x)),

                Dictionary<string, object> x => 
                    new RestModel.MessageContent("application/json", content: new MessageContentContent(x)),

                _ => 
                    throw new ArgumentException("Invalid content found. Must be the following types: Dictionary<string, object>, string")
            };

            var inputMessage = new RestModel.Message( messageContent: inputMessageContent, topics: topics.ToList() );

            var message = await _requestApi.PostRequestAsync( session.Id, inputMessage );

            Model.MessageContent messageContent = content switch
            {
                string x => 
                    new MessageContent<string>(message.MessageId, x),

                Dictionary<string, object> x => 
                    new MessageContent<Dictionary<string, object>>(message.MessageId, x),

                _ => 
                    throw new Exception("Uh oh")
            };

            return new RequestMessage( message.MessageId, messageContent, topics.ToArray(), "" );
        }
    }
}
