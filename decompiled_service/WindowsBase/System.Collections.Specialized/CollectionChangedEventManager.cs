using System.Windows;

namespace System.Collections.Specialized;

/// <summary>Provides a <see cref="T:System.Windows.WeakEventManager" /> implementation so that you can use the "weak event listener" pattern to attach listeners for the <see cref="E:System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged" /> event.</summary>
public class CollectionChangedEventManager : WeakEventManager
{
	private CollectionChangedEventManager()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Adds the specified listener to the <see cref="E:System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged" /> event of the specified source.</summary>
	/// <param name="source">The object with the event.</param>
	/// <param name="listener">The object to add as a listener.</param>
	public static void AddListener(INotifyCollectionChanged source, IWeakEventListener listener)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Removes the specified listener from the <see cref="E:System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged" /> event of the specified source.</summary>
	/// <param name="source">The object with the event.</param>
	/// <param name="listener">The listener to remove.</param>
	public static void RemoveListener(INotifyCollectionChanged source, IWeakEventListener listener)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Adds the specified event handler, which is called when specified source raises the <see cref="E:System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged" /> event.</summary>
	/// <param name="source">The source object that the raises the <see cref="E:System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged" /> event.</param>
	/// <param name="handler">The delegate that handles the <see cref="E:System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged" /> event.</param>
	public static void AddHandler(INotifyCollectionChanged source, EventHandler<NotifyCollectionChangedEventArgs> handler)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Removes the specified event handler from the specified source.</summary>
	/// <param name="source">The source object that the raises the <see cref="E:System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged" /> event.</param>
	/// <param name="handler">The delegate that handles the <see cref="E:System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged" /> event.</param>
	public static void RemoveHandler(INotifyCollectionChanged source, EventHandler<NotifyCollectionChangedEventArgs> handler)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a new object to contain listeners to the <see cref="E:System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged" /> event.</summary>
	/// <returns>A new object to contain listeners to the <see cref="E:System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged" /> event.</returns>
	protected override ListenerList NewListenerList()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Begins listening for the <see cref="E:System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged" /> event on the specified source.</summary>
	/// <param name="source">The object with the event.</param>
	protected override void StartListening(object source)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Stops listening for the <see cref="E:System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged" /> event on the specified source.</summary>
	/// <param name="source">The object with the event.</param>
	protected override void StopListening(object source)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
