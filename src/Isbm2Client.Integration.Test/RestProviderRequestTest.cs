using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2RestClient.Client;
using System.Text.Json;

namespace Isbm2Client.Integration.Test;

[Collection("Request Provider collection")]
public class RestProviderRequestTest
{
    private readonly RequestChannel channel;
    private readonly IConsumerRequest consumer;
    private readonly IProviderRequest provider;

    private static readonly string YO = "Yo!";
    private static readonly string YO_TOPIC = "Yo Topic!";

    public RestProviderRequestTest( RequestProviderFixture fixture )
    {
        channel = fixture.RequestChannel;
        provider = fixture.Provider;
        consumer = fixture.Consumer;
    }

    [Fact]
    public async Task OpenAndCloseSession()
    {
        var session = await provider.OpenSession( channel.Uri, YO_TOPIC );

        await provider.CloseSession( session.Id );
    }

    [Fact]
    public async Task CantCloseSessionTwice()
    {
        var session = await provider.OpenSession(channel.Uri, YO_TOPIC );

        await provider.CloseSession(session.Id);

        Task closeAgain() => provider.CloseSession(session.Id);

        await Assert.ThrowsAsync<ApiException>( closeAgain );
    }

    [Fact]
    public async Task ReadStringRequest()
    {
        var providerSession = await provider.OpenSession( channel.Uri, YO_TOPIC );
        var consumerSession = await consumer.OpenSession( channel.Uri );

        await consumer.PostRequest(consumerSession.Id, YO, YO_TOPIC);

        try
        {
            var message = await provider.ReadRequest( providerSession.Id );
            await provider.RemoveRequest( providerSession.Id );

            var content = message.MessageContent.Deserialise<string>();

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
    public async Task ReadJsonDocumentRequest()
    {
        var providerSession = await provider.OpenSession( channel.Uri, YO_TOPIC);
        var consumerSession = await consumer.OpenSession( channel.Uri );

        var inputContent = new Dictionary<string, object>()
        {
            { "fred", "barney" },
            { "wilma", "betty" }
        };

        var document = JsonSerializer.SerializeToDocument( inputContent );

        await consumer.PostRequest(consumerSession.Id, document, YO_TOPIC);

        try
        {
            var message = await provider.ReadRequest( providerSession.Id );
            await provider.RemoveRequest( providerSession.Id );

            var content = message.MessageContent.Content;

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
        var providerSession = await provider.OpenSession(channel.Uri, YO_TOPIC);
        var consumerSession = await consumer.OpenSession(channel.Uri);

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

        await consumer.PostRequest(consumerSession.Id, inputContent, YO_TOPIC);

        try
        {
            var message = await provider.ReadRequest(providerSession.Id);
            await provider.RemoveRequest( providerSession.Id );

            var content = message.MessageContent.Deserialise<TestObject>();

            Assert.True(content.Weather["Hobart"] == 2);
        }
        finally
        {
            await consumer.CloseSession(consumerSession.Id);
            await provider.CloseSession(providerSession.Id);
        }
    }

    [Fact]
    public async Task ReadRequestPostResponse()
    {
        var providerSession = await provider.OpenSession(channel.Uri, YO_TOPIC);
        var consumerSession = await consumer.OpenSession(channel.Uri);

        await consumer.PostRequest(consumerSession.Id, YO, YO_TOPIC);

        try
        {
            var requestMessage = await provider.ReadRequest(providerSession.Id);
            await provider.RemoveRequest( providerSession.Id );

            var content = requestMessage.MessageContent.Deserialise<string>();

            Assert.True( content == YO );

            var message = await provider.PostResponse( providerSession.Id, requestMessage.Id, "Carrots!" );

            Assert.NotNull( message );
            Assert.Contains( "Carrots", message.MessageContent.Deserialise<string>() );
        }
        finally
        {
            await consumer.CloseSession(consumerSession.Id);
            await provider.CloseSession(providerSession.Id);
        }
    }
}