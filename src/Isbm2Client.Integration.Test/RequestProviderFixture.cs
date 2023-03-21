using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2Client.Service;
using RestClient = Isbm2RestClient.Client;
using RestApi = Isbm2RestClient.Api;

namespace Isbm2Client.Integration.Test;

public class RequestProviderFixture : IAsyncLifetime
{
    public readonly string CHANNEL_URI = $"/pittsh/test/request/provider/{Guid.NewGuid()}";
    public const string CHANNEL_DESCRIPTION = "For RestRequestProviderTest class";

    public readonly RestClient.Configuration ApiConfig = new()
    {
        BasePath = "https://isbm.lab.oiiecosystem.net/rest"
    };

    public RequestChannel RequestChannel { get; set; } = null!;
    public IProviderRequest Provider { get; set; } = null!;
    public IConsumerRequest Consumer { get; set; } = null!;

    public async Task InitializeAsync()
    {
        var channelApi = new RestApi.ChannelManagementApi(ApiConfig);
        var management = new RestChannelManagement(channelApi);

        try
        {
            RequestChannel = await management.CreateChannel<RequestChannel>( CHANNEL_URI, CHANNEL_DESCRIPTION );
        }
        catch (IsbmFault e) when (e.FaultType == IsbmFaultType.ChannelFault)
        {
            var channel = await management.GetChannel(CHANNEL_URI);

            RequestChannel = (RequestChannel)channel;
        }

        var providerApi = new RestApi.ProviderRequestServiceApi( ApiConfig );
        var consumerApi = new RestApi.ConsumerRequestServiceApi( ApiConfig );

        Provider = new RestProviderRequest(providerApi);
        Consumer = new RestConsumerRequest(consumerApi);
    }

    public async Task DisposeAsync()
    {
        var channelApi = new RestApi.ChannelManagementApi(ApiConfig);
        var management = new RestChannelManagement(channelApi);

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
