using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.IO;

/// <summary>The exception that is thrown when an input file or a data stream that is supposed to conform to a certain file format specification is malformed.</summary>
[Serializable]
public class FileFormatException : FormatException, ISerializable
{
	/// <summary>Gets the name of a file that caused the <see cref="T:System.IO.FileFormatException" />.</summary>
	/// <returns>A <see cref="T:System.Uri" /> that represents the name the file that caused the exception.</returns>
	public Uri SourceUri
	{
		[SecuritySafeCritical]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Creates a new instance of the <see cref="T:System.IO.FileFormatException" /> class.</summary>
	public FileFormatException()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a new instance of the <see cref="T:System.IO.FileFormatException" /> class with a specified error message.</summary>
	/// <param name="message">A <see cref="T:System.String" /> value that represents the error message.</param>
	public FileFormatException(string message)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a new instance of the <see cref="T:System.IO.FileFormatException" /> class with a specified error message and exception type.</summary>
	/// <param name="message">A <see cref="T:System.String" /> value that represents the error message.</param>
	/// <param name="innerException">The value of the <see cref="P:System.Exception.InnerException" /> property, which represents the cause of the current exception.</param>
	public FileFormatException(string message, Exception innerException)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a new instance of the <see cref="T:System.IO.FileFormatException" /> class with a source URI value.</summary>
	/// <param name="sourceUri">The <see cref="T:System.Uri" /> value of the file that caused this error.</param>
	public FileFormatException(Uri sourceUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a new instance of the <see cref="T:System.IO.FileFormatException" /> class with a source URI value and a specified error message.</summary>
	/// <param name="sourceUri">The <see cref="T:System.Uri" /> value of the file that caused this error.</param>
	/// <param name="message">A <see cref="T:System.String" /> value that represents the error message.</param>
	public FileFormatException(Uri sourceUri, string message)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a new instance of the <see cref="T:System.IO.FileFormatException" /> class with a source URI value and an exception type.</summary>
	/// <param name="sourceUri">The <see cref="T:System.Uri" /> value of the file that caused this error.</param>
	/// <param name="innerException">The value of the <see cref="P:System.Exception.InnerException" /> property, which represents the cause of the current exception.</param>
	public FileFormatException(Uri sourceUri, Exception innerException)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a new instance of the <see cref="T:System.IO.FileFormatException" /> class with a source URI value, a specified error message, and an exception type.</summary>
	/// <param name="sourceUri">The <see cref="T:System.Uri" /> value of the file that caused this error.</param>
	/// <param name="message">A <see cref="T:System.String" /> value that represents the error message.</param>
	/// <param name="innerException">The value of the <see cref="P:System.Exception.InnerException" /> property, which represents the cause of the current exception.</param>
	public FileFormatException(Uri sourceUri, string message, Exception innerException)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a new instance of the <see cref="T:System.IO.FileFormatException" /> class and initializes it with serialized data. This constructor is called during deserialization to reconstitute the exception object transmitted over a stream. </summary>
	/// <param name="info">The object that holds the serialized object data.</param>
	/// <param name="context">The contextual information about the source or destination.</param>
	protected FileFormatException(SerializationInfo info, StreamingContext context)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the file name and additional exception information.</summary>
	/// <param name="info">The object that holds the serialized object data.</param>
	/// <param name="context">The contextual information about the source or destination.</param>
	[SecurityCritical]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
