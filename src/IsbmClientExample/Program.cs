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

SimpleIsbm2.SimpleClient client = new SimpleIsbm2.SimpleClient(config);

try
{
    // Retrieve the channel, creating it if necessary.
    Channel channel;
    try {
        channel = client.GetChannel(channelUri);
    }
    catch (ApiException ex) {
        if (ex.ErrorCode != 404) throw ex;
        Console.WriteLine("Channel does not exist. Creating publication channel '{0}'", channelUri);
        channel = client.CreateChannel(channelUri, ChannelType.Publication, "A test channel");
    }
    Console.WriteLine("\nRetrieved channel:\n    {0}", channel.ToString().ReplaceLineEndings("\n    "));

    // Subscribe to the channel
    var subscription = client.OpenSubscriptionSession(channelUri, topics);
    Console.WriteLine("\nSubscription session opened:\n    {0}", subscription.ToJson().ReplaceLineEndings("\n    "));

    // Publish a message to the channel
    var session = client.OpenPublicationSession(channelUri);
    Console.WriteLine("\nPublication session opened:\n    {0}", session.ToJson().ReplaceLineEndings("\n    "));

    var contents = new object[] {
        "Hello World!",
        new Dictionary<string, object>(
            new KeyValuePair<string, object>[] {
                new KeyValuePair<string, object>("hello", "world"),
                new KeyValuePair<string, object>("someProperty", 1)
            }
        )
    };
    foreach(var content in contents) {
        var message = client.PostPublication(session.SessionId, content, topics, "PT1H");
        Console.WriteLine("Posted: {0}", message.MessageId);
    }

    // Read the message and remove it
    try {
        var messages = client.ReadAllPublications(subscription.SessionId);
        foreach (var message in messages) {
            Console.WriteLine("\nRead publication:\n    {0}", message.ToJson().ReplaceLineEndings("\n    "));
        }
    }
    catch(ApiException readError) {
        Console.WriteLine("Failed. Unexepected error {0}\n{1}", readError.Message, readError.StackTrace);
    }

    // Delete the channel if desired
    Console.WriteLine("Cleaning up.");
    client.DeleteChannel(channelUri);
}
catch (ApiException e)
{
    Console.WriteLine("Exception when calling SimpleClient: " + e.Message );
    Console.WriteLine("Status Code: "+ e.ErrorCode);
    Console.WriteLine(e.StackTrace);
}
