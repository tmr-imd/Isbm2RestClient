using System.Text.Json;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Isbm2Client.Model;

public record class MessageContent( JsonDocument Content, string MediaType, string? ContentEncoding = null)
{
    public static MessageContent From<T>( T content, string? overrideMediaType = null ) where T : notnull
    {
        var mediaType = overrideMediaType ?? content switch
        {
            string => "text/plain",
            XDocument => "application/xml",
            XElement => "application/xml",
            _ => "application/json"
        };

        var messageContent = content switch
        {
            JsonDocument x => new MessageContent(x, mediaType),
            XDocument x => new MessageContent(JsonSerializer.SerializeToDocument(x.ToString()), mediaType),
            XElement x => new MessageContent(JsonSerializer.SerializeToDocument(x.ToString()), mediaType),
            T x => new MessageContent(JsonSerializer.SerializeToDocument(x), mediaType),
            _ => throw new NotImplementedException()
        };

        var document = messageContent.Content;

        if ( document.RootElement.ValueKind != JsonValueKind.Object && document.RootElement.ValueKind != JsonValueKind.String )
            throw new InvalidCastException("Root element for JsonDocument must either be a String or an Object");

        return messageContent;
    }

    public T Deserialise<T>() where T : notnull
    {
        if ( Content.RootElement.ValueKind != JsonValueKind.Object && Content.RootElement.ValueKind != JsonValueKind.String )
            throw new InvalidCastException("Root element for JsonDocument must either be a String or an Object");

        if ( typeof(T) == typeof(string) && Content.RootElement.ValueKind != JsonValueKind.String )
            throw new InvalidCastException( $"Could not deserialise JsonDocument to: {typeof(T).FullName}" );
        
        if ( typeof(T) != typeof(string) && MediaType.StartsWith("application/xml") && Content.RootElement.ValueKind == JsonValueKind.String )
            return DeserialiseXml<T>();

        var instance = JsonSerializer.Deserialize<T>( Content );

        if ( instance is null )
            throw new InvalidCastException( $"Could not deserialise JsonDocument to: {typeof(T).FullName}" );

        return instance;
    }

    private T DeserialiseXml<T>() where T : notnull
    {
        XDocument xml = XDocument.Load(new StringReader(Deserialise<string>()));

        if (xml is T)
            return (T)(object)xml;
        if (xml.Root is T)
            return (T)(object)xml.Root;
        
        var instance = new XmlSerializer(typeof(T)).Deserialize(xml.CreateReader());

        if ( instance is null )
            throw new InvalidCastException( $"Could not deserialise JsonDocument to: {typeof(T).FullName}" );
        
        return (T)instance;
    }
}
