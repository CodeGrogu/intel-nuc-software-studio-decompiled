using MS.Internal.WindowsBase;

namespace System.Windows;

[FriendAccessAllowed]
internal struct EffectiveValueEntry
{
	private object _value;

	private short _propertyIndex;

	private FullValueSource _source;

	public int PropertyIndex
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal object Value
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal BaseValueSourceInternal BaseValueSourceInternal
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsDeferredReference
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		private set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsExpression
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		private set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsAnimated
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		private set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsCoerced
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		private set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool HasModifiers
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal FullValueSource FullValueSource
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool HasExpressionMarker
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsCoercedWithCurrentValue
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal object LocalValue
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal ModifiedValue ModifiedValue
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static EffectiveValueEntry CreateDefaultValueEntry(DependencyProperty dp, object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal EffectiveValueEntry(DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal EffectiveValueEntry(DependencyProperty dp, BaseValueSourceInternal valueSource)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal EffectiveValueEntry(DependencyProperty dp, FullValueSource fullValueSource)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void SetExpressionValue(object value, object baseValue)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void SetAnimatedValue(object value, object baseValue)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void SetCoercedValue(object value, object baseValue, bool skipBaseValueChecks, bool coerceWithCurrentValue)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void ResetAnimatedValue()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void ResetCoercedValue()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void ResetValue(object value, bool hasExpressionMarker)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void RestoreExpressionMarker()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal EffectiveValueEntry GetFlattenedEntry(RequestFlags requests)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void SetAnimationBaseValue(object animationBaseValue)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void SetCoersionBaseValue(object coersionBaseValue)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Clear()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
