using Isbm2Client.Extensions;
using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2Client.Service;
using Isbm2RestClient.Client;
using RestApi = Isbm2RestClient.Api;
using RestModel = Isbm2RestClient.Model;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace Isbm2Client.Test;

public class IsbmFaultRestTest
{
    public readonly string CHANNEL_URI = $"/isbm2restclient/test/publication/consumer/{Guid.NewGuid()}";
    public const string CHANNEL_DESCRIPTION = "For RestPublicationConsumerTest class";

    public readonly IOptions<ClientConfig> Config = Options.Create(new ClientConfig()
    {
        EndPoint = "https://isbm.lab.oiiecosystem.net/rest"
    });

    public PublicationChannel PublicationChannel { get; set; } = null!;
    public IProviderPublication Provider { get; set; } = null!;
    public IConsumerPublication Consumer { get; set; } = null!;

    public IsbmFaultRestTest() { }

    private Multimap<string, string> exampleHeaders()
    {
        var headers = new Multimap<string, string>();
        headers.Add("Content-Type", "application/json");
        return headers;
    }

    [Fact]
    public void ClientExceptionForLocalErrors()
    {
        IApiResponse response = new ApiResponse<object>((HttpStatusCode)0, new Multimap<string, string>(), "", "");
        response.ErrorText = "Connection timeout";

        var fault = IsbmFaultRestExtensions.IsbmFaultFactory("CreateChannel", response);

        Assert.NotNull(fault);
        Assert.IsType<ClientException>(fault);
    }

    [Theory]
    [InlineData(new object[] { HttpStatusCode.OK })]
    [InlineData(new object[] { HttpStatusCode.Created })]
    [InlineData(new object[] { HttpStatusCode.NoContent })]
    [InlineData(new object[] { HttpStatusCode.Redirect })]
    [InlineData(new object[] { HttpStatusCode.TemporaryRedirect })]
    [InlineData(new object[] { HttpStatusCode.PermanentRedirect })]
    [InlineData(new object[] { (HttpStatusCode)399 })]
    public void NoFaultForSuccessCodes(HttpStatusCode code)
    {
        IApiResponse response = new ApiResponse<object>(code,
            exampleHeaders(),
            new Dictionary<string, object>(),
            "{}"
        );

        var fault = IsbmFaultRestExtensions.IsbmFaultFactory("CreateChannel", response);

        Assert.Null(fault);
    }

    [Fact]
    public void UnknownFaultWhenFailureCodeButInvalidFaultContent()
    {
        IApiResponse response = new ApiResponse<object>(HttpStatusCode.BadRequest,
            exampleHeaders(),
            new Dictionary<string, object>(),
            "{}"
        );

        var fault = IsbmFaultRestExtensions.IsbmFaultFactory("CreateChannel", response);

        Assert.NotNull(fault);
        Assert.IsType<IsbmFault>(fault);
#pragma warning disable CS8600, CS8602 // Converting null literal or possible null value to non-nullable type.
        Assert.Equal(IsbmFaultType.Unknown, ((IsbmFault)fault).FaultType);
        Assert.Equal(response.RawContent, ((IsbmFault)fault).ServerFaultString);
#pragma warning restore CS8600, CS8602 // Converting null literal or possible null value to non-nullable type.
    }

    [Theory]
    [InlineData(new object[] { "ReadPublication" })]
    [InlineData(new object[] { "ReadRequest" })]
    [InlineData(new object[] { "ReadResponse" })]
    public void NoFaultOnReadMessageNotFound(string methodName)
    {
        IApiResponse response = new ApiResponse<object>(HttpStatusCode.NotFound,
            exampleHeaders(),
            new Dictionary<string, object>(),
            @"{""fault"": ""No messages.""}"
        );

        var fault = IsbmFaultRestExtensions.IsbmFaultFactory(methodName, response);

        Assert.Null(fault);
    }

    [Theory]
    [InlineData(new object[] { "ReadPublication" })]
    [InlineData(new object[] { "ReadRequest" })]
    [InlineData(new object[] { "ReadResponse" })]
    public void SessionFaultOnReadMessage(string methodName)
    {
        var restFault = new RestModel.SessionFault("Session not found."); // All faults serialise the same in JSON
        IApiResponse response = new ApiResponse<RestModel.SessionFault>(HttpStatusCode.NotFound,
            exampleHeaders(),
            restFault,
            restFault.ToJson()
        );

        var fault = IsbmFaultRestExtensions.IsbmFaultFactory(methodName, response);

        Assert.NotNull(fault);
        Assert.IsType<IsbmFault>(fault);
#pragma warning disable CS8600, CS8602 // Converting null literal or possible null value to non-nullable type.
        Assert.Equal(IsbmFaultType.SessionFault, ((IsbmFault)fault).FaultType);
        Assert.Equal(restFault.Fault, ((IsbmFault)fault).ServerFaultString);
#pragma warning restore CS8600, CS8602 // Converting null literal or possible null value to non-nullable type.
    }

