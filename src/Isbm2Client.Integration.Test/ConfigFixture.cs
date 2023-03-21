using RestClient = Isbm2RestClient.Client;

namespace Isbm2Client.Integration.Test;

public class ConfigFixture
{
    public readonly RestClient.Configuration ApiConfig = new()
    {
        BasePath = "https://isbm.lab.oiiecosystem.net/rest"
    };
}
