using Isbm2Client.Model;
using RestClient = Isbm2RestClient.Client;
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

    /// <summary>
    /// Exception Factory delegate to be assigned to the REST service client implementations to ensure
    /// only IsbmtFault and ClientException type exceptions are raised.
    /// </summary>
    /// <remarks>
    /// This covers exceptions that would be raised as a result of processing the HTTP response.
    /// </remarks>
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
            && (!hasFault // for servers that do not use a fault element in the 404.
                || (!(faultString.GetString()?.Contains("session", StringComparison.InvariantCultureIgnoreCase) ?? false )
                    || (faultString.GetString()?.Contains("message", StringComparison.InvariantCultureIgnoreCase) ?? false)) ) )
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
                    "CloseSession" => IsbmFaultType.SessionFault,
                    var op when MessageOperation.IsMatch(op) => IsbmFaultType.SessionFault,
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

    /// <summary>
    /// Converts an ApiException from the REST client implementation into an IsbmFault
    /// or ClientException as appropriate.
    /// </summary>
    /// <remarks>
    /// This covers exceptions that are raised directly by the client implementation, 
    /// such as those from client-side validation.
    /// </remarks>
    /// <param name="apiError">The original ApiException</param>
    /// <returns>IsbmFault or ClientException</returns>
    public static Exception FromApiError(RestException apiError)
    {
        // Only 400s for argument exceptions (ParameterFaults) and 500 for
        // deserialisation errors are produced by the generated REST API classes
        // Everything is a ClientException as it is something that is raised by
        // the client library implementation; we assume deserialization errors
        // are the fault of the server.
        return apiError.ErrorCode switch
        {
            400 => new IsbmFault(IsbmFaultType.ParameterFault, message: apiError.Message, innerException: apiError),
            500 => new IsbmFault(IsbmFaultType.Unknown, message: apiError.Message, innerException: apiError),
            _ => new ClientException(message: apiError.Message, innerException: apiError)
        };
    }
}
