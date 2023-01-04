# Isbm2RestClient.Api.ConsumerRequestServiceApi

All URIs are relative to *http://localhost:80*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CloseSession**](ConsumerRequestServiceApi.md#closesession) | **DELETE** /sessions/{session-id} | Closes a session. |
| [**ExpireRequest**](ConsumerRequestServiceApi.md#expirerequest) | **DELETE** /sessions/{session-id}/requests/{message-id} | Expires a posted request message. |
| [**OpenConsumerRequestSession**](ConsumerRequestServiceApi.md#openconsumerrequestsession) | **POST** /channels/{channel-uri}/consumer-request-sessions | Opens a consumer request session for a channel for posting requests and reading responses. |
| [**PostRequest**](ConsumerRequestServiceApi.md#postrequest) | **POST** /sessions/{session-id}/requests | Posts a request message on a channel. |
| [**ReadResponse**](ConsumerRequestServiceApi.md#readresponse) | **GET** /sessions/{session-id}/requests/{request-id}/response | Returns the first response message, if any, in the session message queue associated with the request. |
| [**RemoveResponse**](ConsumerRequestServiceApi.md#removeresponse) | **DELETE** /sessions/{session-id}/requests/{request-id}/response | Deletes the first response message, if any, in the session message queue associated with the request. |

<a name="closesession"></a>
# **CloseSession**
> void CloseSession (string sessionId)

Closes a session.

Closes a session of any type. All unexpired messages that have been posted during the session will be expired. ***Note*** This interface is shared by Close Publication Session, Close Subscription Session, Close Provider Request Session, and Close Consumer Request Session.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class CloseSessionExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ConsumerRequestServiceApi(config);
            var sessionId = "sessionId_example";  // string | The identifier of the session to be accessed (retrieved, deleted, modified, etc.)

            try
            {
                // Closes a session.
                apiInstance.CloseSession(sessionId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ConsumerRequestServiceApi.CloseSession: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CloseSessionWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Closes a session.
    apiInstance.CloseSessionWithHttpInfo(sessionId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ConsumerRequestServiceApi.CloseSessionWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **sessionId** | **string** | The identifier of the session to be accessed (retrieved, deleted, modified, etc.) |  |

### Return type

void (empty response body)

### Authorization

[username_password](../README.md#username_password)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/xml


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **204** | Session is successfully closed |  -  |
| **404** | The session does not exist or has been closed. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="expirerequest"></a>
# **ExpireRequest**
> void ExpireRequest (string sessionId, string messageId)

Expires a posted request message.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class ExpireRequestExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ConsumerRequestServiceApi(config);
            var sessionId = "sessionId_example";  // string | The identifier of the session to which the request message was posted.
            var messageId = "messageId_example";  // string | The identifier of the posted request.

            try
            {
                // Expires a posted request message.
                apiInstance.ExpireRequest(sessionId, messageId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ConsumerRequestServiceApi.ExpireRequest: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ExpireRequestWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Expires a posted request message.
    apiInstance.ExpireRequestWithHttpInfo(sessionId, messageId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ConsumerRequestServiceApi.ExpireRequestWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **sessionId** | **string** | The identifier of the session to which the request message was posted. |  |
| **messageId** | **string** | The identifier of the posted request. |  |

### Return type

void (empty response body)

### Authorization

[username_password](../README.md#username_password)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/xml


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **204** | Request has been expired. If the MessageID does not correspond with the SessionID or the corresponding message has already expired, then no further action is taken. |  -  |
| **404** | The session does not exist (or has been closed). |  -  |
| **422** | The Session is not of type Request Consumer |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="openconsumerrequestsession"></a>
# **OpenConsumerRequestSession**
> Session OpenConsumerRequestSession (string channelUri, Session? session = null)

Opens a consumer request session for a channel for posting requests and reading responses.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class OpenConsumerRequestSessionExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ConsumerRequestServiceApi(config);
            var channelUri = "channelUri_example";  // string | The identifier of the channel to be accessed (retrieved, deleted, modified, etc.)
            var session = new Session?(); // Session? | The configuration of the consumer request session, i.e., optional notication listener address. Only the ListenerURL is to be provided (if desired). (optional) 

            try
            {
                // Opens a consumer request session for a channel for posting requests and reading responses.
                Session result = apiInstance.OpenConsumerRequestSession(channelUri, session);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ConsumerRequestServiceApi.OpenConsumerRequestSession: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the OpenConsumerRequestSessionWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Opens a consumer request session for a channel for posting requests and reading responses.
    ApiResponse<Session> response = apiInstance.OpenConsumerRequestSessionWithHttpInfo(channelUri, session);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ConsumerRequestServiceApi.OpenConsumerRequestSessionWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **channelUri** | **string** | The identifier of the channel to be accessed (retrieved, deleted, modified, etc.) |  |
| **session** | [**Session?**](Session?.md) | The configuration of the consumer request session, i.e., optional notication listener address. Only the ListenerURL is to be provided (if desired). | [optional]  |

### Return type

[**Session**](Session.md)

### Authorization

[username_password](../README.md#username_password)

### HTTP request headers

 - **Content-Type**: application/json, application/xml
 - **Accept**: application/json, application/xml


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | The session has been successfully opened on the channel. Only the SessionID is to be returned. |  * Location - The URL at which the message can be accessed, expired, etc. <br>  |
| **400** | Error in the provided parameters (e.g., ChannelURI not a valid URI, ListenerURL is provided but not a valid URL). |  -  |
| **404** | The Channel does not exists. |  -  |
| **422** | The Channel is not of type Request. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postrequest"></a>
# **PostRequest**
> Message PostRequest (string sessionId, Message? message = null)

Posts a request message on a channel.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class PostRequestExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ConsumerRequestServiceApi(config);
            var sessionId = "sessionId_example";  // string | The identifier of the session to which the message will/is posted.
            var message = new Message?(); // Message? | The Message to be published Only MessageContent, Topic, and Expiry are allowed in the request body. Although `topics` is an array, at most 1 value is allowed. (optional) 

            try
            {
                // Posts a request message on a channel.
                Message result = apiInstance.PostRequest(sessionId, message);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ConsumerRequestServiceApi.PostRequest: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostRequestWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Posts a request message on a channel.
    ApiResponse<Message> response = apiInstance.PostRequestWithHttpInfo(sessionId, message);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ConsumerRequestServiceApi.PostRequestWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **sessionId** | **string** | The identifier of the session to which the message will/is posted. |  |
| **message** | [**Message?**](Message?.md) | The Message to be published Only MessageContent, Topic, and Expiry are allowed in the request body. Although &#x60;topics&#x60; is an array, at most 1 value is allowed. | [optional]  |

### Return type

[**Message**](Message.md)

### Authorization

[username_password](../README.md#username_password)

### HTTP request headers

 - **Content-Type**: application/json, application/xml
 - **Accept**: application/json, application/xml


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | The message has been successfully posted to the channel. Returns only the MessageID. |  * Location - The URL at which the message can be accessed, expired, etc. <br>  |
| **400** | Error in the provided parameters (e.g., no Topic provided, more than 1 topic provided, Expiry in invalid format). |  -  |
| **404** | The session does not exist (or has been closed). |  -  |
| **422** | The Session is not of type Request Consumer |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="readresponse"></a>
# **ReadResponse**
> Message ReadResponse (string sessionId, string requestId)

Returns the first response message, if any, in the session message queue associated with the request.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class ReadResponseExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ConsumerRequestServiceApi(config);
            var sessionId = "sessionId_example";  // string | The identifier of the session at which the response message was recieved.
            var requestId = "requestId_example";  // string | The identifier of the origianal request for the response.

            try
            {
                // Returns the first response message, if any, in the session message queue associated with the request.
                Message result = apiInstance.ReadResponse(sessionId, requestId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ConsumerRequestServiceApi.ReadResponse: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ReadResponseWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Returns the first response message, if any, in the session message queue associated with the request.
    ApiResponse<Message> response = apiInstance.ReadResponseWithHttpInfo(sessionId, requestId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ConsumerRequestServiceApi.ReadResponseWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **sessionId** | **string** | The identifier of the session at which the response message was recieved. |  |
| **requestId** | **string** | The identifier of the origianal request for the response. |  |

### Return type

[**Message**](Message.md)

### Authorization

[username_password](../README.md#username_password)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/xml


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The first response message associated with the request. Only MessageID and MessageContent are allowed in the response. **Note:** In contrast to the SOAP web-service, no message is returned as a 404 rather than an \&quot;empty\&quot; message. This maps better the a RESTful API that is based on the idea of resources. If there are no messages on the queue, the resource does not exist and, hence, 404 should be returned. |  -  |
| **404** | The session does not exist (or has been closed) or there are no messages to retrieve. |  -  |
| **422** | The Session is not of type Request Consumer |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="removeresponse"></a>
# **RemoveResponse**
> void RemoveResponse (string sessionId, string requestId)

Deletes the first response message, if any, in the session message queue associated with the request.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class RemoveResponseExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ConsumerRequestServiceApi(config);
            var sessionId = "sessionId_example";  // string | The identifier of the session at which the response message was recieved.
            var requestId = "requestId_example";  // string | The identifier of the origianal request for the response.

            try
            {
                // Deletes the first response message, if any, in the session message queue associated with the request.
                apiInstance.RemoveResponse(sessionId, requestId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ConsumerRequestServiceApi.RemoveResponse: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the RemoveResponseWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Deletes the first response message, if any, in the session message queue associated with the request.
    apiInstance.RemoveResponseWithHttpInfo(sessionId, requestId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ConsumerRequestServiceApi.RemoveResponseWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **sessionId** | **string** | The identifier of the session at which the response message was recieved. |  |
| **requestId** | **string** | The identifier of the origianal request for the response. |  |

### Return type

void (empty response body)

### Authorization

[username_password](../README.md#username_password)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/xml


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **204** | Response message has been removed from the session message queue associated with the request. **Note:** This response applies even if no messages are in the queue. |  -  |
| **404** | The session does not exist (or has been closed) or there are no messages to retrieve. |  -  |
| **422** | The Session is not of type Request Consumer |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

