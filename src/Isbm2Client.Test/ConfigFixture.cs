using Isbm2Client.Model;
using Isbm2Client.Service;
using Microsoft.Extensions.Options;

namespace Isbm2Client.Test;

public class ConfigFixture
{
    public readonly IOptions<ClientConfig> Config = Options.Create( new ClientConfig() 
    {
        EndPoint = "https://isbm.lab.oiiecosystem.net/rest"
    });
}
