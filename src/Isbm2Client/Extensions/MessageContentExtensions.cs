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
}
