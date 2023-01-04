using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

Trace.Listeners.Add(new ConsoleTraceListener());

Configuration config = new Configuration();
config.BasePath = "https://isbm.lab.oiiecosystem.net/rest";
// Configure HTTP basic authorization: username_password
config.Username = "YOUR_USERNAME";
config.Password = "YOUR_PASSWORD";

var apiInstance = new ChannelManagementApi(config);
var subscriberApi = new ConsumerPublicationServiceApi(config);
var publisherApi = new ProviderPublicationServiceApi(config);
var channelUri = "/example/test_channel/publish";
var topics = new List<string>(new string[]{"Test Topic"});
var requestBody = new List<Dictionary<String, Object>>();

Console.WriteLine("Base Path: {0}", apiInstance.GetBasePath());
Console.WriteLine("Channel URI: {0}", channelUri);

try
{
    // Retrieve the channel, creating it if necessary.
    Channel channel;
    try {
        channel = apiInstance.GetChannel(channelUri);
    }
    catch (ApiException ex) {
        if (ex.ErrorCode != 404) throw ex;
        Console.WriteLine("Channel does not exist. Creating publication channel '{0}'", channelUri);
        Trace.Indent();
        var toBeChannel = new Channel(channelUri, ChannelType.Publication, "A test channel");
        Console.WriteLine("    {0}", toBeChannel.ToJson().ReplaceLineEndings("\n    "));
        Trace.Unindent();

        channel = apiInstance.CreateChannel(toBeChannel);
    }
    Console.WriteLine("\nRetrieved channel:\n    {0}", channel.ToString().ReplaceLineEndings("\n    "));

    // Subscribe to the channel
    var subscription = new Session();
    subscription.Topics = new List<string>(topics);
    Console.WriteLine("\nOpening subscription session:\n    {0}", subscription.ToJson().ReplaceLineEndings("\n    "));
    subscription = subscriberApi.OpenSubscriptionSession(channelUri, subscription);
    Console.WriteLine("\nSubscription session opened:\n    {0}", subscription.ToJson().ReplaceLineEndings("\n    "));

    // Publish a message to the channel
    var session = publisherApi.OpenPublicationSession(channelUri);
    Console.WriteLine("\nPublication session opened:\n    {0}", session.ToJson().ReplaceLineEndings("\n    "));
    var content = new MessageContent[] {
        new MessageContent(mediaType: "text/plain", content: new MessageContentContent("Hello World!")),
        new MessageContent(mediaType: "application/json", content: new MessageContentContent(new Dictionary<string, object>(
            new KeyValuePair<string, object>[] {
                new KeyValuePair<string, object>("hello", "world"),
                new KeyValuePair<string, object>("someProperty", 1)
            }
        )))
    };
    foreach(var c in content) {
        var publication = new Message(messageContent: c, topics: new List<string>(topics), expiry: "PT1H", messageType: MessageType.Publication);
        Console.WriteLine("\nPosting publication:\n    {0}", publication.ToJson().ReplaceLineEndings("\n    "));
        publisherApi.PostPublication(session.SessionId, publication);
    }

    // Read the message and remove it
    Console.WriteLine("\nReading publication:");
    var readMessage = subscriberApi.ReadPublication(subscription.SessionId);
    Console.WriteLine("\n    {0}", readMessage?.ToJson()?.ReplaceLineEndings("\n    "));
    Console.Write("Removing publication... ");
    subscriberApi.RemovePublication(subscription.SessionId);
    string? removeResult = null;
    try {
        var readAgain = subscriberApi.ReadPublication(subscription.SessionId);
        removeResult = readAgain.MessageId == readMessage.MessageId
            ? "Failed. Message not removed."
            : "Success. Next message in queue.";
    }
    catch(ApiException readError) {
        removeResult = readError.ErrorCode != 404
            ? string.Format("Failed. Unexepected error {0}\n{1}", readError.Message, readError.StackTrace)
            : "Success. No messages in queue";
    }
    removeResult ??= "Unexpected outcome";
    Console.WriteLine(removeResult);

    // Delete the channel if desired
    Console.WriteLine("Cleaning up.");
    apiInstance.DeleteChannel(channelUri);
}
catch (ApiException e)
{
    Console.WriteLine("Exception when calling ChannelManagementApi: " + e.Message );
    Console.WriteLine("Status Code: "+ e.ErrorCode);
    Console.WriteLine(e.StackTrace);
}
