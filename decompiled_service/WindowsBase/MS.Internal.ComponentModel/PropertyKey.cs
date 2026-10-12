using System;
using System.Windows;

namespace MS.Internal.ComponentModel;

internal struct PropertyKey : IEquatable<PropertyKey>
{
	internal DependencyProperty DependencyProperty;

	internal Type AttachedType;

	private int _hashCode;

	internal PropertyKey(Type attachedType, DependencyProperty prop)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override bool Equals(object obj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public bool Equals(PropertyKey key)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public static bool operator ==(PropertyKey key1, PropertyKey key2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public static bool operator !=(PropertyKey key1, PropertyKey key2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
