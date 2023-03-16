using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2Client.Service;
using Microsoft.Extensions.Options;
using RestClient = Isbm2RestClient.Client;
using RestApi = Isbm2RestClient.Api;

namespace Isbm2Client.Integration.Test;

public class RequestProviderFixture : IAsyncLifetime
{
    public readonly string CHANNEL_URI = $"/pittsh/test/request/provider/{Guid.NewGuid()}";
    public const string CHANNEL_DESCRIPTION = "For RestRequestProviderTest class";

    public readonly IOptions<ClientConfig> Config = Options.Create( new ClientConfig() 
    {
        EndPoint = "https://isbm.lab.oiiecosystem.net/rest"
    });

    public RequestChannel RequestChannel { get; set; } = null!;
    public IProviderRequest Provider { get; set; } = null!;
    public IConsumerRequest Consumer { get; set; } = null!;

    public async Task InitializeAsync()
    {
        var management = new RestChannelManagement(Config);

        try
        {
            RequestChannel = await management.CreateChannel<RequestChannel>( CHANNEL_URI, CHANNEL_DESCRIPTION );
        }
        catch ( RestClient.ApiException )
        {
            var channel = await management.GetChannel(CHANNEL_URI);

            RequestChannel = (RequestChannel)channel;
        }

        RestClient.Configuration apiConfig = new()
        {
            BasePath = Config.Value.EndPoint
        };

        var providerApi = new RestApi.ProviderRequestServiceApi( apiConfig );
        var consumerApi = new RestApi.ConsumerRequestServiceApi( apiConfig );

        Provider = new RestProviderRequest(providerApi);
        Consumer = new RestConsumerRequest(consumerApi);
    }

    public async Task DisposeAsync()
    {
        // await Task.Yield();
        var management = new RestChannelManagement(Config);

        await management.DeleteChannel(CHANNEL_URI);
    }
}

[CollectionDefinition("Request Provider collection")]
public class RequestProviderCollection : ICollectionFixture<RequestProviderFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
