using Isbm2Client.Extensions;

using RestClient = Isbm2RestClient.Client;

namespace Isbm2Client.Service;

public abstract class AbstractRestService
{
    protected async Task ProtectedApiCallAsync(System.Func<Task> apiCall)
    {
        try {
            await apiCall();
        }
        catch (RestClient.ApiException e) {
            throw IsbmFaultRestExtensions.FromApiError(e);
        }
    }

    protected async Task<T> ProtectedApiCallAsync<T>(System.Func<Task<T>> apiCall)
    {
        try {
            return await apiCall();
        }
        catch (RestClient.ApiException e) {
            throw IsbmFaultRestExtensions.FromApiError(e);
        }
    }

    protected T CreateInstance<T>(Type type, params object?[]? args) where T : notnull
    {
        if (!type.IsAssignableTo(typeof(T))) throw new ArgumentException("Object of 'type' must be assignable to generic type 'T'.");

        var instance = Activator.CreateInstance(type, args);

        if ( instance is null ) throw new Exception($"Activator.CreateInstance({typeof(T).Name},...) unexpectedly returned null. This should not happen");

        return (T)instance;
    }
}