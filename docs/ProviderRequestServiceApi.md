# Isbm2RestClient.Api.ProviderRequestServiceApi

All URIs are relative to *http://localhost:80*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CloseSession**](ProviderRequestServiceApi.md#closesession) | **DELETE** /sessions/{session-id} | Closes a session. |
| [**OpenProviderRequestSession**](ProviderRequestServiceApi.md#openproviderrequestsession) | **POST** /channels/{channel-uri}/provider-request-sessions | Opens a provider request session for a channel for reading requests and posting responses. |
| [**PostResponse**](ProviderRequestServiceApi.md#postresponse) | **POST** /sessions/{session-id}/requests/{request-id}/responses | Posts a response message on a channel. |
| [**ReadRequest**](ProviderRequestServiceApi.md#readrequest) | **GET** /sessions/{session-id}/request | Returns the first non-expired request message or a previously read expired message that satisfies the session message filters. |
| [**RemoveRequest**](ProviderRequestServiceApi.md#removerequest) | **DELETE** /sessions/{session-id}/request | Deletes the first request message, if any, in the session message queue. |

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

            var apiInstance = new ProviderRequestServiceApi(config);
            var sessionId = "sessionId_example";  // string | The identifier of the session to be accessed (retrieved, deleted, modified, etc.)

            try
            {
                // Closes a session.
                apiInstance.CloseSession(sessionId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ProviderRequestServiceApi.CloseSession: " + e.Message);
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
    Debug.Print("Exception when calling ProviderRequestServiceApi.CloseSessionWithHttpInfo: " + e.Message);
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

<a name="openproviderrequestsession"></a>
# **OpenProviderRequestSession**
> Session OpenProviderRequestSession (string channelUri, Session? session = null)

Opens a provider request session for a channel for reading requests and posting responses.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class OpenProviderRequestSessionExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ProviderRequestServiceApi(config);
            var channelUri = "channelUri_example";  // string | The identifier of the channel to be accessed (retrieved, deleted, modified, etc.)
            var session = new Session?(); // Session? | The configuration of the session, i.e., topic filtering, content-filtering, and notication listener address. Only the Topics, ListenerURL, and FilterExpressions are to be provided. (optional) 

            try
            {
                // Opens a provider request session for a channel for reading requests and posting responses.
                Session result = apiInstance.OpenProviderRequestSession(channelUri, session);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ProviderRequestServiceApi.OpenProviderRequestSession: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the OpenProviderRequestSessionWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Opens a provider request session for a channel for reading requests and posting responses.
    ApiResponse<Session> response = apiInstance.OpenProviderRequestSessionWithHttpInfo(channelUri, session);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ProviderRequestServiceApi.OpenProviderRequestSessionWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **channelUri** | **string** | The identifier of the channel to be accessed (retrieved, deleted, modified, etc.) |  |
| **session** | [**Session?**](Session?.md) | The configuration of the session, i.e., topic filtering, content-filtering, and notication listener address. Only the Topics, ListenerURL, and FilterExpressions are to be provided. | [optional]  |

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
| **400** | Error in the provided parameters (e.g., ListenerURL not a valid URI) or duplicate namespaces prefixes in the NamespaceNames list of the FilterExpression. |  -  |
| **404** | The Channel does not exists. |  -  |
| **422** | The Channel is not of type Request. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postresponse"></a>
# **PostResponse**
> Message PostResponse (string sessionId, string requestId, Message? message = null)

Posts a response message on a channel.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class PostResponseExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ProviderRequestServiceApi(config);
            var sessionId = "sessionId_example";  // string | The identifier of the session to which the message will/is posted.
            var requestId = "requestId_example";  // string | The identifier of the origianal request for the response.
            var message = new Message?(); // Message? | The Message to be published. Only MessageContent is allowed in the request body. (optional) 

            try
            {
                // Posts a response message on a channel.
                Message result = apiInstance.PostResponse(sessionId, requestId, message);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ProviderRequestServiceApi.PostResponse: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostResponseWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Posts a response message on a channel.
    ApiResponse<Message> response = apiInstance.PostResponseWithHttpInfo(sessionId, requestId, message);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ProviderRequestServiceApi.PostResponseWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **sessionId** | **string** | The identifier of the session to which the message will/is posted. |  |
| **requestId** | **string** | The identifier of the origianal request for the response. |  |
| **message** | [**Message?**](Message?.md) | The Message to be published. Only MessageContent is allowed in the request body. | [optional]  |

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
| **201** | The message has been successfully posted to the channel. Returns only the MessageID. If there is no request message that can be matched to RequestMessageID, then no further action is taken. |  * Location - The URL at which the message can be accessed, expired, etc. <br>  |
| **404** | The session does not exist (or has been closed) or there are no messages to retrieve. |  -  |
| **422** | The Session is not of type Request Provider |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="readrequest"></a>
# **ReadRequest**
> Message ReadRequest (string sessionId)

Returns the first non-expired request message or a previously read expired message that satisfies the session message filters.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class ReadRequestExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ProviderRequestServiceApi(config);
            var sessionId = "sessionId_example";  // string | The identifier of the session to which the request message was posted.

            try
            {
                // Returns the first non-expired request message or a previously read expired message that satisfies the session message filters.
                Message result = apiInstance.ReadRequest(sessionId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ProviderRequestServiceApi.ReadRequest: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ReadRequestWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Returns the first non-expired request message or a previously read expired message that satisfies the session message filters.
    ApiResponse<Message> response = apiInstance.ReadRequestWithHttpInfo(sessionId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ProviderRequestServiceApi.ReadRequestWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **sessionId** | **string** | The identifier of the session to which the request message was posted. |  |

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
| **200** | Returns the first non-expired request message or previously read expired message that satisfies the session message filters. Only MessageID, MessageContent, and Topic are allowed in the response. Although &#x60;topics&#x60; is an array, it will only contain 1 value in the context of a request. **Note:** In contrast to the SOAP web-service, no message is returned as a 404 rather than an \&quot;empty\&quot; message. This maps better the a RESTful API that is based on the idea of resources. If there are no messages on the queue, the resource does not exist and, hence, 404 should be returned. |  -  |
| **404** | The session does not exist (or has been closed) or there are no messages to retrieve. |  -  |
| **422** | The Session is not of type Request Provider |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="removerequest"></a>
# **RemoveRequest**
> void RemoveRequest (string sessionId)

Deletes the first request message, if any, in the session message queue.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class RemoveRequestExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ProviderRequestServiceApi(config);
            var sessionId = "sessionId_example";  // string | The identifier of the session to which the request message was posted.

            try
            {
                // Deletes the first request message, if any, in the session message queue.
                apiInstance.RemoveRequest(sessionId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ProviderRequestServiceApi.RemoveRequest: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the RemoveRequestWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Deletes the first request message, if any, in the session message queue.
    apiInstance.RemoveRequestWithHttpInfo(sessionId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ProviderRequestServiceApi.RemoveRequestWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **sessionId** | **string** | The identifier of the session to which the request message was posted. |  |

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
| **204** | Request message has been removed from the session message queue. **Note:** This response applies even if no messages are in the queue. |  -  |
| **404** | The session does not exist (or has been closed) or there are no messages to retrieve. |  -  |
| **422** | The Session is not of type Request Provider |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

