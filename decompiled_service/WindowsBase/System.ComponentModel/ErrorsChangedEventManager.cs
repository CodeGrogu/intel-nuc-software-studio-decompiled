using System.Windows;

namespace System.ComponentModel;

/// <summary>Provides a <see cref="T:System.Windows.WeakEventManager" /> implementation so that you can use the weak event listener pattern to attach listeners for the <see cref="E:System.ComponentModel.INotifyDataErrorInfo.ErrorsChanged" /> event.</summary>
public class ErrorsChangedEventManager : WeakEventManager
{
	private ErrorsChangedEventManager()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Adds the specified event handler, which is called when specified source raises the <see cref="E:System.ComponentModel.INotifyDataErrorInfo.ErrorsChanged" /> event.</summary>
	/// <param name="source">The source object that raises the <see cref="E:System.ComponentModel.INotifyDataErrorInfo.ErrorsChanged" /> event.</param>
	/// <param name="handler">The delegate that handles the <see cref="E:System.ComponentModel.INotifyDataErrorInfo.ErrorsChanged" /> event.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="handler" /> is <see langword="null" />.</exception>
	public static void AddHandler(INotifyDataErrorInfo source, EventHandler<DataErrorsChangedEventArgs> handler)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Removes the specified event handler from the specified source. </summary>
	/// <param name="source">The source object that raises the <see cref="E:System.ComponentModel.INotifyDataErrorInfo.ErrorsChanged" /> event.</param>
	/// <param name="handler">The delegate that handles the <see cref="E:System.ComponentModel.INotifyDataErrorInfo.ErrorsChanged" /> event.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="handler" /> is <see langword="null" />.</exception>
	public static void RemoveHandler(INotifyDataErrorInfo source, EventHandler<DataErrorsChangedEventArgs> handler)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a new object to contain listeners to the <see cref="E:System.ComponentModel.INotifyDataErrorInfo.ErrorsChanged" /> event.</summary>
	/// <returns>A new object to contain listeners to the <see cref="E:System.ComponentModel.INotifyDataErrorInfo.ErrorsChanged" /> event.</returns>
	protected override ListenerList NewListenerList()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Begins listening for the <see cref="E:System.ComponentModel.INotifyDataErrorInfo.ErrorsChanged" /> event on the specified source.</summary>
	/// <param name="source">The source object that raises the <see cref="E:System.ComponentModel.INotifyDataErrorInfo.ErrorsChanged" /> event.</param>
	protected override void StartListening(object source)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Stops listening for the <see cref="E:System.ComponentModel.INotifyDataErrorInfo.ErrorsChanged" /> event on the specified source.</summary>
	/// <param name="source">The source object that raises the <see cref="E:System.ComponentModel.INotifyDataErrorInfo.ErrorsChanged" /> event.</param>
	protected override void StopListening(object source)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
