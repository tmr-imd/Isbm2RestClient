namespace Isbm2Client.Model;

public abstract record class Message(string Id, object MessageContent, string[] Topics, string Expiry);
public record class RequestMessage( string Id, object MessageContent, string[] Topics, string Expiry) : Message( Id, MessageContent, Topics, Expiry );
public record class ResponseMessage(string Id, object MessageContent, string[] Topics, string Expiry) : Message(Id, MessageContent, Topics, Expiry);
public record class PublicationMessage(string Id, object MessageContent, string[] Topics, string Expiry) : Message(Id, MessageContent, Topics, Expiry);