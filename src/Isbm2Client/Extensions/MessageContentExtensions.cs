using Isbm2Client.Model;
using RestModel = Isbm2RestClient.Model;

namespace Isbm2Client.Extensions;

public static class MessageContentExtensions
{
    public static RestModel.MessageContent ToRestMessageContent( this MessageContent messageContent )
    {
        return new RestModel.MessageContent
        (
            messageContent.MediaType, 
            messageContent.ContentEncoding, 
            new RestModel.MessageContentContent( messageContent.Content )
        );
    }
}
