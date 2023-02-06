using Isbm2Client.Model;
using Isbm2Client.Service;
using Microsoft.Extensions.Options;

namespace Isbm2Client.Test;

public class ServiceFixture
{
    public const string CHANNEL_URI = "/example/test/fixture/publish";
    public const string CHANNEL_DESCRIPTION = "fred";

    public readonly IOptions<ClientConfig> Config = Options.Create( new ClientConfig() 
    {
        EndPoint = "https://isbm.lab.oiiecosystem.net/rest"
    });

    public readonly RestChannelManagement ChannelManagement = null!;

    public ServiceFixture()
    {
        ChannelManagement = new RestChannelManagement( Config );
    }
}
