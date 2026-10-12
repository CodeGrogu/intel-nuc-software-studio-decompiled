using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Security.RightsManagement;

/// <summary>Represents an error condition when a rights management operation cannot complete successfully.</summary>
[Serializable]
public class RightsManagementException : Exception
{
	/// <summary>Gets the <see cref="T:System.Security.RightsManagement.RightsManagementFailureCode" /> for the error.</summary>
	/// <returns>The failure code for the error.</returns>
	public RightsManagementFailureCode FailureCode
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.RightsManagement.RightsManagementException" /> class.</summary>
	public RightsManagementException()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.RightsManagement.RightsManagementException" /> class with a given message.</summary>
	/// <param name="message">A message that describes the error.</param>
	public RightsManagementException(string message)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.RightsManagement.RightsManagementException" /> class with a given <see cref="P:System.Exception.Message" /> and <see cref="P:System.Exception.InnerException" />.</summary>
	/// <param name="message">A message that describes the error.</param>
	/// <param name="innerException">The exception instance that caused this exception.</param>
	public RightsManagementException(string message, Exception innerException)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.RightsManagement.RightsManagementException" /> class with a given <see cref="T:System.Security.RightsManagement.RightsManagementFailureCode" />.</summary>
	/// <param name="failureCode">The failure code for the error.</param>
	public RightsManagementException(RightsManagementFailureCode failureCode)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.RightsManagement.RightsManagementException" /> class with a given <see cref="T:System.Security.RightsManagement.RightsManagementFailureCode" /> and <see cref="P:System.Exception.Message" />.</summary>
	/// <param name="failureCode">The failure code for the error.</param>
	/// <param name="message">A message that describes the error.</param>
	public RightsManagementException(RightsManagementFailureCode failureCode, string message)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.RightsManagement.RightsManagementException" /> class with a given <see cref="T:System.Security.RightsManagement.RightsManagementFailureCode" /> and <see cref="P:System.Exception.InnerException" />.</summary>
	/// <param name="failureCode">The failure code for the error.</param>
	/// <param name="innerException">The exception instance that caused the error.</param>
	public RightsManagementException(RightsManagementFailureCode failureCode, Exception innerException)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.RightsManagement.RightsManagementException" /> class with a given <see cref="T:System.Security.RightsManagement.RightsManagementFailureCode" />, <see cref="P:System.Exception.Message" /> and <see cref="P:System.Exception.InnerException" />.</summary>
	/// <param name="failureCode">The failure code for the error.</param>
	/// <param name="message">A message that describes the error.</param>
	/// <param name="innerException">The exception instance that caused the error.</param>
	public RightsManagementException(RightsManagementFailureCode failureCode, string message, Exception innerException)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.RightsManagement.RightsManagementException" /> class and sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store with information about the exception.</summary>
	/// <param name="info">The object that holds the serialized data.</param>
	/// <param name="context">The contextual information about the source or destination.</param>
	protected RightsManagementException(SerializationInfo info, StreamingContext context)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store with the parameter name and information about the exception.</summary>
	/// <param name="info">The object that holds the serialized data.</param>
	/// <param name="context">The contextual information about the source or destination.</param>
	/// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is <see langword="null" />.</exception>
	[SecurityCritical]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
