using Isbm2Client.Service;

namespace Isbm2Client.Model;

public record ClientConfig : IClientConfig
{
    public string EndPoint { get; init; } = "http://localhost/rest";
}
