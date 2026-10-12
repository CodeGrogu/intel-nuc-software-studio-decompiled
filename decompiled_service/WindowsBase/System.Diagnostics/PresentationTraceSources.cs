using System.Runtime.CompilerServices;
using System.Windows;

namespace System.Diagnostics;

/// <summary>Provides debug tracing support that is specifically targeted for Windows Presentation Foundation (WPF) applications. </summary>
public static class PresentationTraceSources
{
	/// <summary>Identifies the <see cref="P:System.Diagnostics.PresentationTraceSources.TraceLevel" /> attached property.</summary>
	public static readonly DependencyProperty TraceLevelProperty;

	internal static TraceSource _DependencyPropertySource;

	internal static TraceSource _FreezableSource;

	internal static TraceSource _NameScopeSource;

	internal static TraceSource _RoutedEventSource;

	internal static TraceSource _AnimationSource;

	internal static TraceSource _DataBindingSource;

	internal static TraceSource _DocumentsSource;

	internal static TraceSource _ResourceDictionarySource;

	internal static TraceSource _MarkupSource;

	internal static TraceSource _HwndHostSource;

	internal static TraceSource _ShellSource;

	/// <summary>Gets a dependency property trace source.</summary>
	/// <returns>A dependency property trace source.</returns>
	public static TraceSource DependencyPropertySource
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a Freezable trace source.</summary>
	/// <returns>A Freezable trace source.</returns>
	public static TraceSource FreezableSource
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a name scope trace source.</summary>
	/// <returns>A name scope trace source.</returns>
	public static TraceSource NameScopeSource
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a routed event trace source.</summary>
	/// <returns>A routed event trace source.</returns>
	public static TraceSource RoutedEventSource
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets an animation trace source.</summary>
	/// <returns>An animation trace source.</returns>
	public static TraceSource AnimationSource
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a data-binding trace source.</summary>
	/// <returns>A data-binding trace source.</returns>
	public static TraceSource DataBindingSource
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a document trace source.</summary>
	/// <returns>A document trace source.</returns>
	public static TraceSource DocumentsSource
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a resource dictionary trace source.</summary>
	/// <returns>A resource dictionary trace source.</returns>
	public static TraceSource ResourceDictionarySource
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a markup trace source.</summary>
	/// <returns>A markup trace source.</returns>
	public static TraceSource MarkupSource
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets an hwnd host trace source.</summary>
	/// <returns>An hwnd host trace source.</returns>
	public static TraceSource HwndHostSource
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a shell trace source.</summary>
	/// <returns>A shell trace source.</returns>
	public static TraceSource ShellSource
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static event TraceRefreshEventHandler TraceRefresh
	{
		[CompilerGenerated]
		add
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[CompilerGenerated]
		remove
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the value of the <see cref="P:System.Diagnostics.PresentationTraceSources.TraceLevel" /> attached property for a specified element.</summary>
	/// <param name="element">The element from which the property value is read.</param>
	/// <returns>The <see cref="P:System.Diagnostics.PresentationTraceSources.TraceLevel" /> property value for the element.</returns>
	public static PresentationTraceLevel GetTraceLevel(object element)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Sets the value of the <see cref="P:System.Diagnostics.PresentationTraceSources.TraceLevel" /> attached property to a specified element.</summary>
	/// <param name="element">The element to which the attached property is written.</param>
	/// <param name="traceLevel">The needed <see cref="T:System.Diagnostics.PresentationTraceLevel" /> value.</param>
	public static void SetTraceLevel(object element, PresentationTraceLevel traceLevel)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Refreshes trace sources, by forcing the app.config file to be re-read.</summary>
	public static void Refresh()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static PresentationTraceSources()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
