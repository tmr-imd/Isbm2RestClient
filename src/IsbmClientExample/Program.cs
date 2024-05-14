using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;
using System.Text.Json;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

Trace.Listeners.Add(new ConsoleTraceListener());

// Variables and defaults
var operation = "run all";
Dictionary<string, string> options = new Dictionary<string, string>();
var channelUri = "/example/test_channel/publish";
var topics = new List<string>(new string[]{"Test Topic"});

Configuration config = new()
{
    BasePath = "https://isbm.lab.oiiecosystem.net/rest",
    // Configure HTTP basic authorization: username_password
    Username = "YOUR_USERNAME",
    Password = "YOUR_PASSWORD",
    // Accept all SSL certificates with chain errors for the moment (we are not configuring any root certificates, so internal certificates cause failure)
    ServerCertificateValidationCallback = (object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors) => 
        sslPolicyErrors == SslPolicyErrors.None || sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors
};

// Reusable methods
var printUsage = () => {
    Console.WriteLine("Usage: IsbmClientExample.exe [URL OPERATION [OPTIONS...]]");
};

var validateBasePath = (string path) => {
    try {
        Uri uri = new Uri(path);
        if (!uri.Scheme.StartsWith("http")) return false;
        return true;
    }
    catch (UriFormatException) {
        return false;
    }
};

var validateOperation = (string operation) => {
    switch (operation) {
    case "create-channel":
    case "delete-channel":
    case "subscribe":
    case "cancel-subscription":
    case "open-publication-session":
    case "close-publication-session":
    case "post":
    case "read":
    case "open-request-session":
    case "close-request-session":
    case "open-response-session":
    case "close-response-session":
    case "close-sessions":
        return true;
    }
    return false;
};

var optionsForOperations = new Dictionary<string, Func<string[], int, Dictionary<string, string>>>();
optionsForOperations["create-channel"] = (string[] arguments, int start) => {
    var options = new Dictionary<string, string>();
    for (int i = start; i < arguments.Length; ++i) {
        if (!options.ContainsKey("ChannelUri")) {
            options["ChannelUri"] = new Uri(arguments[i], UriKind.Relative).ToString();
        }
        else {
            options["Description"] = arguments[i];
            break;
        }
    }
    return options;
};
optionsForOperations["delete-channel"] = optionsForOperations["create-channel"];
optionsForOperations["subscribe"] = (string[] arguments, int start) => {
    var options = new Dictionary<string, string>();
    for (int i = start; i < arguments.Length; ++i) {
        if (!options.ContainsKey("ChannelUri")) {
            options["ChannelUri"] = new Uri(arguments[i], UriKind.Relative).ToString();
        }
        else {
            options["Topics"] = options.ContainsKey("Topics") ? $"{options["Topics"]},{arguments[i]}" : arguments[i];
        }
    }
    return options;
};
optionsForOperations["cancel-subscription"] = (string[] arguments, int start) => {
    var options = new Dictionary<string, string>();
    for (int i = start; i < arguments.Length; ++i) {
        if (!options.ContainsKey("SessionId")) {
            options["SessionId"] = arguments[i];
            break;
        }
    }
    return options;
};
optionsForOperations["close-publication-session"] = optionsForOperations["cancel-subscription"];
optionsForOperations["close-request-session"] = optionsForOperations["cancel-subscription"];
optionsForOperations["close-response-session"] = optionsForOperations["cancel-subscription"];
optionsForOperations["close-sessions"] = (string[] arguments, int start) => {
    var options = new Dictionary<string, string>();
    for (int i = start; i < arguments.Length; ++i) {
        if (!options.ContainsKey("FilePath")) {
            options["FilePath"] = arguments[i];
            break;
        }
    }
    return options;
};
optionsForOperations["open-publication-session"] = (string[] arguments, int start) => {
    var options = new Dictionary<string, string>();
    for (int i = start; i < arguments.Length; ++i) {
        if (!options.ContainsKey("ChannelUri")) {
            options["ChannelUri"] = new Uri(arguments[i], UriKind.Relative).ToString();
            break;
        }
    }
    return options;
};
optionsForOperations["open-response-session"] = optionsForOperations["subscribe"];
optionsForOperations["open-request-session"] = optionsForOperations["open-publication-session"]; // although will need to support listener URL later
optionsForOperations["post"] = (string[] arguments, int start) => {
    var options = new Dictionary<string, string>();
    for (int i = start; i < arguments.Length; ++i) {
        if (!options.ContainsKey("SessionId")) {
            options["SessionId"] = arguments[i];
        }
        else if (!options.ContainsKey("ContentString") && !options.ContainsKey("ContentDict")) {
            var contentString = arguments[i];
            try {
                var content = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                options["ContentDict"] = Newtonsoft.Json.JsonConvert.SerializeObject(content);
            }
            catch (Exception) {
                options["ContentString"] = contentString;
            }
        }
        else {
            options["Topics"] = options.ContainsKey("Topics") ? $"{options["Topics"]},{arguments[i]}" : arguments[i];
        }
    }
    if (options.Count > 0) options["Expiry"] = "PT1H"; // Default expiry
    return options;
};
optionsForOperations["read"] = (string[] arguments, int start) => {
    var options = new Dictionary<string, string>();
    for (int i = start; i < arguments.Length; ++i) {
        if (!options.ContainsKey("SessionId")) {
            options["SessionId"] = arguments[i];
        }
        else if (arguments[i] == "all") {
            options["ReadAll"] = arguments[i];
        }
    }
    return options;
};

