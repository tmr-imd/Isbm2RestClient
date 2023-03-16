using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2RestClient.Client;

namespace Isbm2Client.Test.Integration_Tests;

[Collection("Publication Provider collection")]
public class RestProviderPublicationTest
{
    private readonly PublicationChannel channel;
    private readonly IConsumerPublication consumer;
    private readonly IProviderPublication provider;

    private static readonly string BOO = "Boo!";
    private static readonly string BOO_TOPIC = "Boo Topic!";
    private static readonly string EXPIRY = "P1D";

    public RestProviderPublicationTest( PublicationProviderFixture fixture )
    {
        channel = fixture.PublicationChannel;
        provider = fixture.Provider;
        consumer = fixture.Consumer;
    }

    [Fact]
    public async Task OpenAndCloseSession()
    {
        PublicationProviderSession session = await provider.OpenSession( channel.Uri );

        await provider.CloseSession( session.Id );
    }

    [Fact]
    public async Task CantCloseSessionTwice()
    {
        PublicationProviderSession session = await provider.OpenSession( channel.Uri);

        await provider.CloseSession(session.Id);

        Task closeAgain() => provider.CloseSession(session.Id);

        await Assert.ThrowsAsync<ApiException>(closeAgain);
    }

    [Fact]
    public async Task PostPublication()
    {
        PublicationProviderSession session = await provider.OpenSession(channel.Uri);

        PublicationMessage message = await provider.PostPublication( session.Id, BOO, BOO_TOPIC, EXPIRY );

        Assert.NotNull(message.Id);
        Assert.NotEmpty(message.Id);

        await provider.CloseSession( session.Id );
    }

    [Fact]
    public async Task ExpirePublication()
    {
        PublicationProviderSession session = await provider.OpenSession(channel.Uri);

        PublicationMessage message = await provider.PostPublication( session.Id, BOO, BOO_TOPIC );

        Assert.NotEmpty(message.Id);

        await provider.ExpirePublication( session.Id, message.Id );

        await provider.CloseSession( session.Id );
    }
}