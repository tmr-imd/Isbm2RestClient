using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2Client.Service;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Microsoft.Extensions.Options;
using RestClient = Isbm2RestClient.Client;

namespace Isbm2Client.Test;

public class RequestConsumerFixture : IAsyncLifetime
{
    public readonly string CHANNEL_URI = $"/pittsh/test/request/consumer/{Guid.NewGuid()}";
    public const string CHANNEL_DESCRIPTION = "For RestRequestConsumerTest class";

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
        catch ( ApiException )
        {
            var channel = await management.GetChannel(CHANNEL_URI);

            RequestChannel = (RequestChannel)channel;
        }

        RestClient.Configuration apiConfig = new()
        {
            BasePath = Config.Value.EndPoint
        };

        var requestApi = new ConsumerRequestServiceApi(apiConfig);

        Provider = new RestProviderRequest(Config);
        Consumer = new RestConsumerRequest(requestApi);
    }

    public async Task DisposeAsync()
    {
        // await Task.Yield();
        var management = new RestChannelManagement(Config);

        await management.DeleteChannel(CHANNEL_URI);
    }
}

[CollectionDefinition("Request Consumer collection")]
public class RequestConsumerCollection : ICollectionFixture<RequestConsumerFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
