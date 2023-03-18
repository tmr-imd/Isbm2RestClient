using Isbm2Client.Model;
using RestClient = Isbm2RestClient.Client;
using RestModel = Isbm2RestClient.Model;
using RestException = Isbm2RestClient.Client.ApiException;
using HttpStatusCode = System.Net.HttpStatusCode;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace Isbm2Client.Extensions;

public static class IsbmFaultRestExtensions
{
    private static readonly Regex OpenSessionWithFilters = new Regex("Open(Subscription|ProviderRequest)Session");
    private static readonly Regex OpenAnySession = new Regex("Open.+Session");
    private static readonly Regex MessageOperation = new Regex("((Read)|(Remove)|(Post)|(Expire))((Publication)|(Request)|(Response))");
    private static readonly Regex ReadMessage = new Regex("Read((Publication)|(Request)|(Response))");

    public static readonly RestClient.ExceptionFactory IsbmFaultFactory = (methodName, response) =>
    {
        var status = (int)response.StatusCode;
        if (status == 0)
        {
            return new ClientException($"Error calling {methodName}: {response.ErrorText}");
        }
        if (status < 400) return null;

        // All faults have a single property 'fault'
        JsonElement responseFault = JsonSerializer.Deserialize<JsonElement>(response.RawContent);
        JsonElement faultString;
        var hasFault = responseFault.TryGetProperty("fault", out faultString);
        if (!hasFault)
        {
            return new IsbmFault(IsbmFaultType.Unknown,
                response.RawContent,
                $"Error calling {methodName}: {response.RawContent}\n{response.Headers}");
        }

        if (ReadMessage.IsMatch(methodName) && response.StatusCode == HttpStatusCode.NotFound
            && (!(faultString.GetString()?.Contains("session", StringComparison.InvariantCultureIgnoreCase) ?? false )
                || (faultString.GetString()?.Contains("message", StringComparison.InvariantCultureIgnoreCase) ?? false)) )
        {
            // We assume that if the fault does not mention the 'session' or it also mentions 'message',
            // then it is saying that there are no messages.
            // This captures faults like: 'No messages for session <...>'
            return null;
        }

        IsbmFaultType faultType;
        switch (response.StatusCode)
        {
            case HttpStatusCode.BadRequest:
                faultType = methodName switch
                {
                    // Namespace faults are kind of parameter fault, so we assume it is a namespace fault
                    // if the fault string mentions namespaces (only applies to opening subscription and provider request sessions)
                    var op when OpenSessionWithFilters.IsMatch(op)
                            && (faultString.GetString()?.Contains("namespace", StringComparison.InvariantCultureIgnoreCase) ?? false)
                        => IsbmFaultType.NamespaceFault,
                    _ => IsbmFaultType.ParameterFault
                };
                break;
            case HttpStatusCode.NotFound:
                faultType = methodName switch
                {
                    "GetChannel" => IsbmFaultType.ChannelFault,
                    "DeleteChannel" => IsbmFaultType.ChannelFault,
                    "AddSecurityTokens" => IsbmFaultType.ChannelFault,
                    "RemoveSecurityTokens" => IsbmFaultType.ChannelFault,
                    var op when OpenAnySession.IsMatch(op) => IsbmFaultType.ChannelFault,
                    // "OpenPublicationSession" => IsbmFaultType.ChannelFault,
                    // "OpenSubscriptionSession" => IsbmFaultType.ChannelFault,
                    // "OpenProviderRequestSession" => IsbmFaultType.ChannelFault,
                    // "OpenConsumerRequestSession" => IsbmFaultType.ChannelFault,
                    "CloseSession" => IsbmFaultType.SessionFault,
                    var op when MessageOperation.IsMatch(op) => IsbmFaultType.SessionFault,
                    // "ReadPublication" => IsbmFaultType.SessionFault,
                    // "RemovePublication" => IsbmFaultType.SessionFault,
                    // "PostPublication" => IsbmFaultType.SessionFault,
                    // "ExpirePublication" => IsbmFaultType.SessionFault,
                    // "ReadRequest" => IsbmFaultType.SessionFault,
                    // "RemoveRequest" => IsbmFaultType.SessionFault,
                    // "PostRequest" => IsbmFaultType.SessionFault,
                    // "ExpireRequest" => IsbmFaultType.SessionFault,
                    // "ReadResponse" => IsbmFaultType.SessionFault,
                    // "RemoveResponse" => IsbmFaultType.SessionFault,
                    // "PostResponse" => IsbmFaultType.SessionFault,
                    _ => IsbmFaultType.Unknown
                };
                break;
            case HttpStatusCode.Conflict:
                faultType = methodName switch
                {
                    "CreateChannel" => IsbmFaultType.ChannelFault,
                    "AddSecurityTokens" => IsbmFaultType.OperationFault,
                    "RemoveSecurityTokens" => IsbmFaultType.SecurityTokenFault,
                    _ => IsbmFaultType.Unknown
                };
                break;
            case HttpStatusCode.UnprocessableEntity:
                faultType = methodName switch
                {
                    var op when OpenAnySession.IsMatch(op) => IsbmFaultType.OperationFault,
                    var op when MessageOperation.IsMatch(op) => IsbmFaultType.SessionFault,
                    _ => IsbmFaultType.Unknown
                };
                break;
            case HttpStatusCode.Unauthorized:
                faultType = methodName switch
                {
                    "GetSecurityDetails" => IsbmFaultType.SecurityTokenFault,
                    _ => IsbmFaultType.Unknown
                };
                break;
            default:
                return new IsbmFault(IsbmFaultType.Unknown,
                    faultString.GetString(),
                    $"Error calling {methodName} (Unexpected HTTP Status ${response.StatusCode}): {response.RawContent}\n{response.Headers}");;
        }
        return new IsbmFault(faultType,
            faultString.GetString(),
            $"Error calling {methodName}: {response.RawContent}\n{response.Headers}");
    };

    public static IsbmFault FromApiError(RestException apiError)
    {
        return new IsbmFault(IsbmFaultType.ChannelFault);
    }
}
