using System.ComponentModel;
using MS.Internal.WindowsBase;

namespace System.Windows;

/// <summary>This type supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code. </summary>
[TypeConverter(typeof(ExpressionConverter))]
public class Expression
{
	[FriendAccessAllowed]
	internal static readonly object NoValue;

	internal bool Attachable
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool Shareable
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool ForwardsInvalidations
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool SupportsUnboundSources
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool HasBeenAttached
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool HasBeenDetached
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal Expression()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal Expression(ExpressionMode mode)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal virtual Expression Copy(DependencyObject targetObject, DependencyProperty targetDP)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal virtual DependencySource[] GetSources()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal virtual object GetValue(DependencyObject d, DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal virtual bool SetValue(DependencyObject d, DependencyProperty dp, object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal virtual void OnAttach(DependencyObject d, DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal virtual void OnDetach(DependencyObject d, DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal virtual void OnPropertyInvalidation(DependencyObject d, DependencyPropertyChangedEventArgs args)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void ChangeSources(DependencyObject d, DependencyProperty dp, DependencySource[] newSources)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void MarkAttached()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void MarkDetached()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static Expression()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
