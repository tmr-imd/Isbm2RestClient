using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2RestClient.Client;
using System.Text.Json;

namespace Isbm2Client.Integration.Test;

[Collection("Publication Consumer collection")]
public class RestConsumerPublicationTest
{
    private readonly PublicationChannel channel;
    private readonly IConsumerPublication consumer;
    private readonly IProviderPublication provider;

    private static readonly string YO = "Yo!";
    private static readonly string YO_TOPIC = "Yo Topic!";

    public RestConsumerPublicationTest( PubSubConsumerFixture fixture )
    {
        channel = fixture.PublicationChannel;
        provider = fixture.Provider;
        consumer = fixture.Consumer;
    }

    [Fact]
    public async Task OpenAndCloseSession()
    {
        var session = await consumer.OpenSession( channel.Uri, YO_TOPIC );

        await consumer.CloseSession( session.Id );
    }

    [Fact]
    public async Task CantCloseSessionTwice()
    {
        var session = await consumer.OpenSession(channel.Uri, YO_TOPIC );

        await consumer.CloseSession(session.Id);

        Task closeAgain() => consumer.CloseSession(session.Id);

        await Assert.ThrowsAsync<IsbmFault>( closeAgain );
    }

    [Fact]
    public async Task ReadStringPublication()
    {
        var providerSession = await provider.OpenSession( channel.Uri );
        var consumerSession = await consumer.OpenSession( channel.Uri, YO_TOPIC );

        Assert.Null(await consumer.ReadPublication( consumerSession.Id ));
        await provider.PostPublication(providerSession.Id, YO, YO_TOPIC);

        try
        {
            var message = await consumer.ReadPublication( consumerSession.Id );
            await consumer.RemovePublication( consumerSession.Id );

            Assert.NotNull(message);
            Assert.False(String.IsNullOrWhiteSpace(message?.Id));

            var content = message?.MessageContent.Deserialise<string>();

            Assert.NotNull(content);
            Assert.Contains(YO, content);
        }
        finally
        {
            await consumer.CloseSession( consumerSession.Id );
            await provider.CloseSession( providerSession.Id );
        }
    }

    [Fact]
    public async Task ReadJsonDocumentPublication()
    {
        var providerSession = await provider.OpenSession( channel.Uri);
        var consumerSession = await consumer.OpenSession( channel.Uri, YO_TOPIC );

        var inputContent = new Dictionary<string, object>()
        {
            { "fred", "barney" },
            { "wilma", "betty" }
        };

        var document = JsonSerializer.SerializeToDocument( inputContent );

        Assert.Null(await consumer.ReadPublication( consumerSession.Id ));
        await provider.PostPublication(providerSession.Id, document, YO_TOPIC);

        try
        {
            var message = await consumer.ReadPublication( consumerSession.Id );
            await consumer.RemovePublication( consumerSession.Id );

            Assert.NotNull(message);
            Assert.False(String.IsNullOrWhiteSpace(message?.Id));

            var content = message?.MessageContent.Content;

            Assert.NotNull(content);

            if ( content is not null )
                Assert.Contains( "barney", content.RootElement.GetProperty("fred").GetString() );
        }
        finally
        {
            await consumer.CloseSession( consumerSession.Id );
            await provider.CloseSession( providerSession.Id );
        }
    }

    [Fact]
    public async Task ReadComplexObjectRequest()
    {
        var providerSession = await provider.OpenSession(channel.Uri);
        var consumerSession = await consumer.OpenSession(channel.Uri, YO_TOPIC);

        var inputContent = new TestObject()
        {
            Numbers = new[] { 23.0, 45, 100 },
            Text = "Hello",
            Weather = new Dictionary<string, double>()
            {
                {"Devonport", 14.0 },
                {"Hobart", 2.0 }
            }
        };

        Assert.Null(await consumer.ReadPublication(consumerSession.Id));
        await provider.PostPublication(providerSession.Id, inputContent, YO_TOPIC);

        try
        {
            var message = await consumer.ReadPublication(consumerSession.Id);
            await consumer.RemovePublication( consumerSession.Id );

            Assert.NotNull(message);
            Assert.False(String.IsNullOrWhiteSpace(message?.Id));

            var content = message?.MessageContent.Deserialise<TestObject>();

            Assert.True(content?.Weather["Hobart"] == 2);
        }
        finally
        {
            await consumer.CloseSession(consumerSession.Id);
            await provider.CloseSession(providerSession.Id);
        }
    }
}