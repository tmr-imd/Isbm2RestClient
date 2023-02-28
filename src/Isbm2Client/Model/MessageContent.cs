using System.Text.Json;

namespace Isbm2Client.Model;

public record class MessageContent( JsonDocument Content )
{
    public static MessageContent From<T>( T content ) where T : notnull
    {
        return content switch
        {
            JsonDocument x => new MessageContent(x),
            T => new MessageContent(JsonSerializer.SerializeToDocument(content)),
            _ => throw new NotImplementedException()
        };
    }

    public T Deserialise<T>() where T : notnull
    {
        if ( Content.RootElement.ValueKind != JsonValueKind.Object && Content.RootElement.ValueKind != JsonValueKind.String )
            throw new InvalidCastException("Root element for JsonDocument must either be a String or an Object");

        if ( typeof(T) == typeof(string) && Content.RootElement.ValueKind != JsonValueKind.String )
            throw new InvalidCastException( $"Could not deserialise JsonDocument to: {typeof(T).FullName}" );

        var instance = JsonSerializer.Deserialize<T>( Content );

        if ( instance is null )
            throw new InvalidCastException($"Could not deserialise JsonDocument to: {typeof(T).FullName}");

        return instance;
    }
}
