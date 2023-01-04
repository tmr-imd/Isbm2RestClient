# Isbm2RestClient.Api.ChannelManagementApi

All URIs are relative to *http://localhost:80*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**AddSecurityTokens**](ChannelManagementApi.md#addsecuritytokens) | **POST** /channels/{channel-uri}/security-tokens | Adds security tokens to a channel. |
| [**CreateChannel**](ChannelManagementApi.md#createchannel) | **POST** /channels | Create a new channel with the specified URI path fragment. |
| [**DeleteChannel**](ChannelManagementApi.md#deletechannel) | **DELETE** /channels/{channel-uri} | Delete the Channel specified by &#39;channel-uri&#39; |
| [**GetChannel**](ChannelManagementApi.md#getchannel) | **GET** /channels/{channel-uri} | Retrieve the Channel identified by &#39;channel-uri&#39; |
| [**GetChannels**](ChannelManagementApi.md#getchannels) | **GET** /channels | Retrieve all the channels, subject to security permissions. |
| [**RemoveSecurityTokens**](ChannelManagementApi.md#removesecuritytokens) | **DELETE** /channels/{channel-uri}/security-tokens | Removes security tokens from a channel. |

<a name="addsecuritytokens"></a>
# **AddSecurityTokens**
> void AddSecurityTokens (string channelUri, List<Dictionary>? requestBody = null)

Adds security tokens to a channel.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class AddSecurityTokensExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ChannelManagementApi(config);
            var channelUri = "channelUri_example";  // string | The identifier of the channel to be accessed (retrieved, deleted, modified, etc.)
            var requestBody = new List<Dictionary>?(); // List<Dictionary>? | The SecurityTokens to add. (optional) 

            try
            {
                // Adds security tokens to a channel.
                apiInstance.AddSecurityTokens(channelUri, requestBody);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChannelManagementApi.AddSecurityTokens: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the AddSecurityTokensWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Adds security tokens to a channel.
    apiInstance.AddSecurityTokensWithHttpInfo(channelUri, requestBody);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChannelManagementApi.AddSecurityTokensWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **channelUri** | **string** | The identifier of the channel to be accessed (retrieved, deleted, modified, etc.) |  |
| **requestBody** | [**List&lt;Dictionary&gt;?**](Dictionary.md) | The SecurityTokens to add. | [optional]  |

### Return type

void (empty response body)

### Authorization

[username_password](../README.md#username_password)

### HTTP request headers

 - **Content-Type**: application/json, application/xml
 - **Accept**: application/json, application/xml


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | Tokens have been added to the Channel. |  -  |
| **404** | The requested Channel does not exist. |  -  |
| **409** | The requested Channel has no security tokens and must not be assigned any. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="createchannel"></a>
# **CreateChannel**
> Channel CreateChannel (Channel? channel = null)

Create a new channel with the specified URI path fragment.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class CreateChannelExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            var apiInstance = new ChannelManagementApi(config);
            var channel = new Channel?(); // Channel? | The Channel to create (optional) 

            try
            {
                // Create a new channel with the specified URI path fragment.
                Channel result = apiInstance.CreateChannel(channel);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChannelManagementApi.CreateChannel: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateChannelWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create a new channel with the specified URI path fragment.
    ApiResponse<Channel> response = apiInstance.CreateChannelWithHttpInfo(channel);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChannelManagementApi.CreateChannelWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **channel** | [**Channel?**](Channel?.md) | The Channel to create | [optional]  |

### Return type

[**Channel**](Channel.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json, application/xml
 - **Accept**: application/json, application/xml


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | The newly created Channel, excluding any configured security tokens. |  -  |
| **409** | Could not create the channel, URI already exists. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletechannel"></a>
# **DeleteChannel**
> void DeleteChannel (string channelUri)

Delete the Channel specified by 'channel-uri'

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class DeleteChannelExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ChannelManagementApi(config);
            var channelUri = "channelUri_example";  // string | The identifier of the channel to be accessed (retrieved, deleted, modified, etc.)

            try
            {
                // Delete the Channel specified by 'channel-uri'
                apiInstance.DeleteChannel(channelUri);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChannelManagementApi.DeleteChannel: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteChannelWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete the Channel specified by 'channel-uri'
    apiInstance.DeleteChannelWithHttpInfo(channelUri);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChannelManagementApi.DeleteChannelWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **channelUri** | **string** | The identifier of the channel to be accessed (retrieved, deleted, modified, etc.) |  |

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
| **204** | Channel successfully deleted. |  -  |
| **404** | The requested Channel does not exist. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getchannel"></a>
# **GetChannel**
> Channel GetChannel (string channelUri)

Retrieve the Channel identified by 'channel-uri'

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class GetChannelExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ChannelManagementApi(config);
            var channelUri = "channelUri_example";  // string | The identifier of the channel to be accessed (retrieved, deleted, modified, etc.)

            try
            {
                // Retrieve the Channel identified by 'channel-uri'
                Channel result = apiInstance.GetChannel(channelUri);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChannelManagementApi.GetChannel: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetChannelWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve the Channel identified by 'channel-uri'
    ApiResponse<Channel> response = apiInstance.GetChannelWithHttpInfo(channelUri);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChannelManagementApi.GetChannelWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **channelUri** | **string** | The identifier of the channel to be accessed (retrieved, deleted, modified, etc.) |  |

### Return type

[**Channel**](Channel.md)

### Authorization

[username_password](../README.md#username_password)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/xml


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request Channel; excluding associated SecurityTokens. |  -  |
| **404** | The requested Channel does not exist. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getchannels"></a>
# **GetChannels**
> List&lt;Channel&gt; GetChannels ()

Retrieve all the channels, subject to security permissions.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class GetChannelsExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ChannelManagementApi(config);

            try
            {
                // Retrieve all the channels, subject to security permissions.
                List<Channel> result = apiInstance.GetChannels();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChannelManagementApi.GetChannels: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetChannelsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all the channels, subject to security permissions.
    ApiResponse<List<Channel>> response = apiInstance.GetChannelsWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChannelManagementApi.GetChannelsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**List&lt;Channel&gt;**](Channel.md)

### Authorization

[username_password](../README.md#username_password)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/xml


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A (possibly empty) list of Channels. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="removesecuritytokens"></a>
# **RemoveSecurityTokens**
> void RemoveSecurityTokens (string channelUri, List<Dictionary>? requestBody = null)

Removes security tokens from a channel.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class RemoveSecurityTokensExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ChannelManagementApi(config);
            var channelUri = "channelUri_example";  // string | The identifier of the channel to be accessed (retrieved, deleted, modified, etc.)
            var requestBody = new List<Dictionary>?(); // List<Dictionary>? | The security tokens to remove: each token must be specified in full to be removed, i.e., specifying only the username of a UsernamePassword token is insufficient. (optional) 

            try
            {
                // Removes security tokens from a channel.
                apiInstance.RemoveSecurityTokens(channelUri, requestBody);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChannelManagementApi.RemoveSecurityTokens: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the RemoveSecurityTokensWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Removes security tokens from a channel.
    apiInstance.RemoveSecurityTokensWithHttpInfo(channelUri, requestBody);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChannelManagementApi.RemoveSecurityTokensWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **channelUri** | **string** | The identifier of the channel to be accessed (retrieved, deleted, modified, etc.) |  |
| **requestBody** | [**List&lt;Dictionary&gt;?**](Dictionary.md) | The security tokens to remove: each token must be specified in full to be removed, i.e., specifying only the username of a UsernamePassword token is insufficient. | [optional]  |

### Return type

void (empty response body)

### Authorization

[username_password](../README.md#username_password)

### HTTP request headers

 - **Content-Type**: application/json, application/xml
 - **Accept**: application/json, application/xml


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **204** | Security tokens successfully removed from the channel. |  -  |
| **404** | The requested Channel does not exist. |  -  |
| **409** | The security tokens do not match those assigned to the Channel. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

