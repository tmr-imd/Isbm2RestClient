# Isbm2RestClient.Api.ConfigurationDiscoveryServiceApi

All URIs are relative to *http://localhost:80*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetSecurityDetails**](ConfigurationDiscoveryServiceApi.md#getsecuritydetails) | **GET** /configuration/security-details | Gets the detailed security related information of the ISBM service provider. The security details are exposed only if the connecting application provides a valid SecurityToken. Each application may be assigned a SecurityToken out-of-band by the service provider. |
| [**GetSupportedOperations**](ConfigurationDiscoveryServiceApi.md#getsupportedoperations) | **GET** /configuration/supported-operations | Gets information about the supported operations and features of the ISBM service provider. The purpose of this operation is to allow an application to be configured appropriately to communicate successfully with the service provider. |

<a name="getsecuritydetails"></a>
# **GetSecurityDetails**
> SecurityDetails GetSecurityDetails ()

Gets the detailed security related information of the ISBM service provider. The security details are exposed only if the connecting application provides a valid SecurityToken. Each application may be assigned a SecurityToken out-of-band by the service provider.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class GetSecurityDetailsExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            // Configure HTTP basic authorization: username_password
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new ConfigurationDiscoveryServiceApi(config);

            try
            {
                // Gets the detailed security related information of the ISBM service provider. The security details are exposed only if the connecting application provides a valid SecurityToken. Each application may be assigned a SecurityToken out-of-band by the service provider.
                SecurityDetails result = apiInstance.GetSecurityDetails();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ConfigurationDiscoveryServiceApi.GetSecurityDetails: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetSecurityDetailsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Gets the detailed security related information of the ISBM service provider. The security details are exposed only if the connecting application provides a valid SecurityToken. Each application may be assigned a SecurityToken out-of-band by the service provider.
    ApiResponse<SecurityDetails> response = apiInstance.GetSecurityDetailsWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ConfigurationDiscoveryServiceApi.GetSecurityDetailsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**SecurityDetails**](SecurityDetails.md)

### Authorization

[username_password](../README.md#username_password)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/xml


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Detailed security related information of the ISBM service provider. |  -  |
| **401** | The security tokens do not match those assigned to the application (SecurityTokenFault). |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsupportedoperations"></a>
# **GetSupportedOperations**
> SupportedOperations GetSupportedOperations ()

Gets information about the supported operations and features of the ISBM service provider. The purpose of this operation is to allow an application to be configured appropriately to communicate successfully with the service provider.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Isbm2RestClient.Api;
using Isbm2RestClient.Client;
using Isbm2RestClient.Model;

namespace Example
{
    public class GetSupportedOperationsExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:80";
            var apiInstance = new ConfigurationDiscoveryServiceApi(config);

            try
            {
                // Gets information about the supported operations and features of the ISBM service provider. The purpose of this operation is to allow an application to be configured appropriately to communicate successfully with the service provider.
                SupportedOperations result = apiInstance.GetSupportedOperations();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ConfigurationDiscoveryServiceApi.GetSupportedOperations: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetSupportedOperationsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Gets information about the supported operations and features of the ISBM service provider. The purpose of this operation is to allow an application to be configured appropriately to communicate successfully with the service provider.
    ApiResponse<SupportedOperations> response = apiInstance.GetSupportedOperationsWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ConfigurationDiscoveryServiceApi.GetSupportedOperationsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**SupportedOperations**](SupportedOperations.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/xml


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Operations supported by the ISBM service provider. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

