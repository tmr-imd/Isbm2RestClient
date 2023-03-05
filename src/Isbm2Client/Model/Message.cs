namespace Isbm2Client.Model;

public abstract record class Message(string Id, MessageContent MessageContent, string[] Topics, string? Expiry, string? RequestMessageId);
public record class RequestMessage(string Id, MessageContent MessageContent, string[] Topics, string? Expiry) : Message(Id, MessageContent, Topics, Expiry, null);
public record class ResponseMessage(string Id, MessageContent MessageContent, string RequestMessageId) : Message(Id, MessageContent, Array.Empty<string>(), null, RequestMessageId);
public record class PublicationMessage(string Id, MessageContent MessageContent, string[] Topics, string? Expiry) : Message(Id, MessageContent, Topics, Expiry, null);