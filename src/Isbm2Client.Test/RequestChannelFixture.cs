﻿using Isbm2Client.Model;
using Isbm2Client.Service;
using Isbm2RestClient.Client;
using Microsoft.Extensions.Options;

namespace Isbm2Client.Test;

public class RequestChannelFixture : IAsyncLifetime
{
    public const string CHANNEL_URI = "/pittsh/test/request";
    public const string CHANNEL_DESCRIPTION = "fred";

    public readonly IOptions<ClientConfig> Config = Options.Create( new ClientConfig() 
    {
        EndPoint = "https://isbm.lab.oiiecosystem.net/rest"
    });

    public RequestChannel RequestChannel { get; set; } = null!;

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
    }

    public async Task DisposeAsync()
    {
        var management = new RestChannelManagement(Config);

        await management.DeleteChannel(CHANNEL_URI);
    }
}

[CollectionDefinition("Request Channel collection")]
public class RequestChannelCollection : ICollectionFixture<RequestChannelFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
