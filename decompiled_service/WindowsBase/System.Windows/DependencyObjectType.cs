using MS.Internal.WindowsBase;

namespace System.Windows;

/// <summary>Implements an underlying type cache for all <see cref="T:System.Windows.DependencyObject" /> derived types.</summary>
public class DependencyObjectType
{
	/// <summary>Gets a zero-based unique identifier for constant-time array lookup operations.</summary>
	/// <returns>An internal identifier.</returns>
	public int Id
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary> Gets the common language runtime (CLR) system type represented by this <see cref="T:System.Windows.DependencyObjectType" />. </summary>
	/// <returns>The CLR system type represented by this <see cref="T:System.Windows.DependencyObjectType" />.</returns>
	public Type SystemType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the <see cref="T:System.Windows.DependencyObjectType" /> of the immediate base class of the current <see cref="T:System.Windows.DependencyObjectType" />. </summary>
	/// <returns>The type of the base class.</returns>
	public DependencyObjectType BaseType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the name of the represented common language runtime (CLR) system type. </summary>
	/// <returns>The name of the represented CLR system type.</returns>
	public string Name
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Returns a <see cref="T:System.Windows.DependencyObjectType" /> that represents a given system (CLR) type.</summary>
	/// <param name="systemType">The system (CLR) type to convert.</param>
	/// <returns>A <see cref="T:System.Windows.DependencyObjectType" /> that represents the system (CLR) type.</returns>
	public static DependencyObjectType FromSystemType(Type systemType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal static DependencyObjectType FromSystemTypeInternal(Type systemType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the specified object is an instance of the current <see cref="T:System.Windows.DependencyObjectType" />.</summary>
	/// <param name="dependencyObject">The object to compare with the current <see cref="T:System.Windows.DependencyObjectType" />.</param>
	/// <returns>
	///     <see langword="true" /> if the class represented by the current <see cref="T:System.Windows.DependencyObjectType" /> is in the inheritance hierarchy of the <see cref="T:System.Windows.DependencyObject" /> passed as <paramref name="d" />; otherwise, <see langword="false" />.</returns>
	public bool IsInstanceOfType(DependencyObject dependencyObject)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the current <see cref="T:System.Windows.DependencyObjectType" /> derives from the specified <see cref="T:System.Windows.DependencyObjectType" />.</summary>
	/// <param name="dependencyObjectType">The <see cref="T:System.Windows.DependencyObjectType" /> to compare.</param>
	/// <returns>
	///     <see langword="true" /> if the <paramref name="dependencyObjectType" /> parameter and the current <see cref="T:System.Windows.DependencyObjectType" /> represent types of classes, and the class represented by the current <see cref="T:System.Windows.DependencyObjectType" /> derives from the class represented by <paramref name="dependencyObjectType" />. Otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="dependencyObjectType" /> and the current <see cref="T:System.Windows.DependencyObjectType" /> represent the same class.</returns>
	public bool IsSubclassOf(DependencyObjectType dependencyObjectType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the hash code for this <see cref="T:System.Windows.DependencyObjectType" />.</summary>
	/// <returns>A 32-bit signed integer hash code. </returns>
	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	private DependencyObjectType()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static DependencyObjectType()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
