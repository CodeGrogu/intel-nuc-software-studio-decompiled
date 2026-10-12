using System.Windows;

namespace System.ComponentModel;

/// <summary>Provides a <see cref="T:System.Windows.WeakEventManager" /> implementation so that you can use the "weak event listener" pattern to attach listeners for the <see cref="E:System.ComponentModel.INotifyPropertyChanged.PropertyChanged" /> event.</summary>
public class PropertyChangedEventManager : WeakEventManager
{
	private PropertyChangedEventManager()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Adds the specified listener to the list of listeners on the specified source.</summary>
	/// <param name="source">The object with the event.</param>
	/// <param name="listener">The object to add as a listener.</param>
	/// <param name="propertyName">The name of the property that exists on <paramref name="source" /> upon which to listen for changes. Set to <see cref="F:System.String.Empty" /> to indicate "any property".</param>
	public static void AddListener(INotifyPropertyChanged source, IWeakEventListener listener, string propertyName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Removes the specified listener from the list of listeners on the provided source.</summary>
	/// <param name="source">The object to remove the listener from.</param>
	/// <param name="listener">The listener to remove.</param>
	/// <param name="propertyName">The name of the property that exists on <paramref name="source" /> upon which to stop listening for changes. Set to <see cref="F:System.String.Empty" /> to indicate "any property".</param>
	public static void RemoveListener(INotifyPropertyChanged source, IWeakEventListener listener, string propertyName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Adds the specified event handler, which is called when specified source raises the <see cref="E:System.ComponentModel.INotifyPropertyChanged.PropertyChanged" /> event for the specified property.</summary>
	/// <param name="source">The source object that the raises the <see cref="E:System.ComponentModel.INotifyPropertyChanged.PropertyChanged" /> event.</param>
	/// <param name="handler">The delegate that handles the <see cref="E:System.ComponentModel.INotifyPropertyChanged.PropertyChanged" /> event.</param>
	/// <param name="propertyName">The name of the property that exists on <paramref name="source" /> upon which to listen for changes. Set to <see cref="F:System.String.Empty" /> to indicate "any property".</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="handler" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.NotSupportedException">More than one method is associated with <paramref name="handler" />.</exception>
	public static void AddHandler(INotifyPropertyChanged source, EventHandler<PropertyChangedEventArgs> handler, string propertyName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Removes the specified event handler from the specified source.</summary>
	/// <param name="source">The source object that the raises the <see cref="E:System.ComponentModel.INotifyPropertyChanged.PropertyChanged" /> event.</param>
	/// <param name="handler">The delegate that handles the <see cref="E:System.ComponentModel.INotifyPropertyChanged.PropertyChanged" /> event.</param>
	/// <param name="propertyName">The name of the property that exists on <paramref name="source" /> upon which to stop listening for changes. Set to <see cref="F:System.String.Empty" /> to indicate "any property".</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="handler" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.NotSupportedException">More than one method is associated with <paramref name="handler" />.</exception>
	public static void RemoveHandler(INotifyPropertyChanged source, EventHandler<PropertyChangedEventArgs> handler, string propertyName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a new object to contain listeners to the <see cref="E:System.ComponentModel.INotifyPropertyChanged.PropertyChanged" /> event.</summary>
	/// <returns>A new object to contain listeners to the <see cref="E:System.ComponentModel.INotifyPropertyChanged.PropertyChanged" /> event.</returns>
	protected override ListenerList NewListenerList()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Begins listening for the <see cref="E:System.ComponentModel.INotifyPropertyChanged.PropertyChanged" /> event on the provided source.</summary>
	/// <param name="source">The object on which to start listening for <see cref="E:System.ComponentModel.INotifyPropertyChanged.PropertyChanged" />.</param>
	protected override void StartListening(object source)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Stops listening for the <see cref="E:System.ComponentModel.INotifyPropertyChanged.PropertyChanged" /> event on the provided source.</summary>
	/// <param name="source">The source object on which to stop listening for <see cref="E:System.ComponentModel.INotifyPropertyChanged.PropertyChanged" />.</param>
	protected override void StopListening(object source)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Removes listeners that are no longer active from the data for the provided source. </summary>
	/// <param name="source">Source object to remove all listeners from.</param>
	/// <param name="data">The data to purge. This is expected to be a <see cref="T:System.Collections.Specialized.HybridDictionary" />.</param>
	/// <param name="purgeAll">Set to <see langword="true" /> to stop listening and to completely remove all data.</param>
	/// <returns>
	///     <see langword="true" /> if some entries were removed; otherwise, <see langword="false" />.</returns>
	protected override bool Purge(object source, object data, bool purgeAll)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static PropertyChangedEventManager()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
