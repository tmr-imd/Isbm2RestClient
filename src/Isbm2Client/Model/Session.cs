namespace Isbm2Client.Model;

public abstract record class Session(string Id, string? ListenerUri, string[] Topics, string[] FilterExressions);
public record class PublicationProviderSession(string Id, string? ListenerUri, string[] Topics, string[] FilterExressions) : Session(Id, ListenerUri, Topics, FilterExressions);
public record class PublicationConsumerSession(string Id, string? ListenerUri, string[] Topics, string[] FilterExressions) : Session(Id, ListenerUri, Topics, FilterExressions);
public record class RequestProviderSession(string Id, string? ListenerUri, string[] Topics, string[] FilterExressions) : Session(Id, ListenerUri, Topics, FilterExressions);
public record class RequestConsumerSession(string Id, string? ListenerUri, string[] Topics, string[] FilterExressions) : Session(Id, ListenerUri, Topics, FilterExressions);