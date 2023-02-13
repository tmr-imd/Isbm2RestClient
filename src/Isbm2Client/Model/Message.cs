namespace Isbm2Client.Model;

public abstract record class Message(string Id, MessageContent MessageContent, string[] Topics, string Expiry);
public record class RequestMessage( string Id, MessageContent MessageContent, string[] Topics, string Expiry) : Message( Id, MessageContent, Topics, Expiry );
public record class ResponseMessage(string Id, MessageContent MessageContent, string[] Topics, string Expiry) : Message(Id, MessageContent, Topics, Expiry);
public record class PublicationMessage(string Id, MessageContent MessageContent, string[] Topics, string Expiry) : Message(Id, MessageContent, Topics, Expiry);