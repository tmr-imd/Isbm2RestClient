# Isbm2RestClient.Api.ProviderPublicationServiceApi

All URIs are relative to *http://localhost:80*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CloseSession**](ProviderPublicationServiceApi.md#closesession) | **DELETE** /sessions/{session-id} | Closes a session. |
| [**ExpirePublication**](ProviderPublicationServiceApi.md#expirepublication) | **DELETE** /sessions/{session-id}/publications/{message-id} | Expires a posted publication. |
| [**OpenPublicationSession**](ProviderPublicationServiceApi.md#openpublicationsession) | **POST** /channels/{channel-uri}/publication-sessions | Opens a publication session for a channel. |
| [**PostPublication**](ProviderPublicationServiceApi.md#postpublication) | **POST** /sessions/{session-id}/publications | Posts a publication message on a channel. |

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

            var apiInstance = new ProviderPublicationServiceApi(config);
            var sessionId = "sessionId_example";  // string | The identifier of the session to be accessed (retrieved, deleted, modified, etc.)

            try
            {
                // Closes a session.
                apiInstance.CloseSession(sessionId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ProviderPublicationServiceApi.CloseSession: " + e.Message);
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
    Debug.Print("Exception when calling ProviderPublicationServiceApi.CloseSessionWithHttpInfo: " + e.Message);
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

<a name="expirepublication"></a>
# **ExpirePublication**
> void ExpirePublication (string sessionId, string messageId)

Expires a posted publication.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class ExpirePublicationExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ProviderPublicationServiceApi(config);
            var sessionId = "sessionId_example";  // string | The identifier of the session to which the publication was posted.
            var messageId = "messageId_example";  // string | The identifier of the posted publication.

            try
            {
                // Expires a posted publication.
                apiInstance.ExpirePublication(sessionId, messageId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ProviderPublicationServiceApi.ExpirePublication: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ExpirePublicationWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Expires a posted publication.
    apiInstance.ExpirePublicationWithHttpInfo(sessionId, messageId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ProviderPublicationServiceApi.ExpirePublicationWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **sessionId** | **string** | The identifier of the session to which the publication was posted. |  |
| **messageId** | **string** | The identifier of the posted publication. |  |

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
| **204** | Publication has been expired. If the MessageID does not correspond with the SessionID or the corresponding message has already expired, then no further action is taken. The message is expired for all topics associated with the message. |  -  |
| **404** | The session does not exist (or has been closed). |  -  |
| **422** | The Session is not of type Publication Consumer |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="openpublicationsession"></a>
# **OpenPublicationSession**
> Session OpenPublicationSession (string channelUri)

Opens a publication session for a channel.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class OpenPublicationSessionExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ProviderPublicationServiceApi(config);
            var channelUri = "channelUri_example";  // string | The identifier of the channel to be accessed (retrieved, deleted, modified, etc.)

            try
            {
                // Opens a publication session for a channel.
                Session result = apiInstance.OpenPublicationSession(channelUri);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ProviderPublicationServiceApi.OpenPublicationSession: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the OpenPublicationSessionWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Opens a publication session for a channel.
    ApiResponse<Session> response = apiInstance.OpenPublicationSessionWithHttpInfo(channelUri);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ProviderPublicationServiceApi.OpenPublicationSessionWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **channelUri** | **string** | The identifier of the channel to be accessed (retrieved, deleted, modified, etc.) |  |

### Return type

[**Session**](Session.md)

### Authorization

[username_password](../README.md#username_password)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/xml


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | The publication session has been successfully opened on the channel. Only the SessionID is to be returned. |  * Location - The URL at which the message can be accessed, expired, etc. <br>  |
| **400** | Error in the provided parameters (e.g., ChannelURI not a valid URI). |  -  |
| **404** | The Channel does not exists. |  -  |
| **422** | The Channel is not of type Publication. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postpublication"></a>
# **PostPublication**
> Message PostPublication (string sessionId, Message? message = null)

Posts a publication message on a channel.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class PostPublicationExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ProviderPublicationServiceApi(config);
            var sessionId = "sessionId_example";  // string | The identifier of the session to which the message will be posted.
            var message = new Message?(); // Message? | The Message to be published Only MessageContent, Topic, and Expiry are allowed in the request body. (optional) 

            try
            {
                // Posts a publication message on a channel.
                Message result = apiInstance.PostPublication(sessionId, message);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ProviderPublicationServiceApi.PostPublication: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostPublicationWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Posts a publication message on a channel.
    ApiResponse<Message> response = apiInstance.PostPublicationWithHttpInfo(sessionId, message);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ProviderPublicationServiceApi.PostPublicationWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **sessionId** | **string** | The identifier of the session to which the message will be posted. |  |
| **message** | [**Message?**](Message?.md) | The Message to be published Only MessageContent, Topic, and Expiry are allowed in the request body. | [optional]  |

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
| **400** | Error in the provided parameters (e.g., no Topic provided, Expiry in invalid format). |  -  |
| **404** | The session does not exist (or has been closed). |  -  |
| **422** | The Session is not of type Publication Consumer |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