var methodsForOperations = new Dictionary<string, Action<Dictionary<string, string>, SimpleIsbm2.SimpleClient>>();
methodsForOperations["create-channel"] = (Dictionary<string, string> options, SimpleIsbm2.SimpleClient client) => {
    Console.WriteLine("Creating channel {0}...", channelUri);
    var channel = client.CreateChannel(channelUri, ChannelType.Publication, options.ContainsKey("Description") ? options["Description"] : "No description");
    client.GetChannel(channelUri);
    Console.WriteLine("Done");
};
methodsForOperations["delete-channel"] = (Dictionary<string, string> options, SimpleIsbm2.SimpleClient client) => {
    Console.Write("Deleting channel {0}...", channelUri);
    client.DeleteChannel(channelUri);
    Console.WriteLine("Done");
};
methodsForOperations["subscribe"] = (Dictionary<string, string> options, SimpleIsbm2.SimpleClient client) => {
    Console.WriteLine("Subscribing to {0} on {1}", options["Topics"], channelUri);
    var subscription = client.OpenSubscriptionSession(channelUri, options["Topics"].Split(","));
    Console.WriteLine("Subscription session opened:\n    {0}", subscription.ToJson().ReplaceLineEndings("\n    "));
};
methodsForOperations["cancel-subscription"] = (Dictionary<string, string> options, SimpleIsbm2.SimpleClient client) => {
    Console.Write("Closing subscription {0}...", options["SessionId"]);
    client.CloseSubscriptionSession(options["SessionId"]);
    Console.WriteLine("Done");
};
methodsForOperations["close-sessions"] = (Dictionary<string, string> options, SimpleIsbm2.SimpleClient client) => {
    Console.WriteLine("Closing sessions from file {0}...", options["FilePath"]);
    var sessions = new List<string>();
    using (var file = new FileStream(options["FilePath"], FileMode.Open))
    {
        sessions = JsonSerializer.Deserialize<List<string>>(JsonDocument.Parse(file));
    }
    sessions?.ForEach(s => {
        Console.Write("Closing session {0}...", s);
        try {
            client.CloseSubscriptionSession(s);
            Console.WriteLine("Done");
        } catch (Exception e)
        {
            Console.WriteLine("Failed with {0}", e.Message);
        }
    });
    Console.WriteLine("Done");
};
methodsForOperations["open-publication-session"] = (Dictionary<string, string> options, SimpleIsbm2.SimpleClient client) => {
    Console.WriteLine("Preparing to publish on {0}", channelUri);
    var session = client.OpenPublicationSession(channelUri);
    Console.WriteLine("Publication session opened:\n    {0}", session.ToJson().ReplaceLineEndings("\n    "));
};
methodsForOperations["close-publication-session"] = (Dictionary<string, string> options, SimpleIsbm2.SimpleClient client) => {
    Console.Write("Closing publication session {0}...", options["SessionId"]);
    client.ClosePublicationSession(options["SessionId"]);
    Console.WriteLine("Done");
};
methodsForOperations["open-response-session"] = (Dictionary<string, string> options, SimpleIsbm2.SimpleClient client) => {
    Console.WriteLine("Preparing to receive requests to {0} on {1}", options["Topics"], channelUri);
    var session = client.OpenResponseSession(channelUri, options["Topics"].Split(","));
    Console.WriteLine("Response session opened:\n    {0}", session.ToJson().ReplaceLineEndings("\n    "));
};
methodsForOperations["close-response-session"] = (Dictionary<string, string> options, SimpleIsbm2.SimpleClient client) => {
    Console.Write("Closing response session {0}...", options["SessionId"]);
    client.CloseResponseSession(options["SessionId"]);
    Console.WriteLine("Done");
};
methodsForOperations["open-request-session"] = (Dictionary<string, string> options, SimpleIsbm2.SimpleClient client) => {
    Console.WriteLine("Preparing to post requests on {0}", channelUri);
    var session = client.OpenRequestSession(channelUri);
    Console.WriteLine("Request session opened:\n    {0}", session.ToJson().ReplaceLineEndings("\n    "));
};
methodsForOperations["close-request-session"] = (Dictionary<string, string> options, SimpleIsbm2.SimpleClient client) => {
    Console.Write("Closing request session {0}...", options["SessionId"]);
    client.CloseRequestSession(options["SessionId"]);
    Console.WriteLine("Done");
};
methodsForOperations["post"] = (Dictionary<string, string> options, SimpleIsbm2.SimpleClient client) => {
    Console.WriteLine("Posting publication on {0}", options["SessionId"]);
    object? content = options.ContainsKey("ContentString")
                    ? options["ContentString"]
                    : Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(options["ContentDict"]);
    var message = client.PostPublication(options["SessionId"], content ?? "Invalid Content", options["Topics"].Split(","), options["Expiry"]);
    Console.WriteLine("Posted: {0}", message.MessageId);
};
methodsForOperations["read"] = (Dictionary<string, string> options, SimpleIsbm2.SimpleClient client) => {
    if (options.ContainsKey("ReadAll")) {
        var publications = client.ReadAllPublications(options["SessionId"]);
        foreach (var publication in publications) {
            Console.WriteLine("Read:\n{0}", publication.ToJson());
        }
    }
    else {
        var publication = client.ReadPublication(options["SessionId"]);
        Console.WriteLine("Read:\n{0}", publication?.ToJson() ?? "No messages");
        client.RemovePublication(options["SessionId"]);
    }
};
methodsForOperations["run all"] = (Dictionary<string, string> options, SimpleIsbm2.SimpleClient client) => {
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
};

