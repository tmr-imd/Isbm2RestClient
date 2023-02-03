namespace Isbm2Client.Model;

public abstract record class Session(string Id, string ListenerUrl, string[] Topics, string[] FilterExressions);
public record class PublicationProvider(string Id, string ListenerUrl, string[] Topics, string[] FilterExressions) : Session(Id, ListenerUrl, Topics, FilterExressions);
public record class PublicationConsumer(string Id, string ListenerUrl, string[] Topics, string[] FilterExressions) : Session(Id, ListenerUrl, Topics, FilterExressions);
public record class RequestProvider(string Id, string ListenerUrl, string[] Topics, string[] FilterExressions) : Session(Id, ListenerUrl, Topics, FilterExressions);
public record class RequestConsumer(string Id, string ListenerUrl, string[] Topics, string[] FilterExressions) : Session(Id, ListenerUrl, Topics, FilterExressions);