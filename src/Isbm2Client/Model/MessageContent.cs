using Isbm2Client.Extensions;
using System.Text.Json;

namespace Isbm2Client.Model;

public abstract record class MessageContent( string Id )
{
    private readonly object instance = null!;

    public T GetContent<T>()
    {
        if ( typeof(T) != typeof(string) && typeof(T) != typeof(Dictionary<string, object>) )
        {
            throw new ArgumentException("Requested type must be one of the following: string or Dictionary<string, object>");
        }

        if (instance is not T)
            throw new InvalidCastException($"Instance is not of type: string");

        return (T)instance;
    }

    public T Deserialise<T>() where T : class, new()
    {
        if ( instance is Dictionary<string, object> dictionary )
        {
            return ObjectExtensions.ToObject<T>( dictionary );
        }

        throw new InvalidOperationException( "Uh oh" );
    }

    public MessageContent( string id, object instance ) : this(id)
    {
        if ( instance is not string && instance is not Dictionary<string, object> )
        {
            throw new ArgumentException("Invalid instance found. Must be the following types: string or Dictionary<string, object>");
        }

        this.instance = instance;
    }
}
public record class MessageString( string Id, string Content ) : MessageContent( Id, Content );
public record class MessageJsonDocument(string Id, Dictionary<string, object> Content) : MessageContent(Id, Content);
