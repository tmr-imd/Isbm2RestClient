# Isbm2RestClient.Model.Message
Message Content may be XML, JSON, or possibly an arbitrary type. However, XML and JSON must be supported. When receiving a Message object as the result of a POST, MUST only include the message ID confirming the creation of the Message. The message type is implicit based on the context and MUST NOT appear in request/response bodies.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**MessageId** | **string** |  | [optional] 
**MessageType** | **MessageType** |  | [optional] 
**MessageContent** | [**MessageContent**](MessageContent.md) |  | [optional] 
**Topics** | **List&lt;string&gt;** | The Topic(s) to which the message will be posted. | [optional] 
**Expiry** | **string** | The duration after which the message will be automatically expired. Negative duration is no duration. Duration as defined by XML Schema xs:duration, http://w3c.org/TR/xmlschema-2/#duration | [optional] 
**RequestMessageId** | **string** | Only valid for Response messages; refers to the original Request message. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

