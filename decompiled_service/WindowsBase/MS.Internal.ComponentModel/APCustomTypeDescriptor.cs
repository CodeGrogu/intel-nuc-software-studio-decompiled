using System;
using System.ComponentModel;
using System.Windows;

namespace MS.Internal.ComponentModel;

internal struct APCustomTypeDescriptor : ICustomTypeDescriptor
{
	private ICustomTypeDescriptor _parent;

	private DependencyObject _instance;

	internal APCustomTypeDescriptor(ICustomTypeDescriptor parent, object instance)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public PropertyDescriptorCollection GetProperties()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public AttributeCollection GetAttributes()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public string GetClassName()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public string GetComponentName()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public TypeConverter GetConverter()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public EventDescriptor GetDefaultEvent()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public PropertyDescriptor GetDefaultProperty()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public object GetEditor(Type editorBaseType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public EventDescriptorCollection GetEvents()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public EventDescriptorCollection GetEvents(Attribute[] attributes)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public object GetPropertyOwner(PropertyDescriptor property)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static APCustomTypeDescriptor()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
