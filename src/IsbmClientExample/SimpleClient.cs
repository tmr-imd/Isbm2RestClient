using System.Collections.Generic;
using System.Diagnostics;
using CommonMessageContent = Isbm2Client.Model.MessageContent;
using Isbm2Client.Extensions;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

using Newtonsoft.Json;

namespace SimpleIsbm2 {

class SimpleClient {

    public Configuration ClientConfig { get; private set; }

    private ChannelManagementApi _channelApi;
    private ConsumerPublicationServiceApi _subscriberApi;
    private ProviderPublicationServiceApi _publisherApi;

    public SimpleClient(Configuration config) {
        ClientConfig = config;
        _channelApi = new ChannelManagementApi(config);
        _subscriberApi = new ConsumerPublicationServiceApi(config);
        _publisherApi = new ProviderPublicationServiceApi(config);
    }

    public Channel CreateChannel(string channelUri, ChannelType channelType, string description) {
        var toBeChannel = new Channel(channelUri, channelType, description);
        Console.WriteLine("    {0}", toBeChannel.ToJson().ReplaceLineEndings("\n    "));
        return _channelApi.CreateChannel(toBeChannel);
    }

    public void DeleteChannel(string channelUri) {
        Console.WriteLine("Deleting channel {0}", channelUri);
        _channelApi.DeleteChannel(channelUri);
    }

    public Channel GetChannel(string channelUri) {
        return _channelApi.GetChannel(channelUri);
    }

    public Session OpenSubscriptionSession(string channelUri, IEnumerable<string> topics, string? listenerUrl = default(string?)) {
        var subscriptionParams = new Session(topics: new List<string>(topics), listenerUrl: listenerUrl);
        Console.WriteLine("\nOpening subscription session:\n    {0}", subscriptionParams.ToJson().ReplaceLineEndings("\n    "));
        var subscription = _subscriberApi.OpenSubscriptionSession(channelUri, subscriptionParams);
        subscription.SessionType = SessionType.PublicationConsumer;
        subscription.Topics = subscriptionParams.Topics;
        subscription.ListenerUrl = subscriptionParams.ListenerUrl;
        return subscription;
    }

    public Session OpenPublicationSession(string channelUri) {
        var session = _publisherApi.OpenPublicationSession(channelUri);
        session.SessionType = SessionType.PublicationProvider;
        return session;
    }

    public Message PostPublication<T>(string sessionId, T content, IEnumerable<string> topics, string? expiry = default(string)) where T : notnull {
        try {
            MessageContent messageContent = CommonMessageContent.From(content).ToRestMessageContent();

            var publicationParams = new Message(messageContent: messageContent, topics: new List<string>(topics), expiry: expiry, messageType: MessageType.Publication);
            Console.WriteLine("\nPosting publication:\n    {0}", publicationParams.ToJson().ReplaceLineEndings("\n    "));

            var publication = _publisherApi.PostPublication(sessionId, publicationParams);
            publicationParams.MessageId = publication.MessageId;
            return publicationParams;
        }
        catch (Exception e) when (e switch { NotImplementedException => true, InvalidCastException => true, System.NotSupportedException => true, _ => false }) {
            throw new ArgumentException("Message content must be a string, a JSONDocument, or an object that can be serialised to JSON using System.Text.Json.JsonSerializer.SerializeToDocument");
        }
    }

    public Message? ReadPublication(string sessionId) {
        Console.WriteLine("Reading publication from {0}", sessionId);
        try {
            var message = _subscriberApi.ReadPublication(sessionId);
            if (message == null) {
                throw new InvalidDataException("Unexpected failure deserialising read message. Message is null.");
            }
            message.MessageType = MessageType.Publication;
            return message;
        }
        catch (ApiException readError) {
            if (readError.ErrorCode == 404) return null;
            else throw readError;
        }
    }

    public bool? RemovePublication(string sessionId) {
        Console.WriteLine("Removing publication from {0}", sessionId);
        _subscriberApi.RemovePublication(sessionId);
        return true; // XXX: can try checking the count header field.
    }

    public IEnumerable<Message> ReadAllPublications(string sessionId) {
        List<Message> messages = new List<Message>();
        var readNext = true;
        while (readNext) {
            var message = ReadPublication(sessionId);
            if (readNext = message != null && (messages.Count == 0 || messages[messages.Count - 1].MessageId != message.MessageId)) {
                messages.Add(message ?? new Message());
                RemovePublication(sessionId);
            }
        }
        Console.WriteLine("Read {0} publication{1}", messages.Count, messages.Count == 1 ? "" : "s");
        return messages;
    }
}

}
