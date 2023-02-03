using Isbm2Client.Model;
using Microsoft.Extensions.Options;

namespace Isbm2Client.Test;

public class ServiceFixture
{
    public const string CHANNEL_URI = "/example/test_channel/publish";
    public const string CHANNEL_DESCRIPTION = "fred";

    public readonly IOptions<ClientConfig> Config = Options.Create( new ClientConfig() 
    {
        EndPoint = "https://isbm.lab.oiiecosystem.net/rest"
    });

    public readonly RequestChannel RequestChannel;

    public ServiceFixture()
    {
        RequestChannel = new RequestChannel( CHANNEL_URI, CHANNEL_DESCRIPTION );
    }
}
