using Isbm2Client.Interface;
using Isbm2Client.Model;

using RestApi = Isbm2RestClient.Api;
using RestModel = Isbm2RestClient.Model;
using RestClient = Isbm2RestClient.Client;
using Microsoft.Extensions.Options;
using Isbm2RestClient.Model;

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

        public async Task<RequestProviderSession> OpenProviderRequestSession(RequestChannel channel, IEnumerable<string> topics) 
        {
            var sessionParams = new RestModel.Session()
            {
                SessionType = RestModel.SessionType.RequestProvider,
                Topics = topics.ToList(),
                FilterExpressions = new List<RestModel.FilterExpression>()
            };

            var session = await _requestApi.OpenProviderRequestSessionAsync( channel.Uri, sessionParams );

            if ( session is null ) throw new Exception( "Uh oh" );

            return new RequestProviderSession( session.SessionId, sessionParams.ListenerUrl, sessionParams.Topics.ToArray(), Array.Empty<string>() );
        }

        public async Task CloseProviderRequestSession( RequestProviderSession session )
        {
            await _requestApi.CloseSessionAsync( session.Id );
        }

        public async Task<object> ReadRequest(RequestProviderSession session)
        {
            var response = await _requestApi.ReadRequestAsync( session.Id );

            return new
            {
                response.MessageId,
                response.MessageType,
                response.MessageContent,
                response.Topics
            };
        }
    }
}
