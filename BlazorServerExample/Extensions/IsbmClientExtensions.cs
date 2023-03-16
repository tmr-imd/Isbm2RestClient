using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2Client.Service;
using Isbm2RestClient.Api;

namespace BlazorServerExample.Extensions;

public static class IsbmClientExtensions
{
    public static void AddIsbmRestClient( this IServiceCollection services, ClientConfig config )
    {
        services.AddScoped<IConsumerRequestServiceApi>(x => new ConsumerRequestServiceApi(config.EndPoint));
        services.AddScoped<IProviderRequestServiceApi>(x => new ProviderRequestServiceApi(config.EndPoint));

        services.AddScoped<IChannelManagement, RestChannelManagement>();
        services.AddScoped<IProviderRequest, RestProviderRequest>();
        services.AddScoped<IConsumerRequest, RestConsumerRequest>();
        services.AddScoped<IProviderPublication, RestProviderPublication>();
        services.AddScoped<IConsumerPublication, RestConsumerPublication>();
    }
}
