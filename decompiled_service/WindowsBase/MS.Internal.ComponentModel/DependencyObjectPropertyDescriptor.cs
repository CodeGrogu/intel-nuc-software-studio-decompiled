using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;

namespace MS.Internal.ComponentModel;

internal sealed class DependencyObjectPropertyDescriptor : PropertyDescriptor
{
	public override AttributeCollection Attributes
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override Type ComponentType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override bool IsReadOnly
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override Type PropertyType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override bool SupportsChangeEvents
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal DependencyProperty DependencyProperty
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsAttached
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal PropertyMetadata Metadata
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static Type AttachedPropertyBrowsableAttributeType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal DependencyObjectPropertyDescriptor(PropertyDescriptor property, DependencyProperty dp, Type objectType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal DependencyObjectPropertyDescriptor(DependencyProperty dp, Type ownerType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override bool CanResetValue(object component)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override object GetValue(object component)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void ResetValue(object component)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void SetValue(object component, object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override bool ShouldSerializeValue(object component)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void AddValueChanged(object component, EventHandler handler)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void RemoveValueChanged(object component, EventHandler handler)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void ClearCache()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static MethodInfo GetAttachedPropertyMethod(DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected override AttributeCollection CreateAttributeCollection()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static DependencyObjectPropertyDescriptor()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
