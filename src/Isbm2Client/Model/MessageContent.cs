using System.Text.Json;

namespace Isbm2Client.Model;

public record class MessageContent( JsonDocument Content, string MediaType, string? ContentEncoding = null)
{
    public static MessageContent From<T>( T content ) where T : notnull
    {
        var mediaType = content switch
        {
            string => "text/plain",
            _ => "application/json"
        };

        return content switch
        {
            JsonDocument document => new MessageContent(document, mediaType),
            T x => new MessageContent(JsonSerializer.SerializeToDocument(x), mediaType),
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
            throw new InvalidCastException( $"Could not deserialise JsonDocument to: {typeof(T).FullName}" );

        return instance;
    }
}
