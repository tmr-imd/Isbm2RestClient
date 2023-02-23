using BlazorServerExample.Data;
using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2RestClient.Client;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace BlazorServerExample.Pages;

public class RequestViewModel : IAsyncDisposable
{
    public string Endpoint { get; set; } = "";
    public string ChannelUri { get; set; } = "/fred";
    public string Topic { get; set; } = "Yo!";

    public string FilterCode { get; set; } = "";
    public string FilterType { get; set; } = "";
    public string FilterLocation { get; set; } = "";
    public string FilterOwner { get; set; } = "";
    public string FilterCondition { get; set; } = "";
    public string FilterInspector { get; set; } = "";
    public string Message { get; set; } = "";

    public IEnumerable<StructureAsset> StructureAssets { get; set; } = Enumerable.Empty<StructureAsset>();

    private readonly IChannelManagement channelManagement;
    private readonly IProviderRequest provider;
    private readonly IConsumerRequest consumer;
    private readonly StructureAssetService service;

    private RequestChannel requestChannel = null!;
    private RequestProviderSession providerSession = null!;
    private RequestConsumerSession consumerSession = null!;

    public RequestViewModel( IOptions<ClientConfig> config, IChannelManagement channelManagement, IProviderRequest provider, IConsumerRequest consumer, StructureAssetService service )
    {
        Endpoint = config.Value?.EndPoint ?? "";

        this.channelManagement = channelManagement;
        this.provider = provider;
        this.consumer = consumer;
        this.service = service;

        Task.Run( async () => await Setup() );
    }

    public async ValueTask DisposeAsync()
    {
        await Teardown();
    }

    private async Task Setup()
    {
        try
        {
            requestChannel = await channelManagement.CreateChannel<RequestChannel>(ChannelUri, "Test");
        }
        catch (ApiException)
        {
            var channel = await channelManagement.GetChannel(ChannelUri);

            await channelManagement.DeleteChannel(ChannelUri);

            requestChannel = await channelManagement.CreateChannel<RequestChannel>(ChannelUri, "Test");
        }

        providerSession = await provider.OpenSession(ChannelUri, Topic);
        consumerSession = await consumer.OpenSession(ChannelUri);
    }

    private async Task Teardown()
    {
        if ( consumer != null && consumerSession != null ) await consumer.CloseSession(consumerSession.Id);
        if ( provider != null && providerSession != null ) await provider.CloseSession(providerSession.Id);

        if ( requestChannel != null ) await channelManagement.DeleteChannel( requestChannel.Uri );
    }

    public async Task Process()
    {
        var requestId = await Request();

        await Respond();

        StructureAssets = await ReadResponse( requestId );
    }

    public async Task<string> Request()
    {
        var payload = new Dictionary<string, string>()
        {
            { "FilterCode", FilterCode },
            { "FilterType", FilterType },
            { "FilterLocation", FilterLocation },
            { "FilterOwner", FilterOwner },
            { "FilterCondition", FilterCondition },
            { "FilterInspector", FilterInspector },

        };

        var json = JsonSerializer.Serialize( payload );

        var request = await consumer.PostRequest( consumerSession.Id, json, Topic );

        return request.Id;
    }

    public async Task Respond()
    {
        var requestMessage = await provider.ReadRequest(providerSession.Id);

        var json = requestMessage.MessageContent.GetContent<string>();

        var content = JsonSerializer.Deserialize<Dictionary<string, string>>( json );

        if ( content is not null )
        {
            var filterCode = (string)content["FilterCode"] ?? "";
            var filterType = (string)content["FilterType"] ?? "";
            var filterLocation = (string)content["FilterLocation"] ?? "";
            var filterOwner = (string)content["FilterOwner"] ?? "";
            var filterCondition = (string)content["FilterCondition"] ?? "";
            var filterInspector = (string)content["FilterInspector"] ?? "";

            var payload = new Dictionary<string, object>()
            {
                { "Structures", service.GetStructures(filterCode, filterType, filterLocation, filterOwner, filterCondition, filterInspector) }
            };

            _ = await provider.PostResponse(providerSession.Id, requestMessage.Id, payload);
        }
    }

    public async Task<IEnumerable<StructureAsset>> ReadResponse( string requestId )
    {
        var message = await consumer.ReadResponse( consumerSession.Id, requestId );

        var content = message.MessageContent.GetContent<Dictionary<string, object>>();

        var jsonArray = (JArray)content["Structures"];
        var structures = jsonArray.ToObject<List<StructureAsset>>();

        if (structures is not null)
            return structures;
        
        return Enumerable.Empty<StructureAsset>();
    }
}
