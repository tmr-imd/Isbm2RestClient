namespace BlazorServerExample.Pages;

public class RequestViewModel
{
    public string Endpoint { get; set; } = "http://blah.com";
    public string ChannelName { get; set; } = "/fred";
    public string Topic { get; set; } = "Yo!";

    public string Id { get; set; } = "";
    public string Message { get; set; } = "";
}
