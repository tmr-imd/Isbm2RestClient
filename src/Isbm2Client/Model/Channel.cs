namespace Isbm2Client.Model; 

public abstract record Channel(string Uri, string Description);
public record class PublicationChannel(string Uri, string Description) : Channel(Uri, Description);
public record class RequestChannel(string Uri, string Description) : Channel(Uri, Description);
