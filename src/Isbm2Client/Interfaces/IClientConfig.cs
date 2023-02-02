using Isbm2Client.Model;

namespace Isbm2Client.Service; 

public interface IClientConfig {
    string EndPoint { get; init; }
}
