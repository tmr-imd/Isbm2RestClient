# Isbm2RestClient.Api.ConsumerPublicationServiceApi

All URIs are relative to *http://localhost:80*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CloseSession**](ConsumerPublicationServiceApi.md#closesession) | **DELETE** /sessions/{session-id} | Closes a session. |
| [**OpenSubscriptionSession**](ConsumerPublicationServiceApi.md#opensubscriptionsession) | **POST** /channels/{channel-uri}/subscription-sessions | Opens a subscription session for a channel. |
| [**ReadPublication**](ConsumerPublicationServiceApi.md#readpublication) | **GET** /sessions/{session-id}/publication | Returns the first non-expired publication message or a previously read expired message that satisfies the session message filters. |
| [**RemovePublication**](ConsumerPublicationServiceApi.md#removepublication) | **DELETE** /sessions/{session-id}/publication | Removes the first, if any, publication message in the subscription queue. |

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

            var apiInstance = new ConsumerPublicationServiceApi(config);
            var sessionId = "sessionId_example";  // string | The identifier of the session to be accessed (retrieved, deleted, modified, etc.)

            try
            {
                // Closes a session.
                apiInstance.CloseSession(sessionId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ConsumerPublicationServiceApi.CloseSession: " + e.Message);
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
    Debug.Print("Exception when calling ConsumerPublicationServiceApi.CloseSessionWithHttpInfo: " + e.Message);
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

<a name="opensubscriptionsession"></a>
# **OpenSubscriptionSession**
> Session OpenSubscriptionSession (string channelUri, Session? session = null)

Opens a subscription session for a channel.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class OpenSubscriptionSessionExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ConsumerPublicationServiceApi(config);
            var channelUri = "channelUri_example";  // string | The identifier of the channel to be accessed (retrieved, deleted, modified, etc.)
            var session = new Session?(); // Session? | The configuration of the subscription session, i.e., topic filtering, content-filtering, and notication listener address. Only the Topics, ListenerURL, and FilterExpressions are to be provided. (optional) 

            try
            {
                // Opens a subscription session for a channel.
                Session result = apiInstance.OpenSubscriptionSession(channelUri, session);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ConsumerPublicationServiceApi.OpenSubscriptionSession: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the OpenSubscriptionSessionWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Opens a subscription session for a channel.
    ApiResponse<Session> response = apiInstance.OpenSubscriptionSessionWithHttpInfo(channelUri, session);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ConsumerPublicationServiceApi.OpenSubscriptionSessionWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **channelUri** | **string** | The identifier of the channel to be accessed (retrieved, deleted, modified, etc.) |  |
| **session** | [**Session?**](Session?.md) | The configuration of the subscription session, i.e., topic filtering, content-filtering, and notication listener address. Only the Topics, ListenerURL, and FilterExpressions are to be provided. | [optional]  |

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
| **201** | The subscription session has been successfully opened on the channel. Only the SessionID is to be returned. |  * Location - The URL at which the message can be accessed, expired, etc. <br>  |
| **400** | Error in the provided parameters (e.g., ListenerURL not a valid URI) or duplicate namespaces prefixes in the NamespaceNames list of the FilterExpression. |  -  |
| **404** | The Channel does not exists. |  -  |
| **422** | The Channel is not of type Publication. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="readpublication"></a>
# **ReadPublication**
> Message ReadPublication (string sessionId)

Returns the first non-expired publication message or a previously read expired message that satisfies the session message filters.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class ReadPublicationExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ConsumerPublicationServiceApi(config);
            var sessionId = "sessionId_example";  // string | The identifier of the session to which the publication was posted.

            try
            {
                // Returns the first non-expired publication message or a previously read expired message that satisfies the session message filters.
                Message result = apiInstance.ReadPublication(sessionId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ConsumerPublicationServiceApi.ReadPublication: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ReadPublicationWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Returns the first non-expired publication message or a previously read expired message that satisfies the session message filters.
    ApiResponse<Message> response = apiInstance.ReadPublicationWithHttpInfo(sessionId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ConsumerPublicationServiceApi.ReadPublicationWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **sessionId** | **string** | The identifier of the session to which the publication was posted. |  |

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
| **200** | Returns the first non-expired publication message or a previously read expired message that satisfies the session message filters. Only MessageID, MessageContent, and Topic are allowed in the response. **Note:** In contrast to the SOAP web-service, no message is returned as a 404 rather than an \&quot;empty\&quot; message. This maps better the a RESTful API that is based on the idea of resources. If there are no messages on the queue, the resource does not exist and, hence, 404 should be returned. |  -  |
| **404** | The session does not exist (or has been closed) or there are no messages to retrieve. |  -  |
| **422** | The Session is not of type Publication Consumer |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="removepublication"></a>
# **RemovePublication**
> void RemovePublication (string sessionId)

Removes the first, if any, publication message in the subscription queue.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class RemovePublicationExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ConsumerPublicationServiceApi(config);
            var sessionId = "sessionId_example";  // string | The identifier of the session to which the publication was posted.

            try
            {
                // Removes the first, if any, publication message in the subscription queue.
                apiInstance.RemovePublication(sessionId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ConsumerPublicationServiceApi.RemovePublication: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the RemovePublicationWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Removes the first, if any, publication message in the subscription queue.
    apiInstance.RemovePublicationWithHttpInfo(sessionId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ConsumerPublicationServiceApi.RemovePublicationWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **sessionId** | **string** | The identifier of the session to which the publication was posted. |  |

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
| **204** | Publication message has been removed from the subscription queue. **Note:** This response applies even if no messages are in the queue. |  -  |
| **404** | The session does not exist (or has been closed) or there are no messages to retrieve. |  -  |
| **422** | The Session is not of type Publication Consumer |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

