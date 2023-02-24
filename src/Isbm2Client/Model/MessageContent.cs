using Isbm2Client.Extensions;
using System.Text.Json;

namespace Isbm2Client.Model;

public abstract record class MessageContent( string Id )
{
    private readonly object instance = null!;

    public T GetContent<T>()
    {
        if ( typeof(T) != typeof(string) && typeof(T) != typeof(JsonDocument) )
        {
            throw new ArgumentException("Requested type must be one of the following: string or JsonDocument");
        }

        if (instance is not T)
            throw new InvalidCastException($"Instance is not of type: {typeof(T).FullName}");

        return (T)instance;
    }

    public T Deserialise<T>() where T : class, new()
    {
        if ( instance is JsonDocument document )
        {
            var typedObject = JsonSerializer.Deserialize<T>( document );

            if (typedObject is null)
                throw new InvalidCastException($"Could not deserialise JsonDocument to: {typeof(T).Name}");

            return typedObject;
        }

        throw new InvalidOperationException( "Uh oh" );
    }

    public MessageContent( string id, object instance ) : this(id)
    {
        if ( instance is not string && instance is not JsonDocument )
        {
            throw new ArgumentException("Invalid instance found. Must be the following types: string or JsonDocument");
        }

        this.instance = instance;
    }
}
public record class MessageString( string Id, string Content ) : MessageContent( Id, Content );
public record class MessageJsonDocument(string Id, JsonDocument Content) : MessageContent(Id, Content);
