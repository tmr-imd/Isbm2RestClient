using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2Client.Service;
using Isbm2RestClient.Client;
using Microsoft.Extensions.Options;

namespace Isbm2Client.Test;

public class PublicationConsumerFixture : IAsyncLifetime
{
    public readonly string CHANNEL_URI = $"/isbm2restclient/test/publication/consumer/{Guid.NewGuid()}";
    public const string CHANNEL_DESCRIPTION = "For RestPublicationConsumerTest class";

    public readonly IOptions<ClientConfig> Config = Options.Create( new ClientConfig() 
    {
        EndPoint = "https://isbm.lab.oiiecosystem.net/rest"
    });

    public PublicationChannel PublicationChannel { get; set; } = null!;
    public IProviderPublication Provider { get; set; } = null!;
    public IConsumerPublication Consumer { get; set; } = null!;

    public async Task InitializeAsync()
    {
        var management = new RestChannelManagement(Config);

        try
        {
            PublicationChannel = await management.CreateChannel<PublicationChannel>( CHANNEL_URI, CHANNEL_DESCRIPTION );
        }
        catch ( ApiException )
        {
            var channel = await management.GetChannel(CHANNEL_URI);

            PublicationChannel = (PublicationChannel)channel;
        }

        Provider = new RestProviderPublication(Config);
        Consumer = new RestConsumerPublication(Config);
    }

    public async Task DisposeAsync()
    {
        var management = new RestChannelManagement(Config);

        await management.DeleteChannel(CHANNEL_URI);
    }
}

[CollectionDefinition("Publication Consumer collection")]
public class PublicationConsumerCollection : ICollectionFixture<PublicationConsumerFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
