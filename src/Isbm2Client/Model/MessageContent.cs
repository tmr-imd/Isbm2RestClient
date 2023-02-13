namespace Isbm2Client.Model;

public abstract record MessageContent(string Id, string MediaType, string ContentEncoding)
{
    private readonly object instance = null!;

    public T GetContent<T>()
    {
        if ( instance is not T ) throw new Exception( "Uh oh" );

        return (T)instance;
    }

    public MessageContent( string id, object instance, string mediaType, string contentEncoding ) : this( id, mediaType, contentEncoding )
    {
        if ( instance is not string && instance is not Dictionary<string, object> )
            throw new ArgumentException("Invalid instance found. Must be the following types: Dictionary<string, Object>, string");

        this.instance = instance;
    }
};

public record class MessageContent<T>(string Id, T Content, string MediaType, string ContentEncoding) : MessageContent(Id, Content, MediaType, ContentEncoding) where T : notnull;
