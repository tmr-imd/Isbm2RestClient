namespace Isbm2Client.Model;

public enum IsbmFaultType {

    /// <summary>
    /// Represents an unknown fault. Not technically one of the ISBM Fault types but
    /// it provides a usable default value for the enumeration and supports unexpected 
    /// responses from the server.
    /// </summary>
    Unknown = 0,
    ChannelFault,
    NamespaceFault,
    OperationFault,
    ParameterFault,
    SecurityTokenFault,
    SessionFault
}

/// <summary>
/// Represents faults generated by the ISBM Server according the ISBM specification.
/// </summary>
/// <remarks>
/// <para>
/// The <c>FaultType</c> property indicates the specific ISBM fault type. This allows
/// use of catch filters over multiple catch clauses for each specific fault class.
/// </para>
/// <para>
/// The <c>ServerFaultString</c> can be used to capture the 'fault string' provided by 
/// the server itself independently of a local client error message.
/// </para>
/// </remarks>
[Serializable]
public class IsbmFault : Exception
{
    public IsbmFaultType FaultType { get; }
    public string? ServerFaultString { get; }

    /// <summary>
    /// Initializes a new instance of Isbm2Client.Model.IsbmFault with a specified
    /// ISBM fault type and optional fault string provided by the server, optional 
    /// client message, and optional inner exception that caused this fault.
    /// </summary>
    /// <remarks>
    /// Many underlying implementations interacting with the server raise exceptions.
    /// </remarks>
    /// <param name="faultType">The type of fault according to the ISBM specification.</param>
    /// <param name="serverFaultString">Fault string returned by the ISBM server</param>
    /// <param name="message">
    /// The error message that explains the reason for the exception.
    /// Default: 'Server responded: [serverFaultString]'
    /// </param>
    /// <param name="innerException">
    /// The exception that is the cause of the fault, as seen by the client.
    /// </param>
    public IsbmFault(IsbmFaultType faultType, string? serverFaultString = null, string? message = null, Exception? innerException = null)
        : base(message ?? $"Server responded: ${serverFaultString}", innerException)
    { 
        FaultType = faultType;
        ServerFaultString = serverFaultString;
    }

    /// <summary>
    /// Initializes a new instance of Isbm2Client.Model.IsbmFault.
    /// </summary>
    /// <remarks>
    /// This is here only for adherence to the basic Exception interface; 
    /// the IsbmFault specific constructors should be used instead.
    /// </remarks>
    public IsbmFault() : base()
    {
        FaultType = IsbmFaultType.Unknown;
        ServerFaultString = null;
    }

    /// <summary>
    /// Initializes a new instance of Isbm2Client.Model.IsbmFault with a specified error
    /// message.
    /// </summary>
    /// <remarks>
    /// This is here only for adherence to the basic Exception interface; 
    /// the IsbmFault specific constructors should be used instead.
    /// </remarks>
    /// <param name="message">
    /// The message that describes the error.
    /// </param>
    public IsbmFault(string? message) : base(message)
    {
        FaultType = IsbmFaultType.Unknown;
        ServerFaultString = null;
    }

    /// <summary>
    /// Initializes a new instance of Isbm2Client.Model.IsbmFault with a specified error
    /// message and a reference to the inner exception that is the cause of this exception. 
    /// </summary>
    /// <remarks>
    /// This is here only for adherence to the basic Exception interface; 
    /// the IsbmFault specific constructors should be used instead.
    /// </remarks>
    /// <param name="message">
    /// The error message that explains the reason for the exception.
    /// </param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception, or a null reference 
    /// (Nothing in Visual Basic) if no inner exception is specified.
    /// </param>
    public IsbmFault(string? message, Exception? innerException)
        : base(message, innerException)
    {
        FaultType = IsbmFaultType.Unknown;
        ServerFaultString = null;
    }
}

/// <summary>
/// Represents exceptions caused by the client implementation, as opposed to
/// ISBM Faults returned by the server.
/// </summary>
/// <example>
/// Failure to connect to the server is a typical case for a ClientException.
/// </example>
[Serializable]
public class ClientException : Exception
{
    /// <summary>
    /// Initializes a new instance of Isbm2Client.Model.ClientException.
    /// </summary>
    public ClientException() : base() { }

    /// <summary>
    /// Initializes a new instance of Isbm2Client.Model.ClientException with a specified error
    /// message.
    /// </summary>
    /// <param name="message">
    /// The message that describes the error.
    /// </param>
    public ClientException(string? message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of Isbm2Client.Model.ClientException with a specified error
    /// message and a reference to the inner exception that is the cause of this exception. 
    /// </summary>
    /// <param name="message">
    /// The error message that explains the reason for the exception.
    /// </param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception, or a null reference 
    /// (Nothing in Visual Basic) if no inner exception is specified.
    /// </param>
    public ClientException(string? message, Exception? innerException)
        : base(message, innerException) { }
}
