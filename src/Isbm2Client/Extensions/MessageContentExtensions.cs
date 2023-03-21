using Isbm2Client.Model;
using RestModel = Isbm2RestClient.Model;

namespace Isbm2Client.Extensions;

public static class MessageContentExtensions
{
    public static RestModel.MessageContent ToRestMessageContent( this MessageContent messageContent )
    {
// I'm not sure why the CS8604 is necessary. The Isbm2RestClient project doesn't have `nullable=enable`. So
// RestModel.MessageContent.ContentEncoding should be equivalent to a `string?`
#pragma warning disable CS8604 // Possible null reference argument.
        return new RestModel.MessageContent
        (
            messageContent.MediaType, 
            messageContent.ContentEncoding, 
            new RestModel.MessageContentContent( messageContent.Content )
        );
#pragma warning restore CS8604 // Possible null reference argument.
    }

    /// <summary>
    /// Returns true if the RestModel.Message is actually a deserialized NotFound fault.
    /// </summary>
    /// <remarks>
    /// This occurs as the JSON schema for Message is flexible for different uses and
    /// future extensions, so an empty content successfully deserializes while "fault"
    /// content succesffuly deserializes as additional properties.
    /// </remarks>
    /// <returns>true if it is an empty or "fault" resut of NotFound</returns>
    public static bool NotFound( this RestModel.Message message)
    {
        return message.AdditionalProperties.ContainsKey("fault") || (
            message.MessageId is null
            && message.MessageContent is null
            && message.MessageType is null
            && message.Topics is null
            && message.Expiry is null
            && message.RequestMessageId is null
        );
    }
}
