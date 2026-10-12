namespace System.Windows.Threading;

/// <summary>Represents the Dispatcher when it is in a disable state and provides a means to re-enable dispatcher processing. </summary>
public struct DispatcherProcessingDisabled : IDisposable
{
	internal Dispatcher _dispatcher;

	/// <summary>Re-enables dispatcher processing.</summary>
	public void Dispose()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the specified <see cref="T:System.Windows.Threading.DispatcherProcessingDisabled" /> object is equal to this <see cref="T:System.Windows.Threading.DispatcherProcessingDisabled" /> object.</summary>
	/// <param name="obj">The object to evaluate for equality.</param>
	/// <returns>
	///     <see langword="true" /> if the specified object is equal to this <see cref="T:System.Windows.Threading.DispatcherProcessingDisabled" /> object; otherwise, <see langword="false" />.</returns>
	public override bool Equals(object obj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Gets a hash code for this instance. </summary>
	/// <returns>A signed 32-bit integer hash code.</returns>
	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether two <see cref="T:System.Windows.Threading.DispatcherProcessingDisabled" /> objects are equal.</summary>
	/// <param name="left">The first object to compare.</param>
	/// <param name="right">The second object to compare.</param>
	/// <returns>
	///     <see langword="true" /> if the <see cref="T:System.Windows.Threading.DispatcherProcessingDisabled" /> objects are equal; otherwise, <see langword="false" />.</returns>
	public static bool operator ==(DispatcherProcessingDisabled left, DispatcherProcessingDisabled right)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether two <see cref="T:System.Windows.Threading.DispatcherProcessingDisabled" /> objects are not equal.</summary>
	/// <param name="left">The first object to compare.</param>
	/// <param name="right">The second object to compare.</param>
	/// <returns>
	///     <see langword="true" /> if the <see cref="T:System.Windows.Threading.DispatcherProcessingDisabled" /> objects are not equal; otherwise, <see langword="false" />.</returns>
	public static bool operator !=(DispatcherProcessingDisabled left, DispatcherProcessingDisabled right)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
