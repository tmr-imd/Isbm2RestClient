namespace Isbm2Client.Model;

public abstract record class Session(string Id, string ListenerUrl, string[] Topics, string[] FilterExressions);
public record class PublicationProviderSession(string Id, string ListenerUrl, string[] Topics, string[] FilterExressions) : Session(Id, ListenerUrl, Topics, FilterExressions);
public record class PublicationConsumerSession(string Id, string ListenerUrl, string[] Topics, string[] FilterExressions) : Session(Id, ListenerUrl, Topics, FilterExressions);
public record class RequestProviderSession(string Id, string ListenerUrl, string[] Topics, string[] FilterExressions) : Session(Id, ListenerUrl, Topics, FilterExressions);
public record class RequestConsumerSession(string Id, string ListenerUrl, string[] Topics, string[] FilterExressions) : Session(Id, ListenerUrl, Topics, FilterExressions);