// Process the commandline arguments
if (args.Length > 1 && args.Length < 3) {
    Console.WriteLine("Expected zero, 1, or at least 3 arguments but found {0}", args.Length);
    printUsage();
    return 1;
}

if (args.Length > 0) {
    // ISBM URL/BasePath
    if (validateBasePath(args[0])) {
        Uri basePathUri = new Uri(args[0]);
        config.BasePath = basePathUri.AbsoluteUri;
    }
    else {
        Console.WriteLine("Invalid URL `{0}`. ISBMv2 SimpleClient must be provided with a valid HTTP/S URL", args[0]);
        printUsage();
        return 1;
    }
}

if (args.Length > 1) {
    // operation
    if (validateOperation(args[1])) {
        operation = args[1];
        Console.WriteLine("Performing operation {0}", operation);
    }
    else {
        Console.WriteLine("Invalid Operation `{0}`. ISBMv2 SimpleClient supported operations are X, Y, Z", args[1]);
        printUsage();
        return 1;
    }

    // options for the operation
    if (!optionsForOperations.ContainsKey(operation)) {
        Console.WriteLine("Invalid options for operation.");
        printUsage();
        return 1;
    }
    options = optionsForOperations[operation](args, 2);
    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(options));

    if (options.Count == 0) {
        Console.WriteLine("Invalid options for operation.");
        printUsage();
        return 1;
    }
}

if (options.ContainsKey("ChannelUri")) channelUri = options["ChannelUri"];

Console.WriteLine("Base Path: {0}", config.BasePath);
Console.WriteLine("Channel URI: {0}", channelUri);

SimpleIsbm2.SimpleClient client = new SimpleIsbm2.SimpleClient(config);

try
{
    // Run a specific operation based on the commandline arguments
    if (!methodsForOperations.ContainsKey(operation)) {
        Console.WriteLine("Unexpected operation {0}. Should have already been validated.", operation);
        return 1;
    }
    methodsForOperations[operation](options, client);

    return 0;
}
catch (ApiException e)
{
    Console.WriteLine("Exception when calling SimpleClient: " + e.Message );
    Console.WriteLine("Status Code: "+ e.ErrorCode);
    Console.WriteLine(e.StackTrace);
    return 1;
}
