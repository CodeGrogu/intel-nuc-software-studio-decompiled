using System;
using System.ComponentModel;

namespace MS.Internal.ComponentModel;

internal struct DPCustomTypeDescriptor : ICustomTypeDescriptor
{
	private ICustomTypeDescriptor _parent;

	private Type _objectType;

	private object _instance;

	private const PropertyFilterOptions _anySet = PropertyFilterOptions.SetValues | PropertyFilterOptions.UnsetValues;

	private const PropertyFilterOptions _anyValid = PropertyFilterOptions.Valid;

	internal DPCustomTypeDescriptor(ICustomTypeDescriptor parent, Type objectType, object instance)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public string GetComponentName()
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

	internal static void ClearCache()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static DPCustomTypeDescriptor()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
