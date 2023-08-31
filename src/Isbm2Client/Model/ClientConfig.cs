using Isbm2Client.Service;

namespace Isbm2Client.Model;

public record ClientConfig
{
    public string EndPoint { get; init; } = "http://localhost/rest";
    public string ListenerUrlBase { get; init; } = "";
}