    [Theory]
    [MemberData("ParameterFaultCombinations")]
    [MemberData("NamespaceFaultCombinations")]
    [MemberData("ChannelFaultCombinations")]
    [MemberData("SessionFaultCombinations")]
    [MemberData("OperationFaultCombinations")]
    [InlineData( new object[] { "RemoveSecurityTokens", "Given Security Tokens do not match", HttpStatusCode.Conflict, IsbmFaultType.SecurityTokenFault } )]
    [InlineData( new object[] { "GetSecurityDetails", "Some unexpected fault", HttpStatusCode.Unauthorized, IsbmFaultType.SecurityTokenFault } )]
    [InlineData( new object[] { "CreateChannel", "Some unexpected fault", HttpStatusCode.MethodNotAllowed, IsbmFaultType.Unknown } )]
    [InlineData( new object[] { "SomeNewOperation", "Some unexpected operation", HttpStatusCode.NotFound, IsbmFaultType.Unknown } )]
    public void IsbmFaultForStatusCodeAndOperation(string methodName, string faultString, HttpStatusCode statusCode, IsbmFaultType expectedFaultType)
    {
        var restFault = new RestModel.SessionFault(faultString);
        IApiResponse response = new ApiResponse<RestModel.SessionFault>(statusCode,
            exampleHeaders(),
            restFault,
            restFault.ToJson()
        );

        var fault = IsbmFaultRestExtensions.IsbmFaultFactory(methodName, response);

        Assert.NotNull(fault);
        Assert.IsType<IsbmFault>(fault);
#pragma warning disable CS8600, CS8602 // Converting null literal or possible null value to non-nullable type.
        Assert.Equal(expectedFaultType, ((IsbmFault)fault).FaultType);
        Assert.Equal(restFault.Fault, ((IsbmFault)fault).ServerFaultString);
#pragma warning restore CS8600, CS8602 // Converting null literal or possible null value to non-nullable type.
    }

    // Data methods

    public static IEnumerable<object[]> ParameterFaultCombinations()
    {
        return from methodName in AllIsbmOperationNames()
               select new object[] { methodName, "A Parameter is invalid", HttpStatusCode.BadRequest, IsbmFaultType.ParameterFault };
    }

    public static IEnumerable<object[]> NamespaceFaultCombinations()
    {
        return from methodName in new string[] { "OpenSubscriptionSession", "OpenProviderRequestSession" }
               select new object[] { methodName, "Duplicate namespace definition in filter", HttpStatusCode.BadRequest, IsbmFaultType.NamespaceFault };
    }

    public static IEnumerable<object[]> ChannelFaultCombinations()
    {
        return (
            from method in AllIsbmOperations()
            where (from param in method.GetParameters() select "channelUri".Equals(param.Name)).Any((x) => x)
            select new object[] { method.Name, "The channel does not exist", HttpStatusCode.NotFound, IsbmFaultType.ChannelFault }
        )
        .Append(
            new object[] { "CreateChannel", "Channel already exists", HttpStatusCode.Conflict, IsbmFaultType.ChannelFault }
        );
    }

    public static IEnumerable<object[]> SessionFaultCombinations()
    {
        return (
            from o in (
                from method in AllIsbmOperations()
                where (from param in method.GetParameters() select "sessionId".Equals(param.Name)).Any((x) => x)
                select new { method.Name, FaultString = "The session does not exist", StatusCode = HttpStatusCode.NotFound, FaultType = IsbmFaultType.SessionFault }
            ).Distinct() // exclude the duplicate close sessions
            select new object[] { o.Name, o.FaultString, o.StatusCode, o.FaultType }
        )
        .Union(
            from methodName in AllIsbmOperationNames()
            where methodName.EndsWith("Publication") || methodName.EndsWith("Request") || methodName.EndsWith("Response")
            select new object[] { methodName, "Session is of incorrect type", HttpStatusCode.UnprocessableEntity, IsbmFaultType.SessionFault }
        );
    }

    public static IEnumerable<object[]> OperationFaultCombinations()
    {
        return (
            from methodName in AllIsbmOperationNames()
            where (methodName.StartsWith("Open") && methodName.EndsWith("Session"))
            select new object[] { methodName, "The channel is of incorrect type", HttpStatusCode.UnprocessableEntity, IsbmFaultType.OperationFault }
        ).Append(
            new object[] { "AddSecurityTokens", "Security Tokens cannot be added", HttpStatusCode.Conflict, IsbmFaultType.OperationFault }
        );
    }

    public static IEnumerable<System.Reflection.MethodInfo> AllIsbmOperations()
    {
        return (
            from method in typeof(RestApi.IChannelManagementApiSync).GetMethods()
            where !method.ReturnType.IsAssignableTo(typeof(IApiResponse)) // exclude the WithHttpInfo variants
            select method
        ).Union(
            from method in typeof(RestApi.IConsumerPublicationServiceApiSync).GetMethods()
            where !method.ReturnType.IsAssignableTo(typeof(IApiResponse)) // exclude the WithHttpInfo variants
            select method
        ).Union(
            from method in typeof(RestApi.IConsumerRequestServiceApiSync).GetMethods()
            where !method.ReturnType.IsAssignableTo(typeof(IApiResponse)) // exclude the WithHttpInfo variants
            select method
        ).Union(
            from method in typeof(RestApi.IProviderPublicationServiceApiSync).GetMethods()
            where !method.ReturnType.IsAssignableTo(typeof(IApiResponse)) // exclude the WithHttpInfo variants
            select method
        ).Union(
            from method in typeof(RestApi.IProviderRequestServiceApiSync).GetMethods()
            where !method.ReturnType.IsAssignableTo(typeof(IApiResponse)) // exclude the WithHttpInfo variants
            select method
        );
    }

    public static IEnumerable<string> AllIsbmOperationNames()
    {
        return (
            from method in AllIsbmOperations()
            select method.Name
        ).Distinct();
    }
}