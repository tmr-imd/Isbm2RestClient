# Isbm2RestClient.Api.MetadataApi

All URIs are relative to *http://localhost:80*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetMetadata**](MetadataApi.md#getmetadata) | **GET** /api | Get metadata from the root of the API |

<a name="getmetadata"></a>
# **GetMetadata**
> Object GetMetadata ()

Get metadata from the root of the API

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class GetMetadataExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            var apiInstance = new MetadataApi(config);

            try
            {
                // Get metadata from the root of the API
                Object result = apiInstance.GetMetadata();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MetadataApi.GetMetadata: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetMetadataWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get metadata from the root of the API
    ApiResponse<Object> response = apiInstance.GetMetadataWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MetadataApi.GetMetadataWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

**Object**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The OpenAPI document for the full suite of services available at this endpoint. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

