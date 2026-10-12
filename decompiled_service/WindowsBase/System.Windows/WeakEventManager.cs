using System.Reflection;
using System.Windows.Threading;
using MS.Internal.WindowsBase;

namespace System.Windows;

/// <summary>Provides a base class for the event manager that is used in the weak event pattern. The manager adds and removes listeners for events (or callbacks) that also use the pattern.</summary>
public abstract class WeakEventManager : DispatcherObject
{
	internal struct Listener
	{
		private WeakReference _target;

		private WeakReference _handler;

		public object Target
		{
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		public Delegate Handler
		{
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		public bool HasHandler
		{
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		public Listener(object target)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		public Listener(object target, Delegate handler)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		public bool Matches(object target, Delegate handler)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Provides a built-in collection list for storing listeners for a <see cref="T:System.Windows.WeakEventManager" />.</summary>
	protected class ListenerList
	{
		/// <summary>Gets or sets a specific listener item in the <see cref="T:System.Windows.WeakEventManager.ListenerList" /> .</summary>
		/// <param name="index">The zero-based index of the listener in the list.</param>
		/// <returns>The item at that index, or a null reference if no item was at that index.</returns>
		public IWeakEventListener this[int index]
		{
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		/// <summary>Gets the number of items contained in the <see cref="T:System.Windows.WeakEventManager.ListenerList" />.</summary>
		/// <returns>The number of items contained in the <see cref="T:System.Windows.WeakEventManager.ListenerList" />.</returns>
		public int Count
		{
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		/// <summary>Gets a value that declares whether this <see cref="T:System.Windows.WeakEventManager.ListenerList" />  is empty.</summary>
		/// <returns>
		///     <see langword="true" /> if the list is empty; otherwise, <see langword="false" />.</returns>
		public bool IsEmpty
		{
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		/// <summary>Gets a value that represents an empty list for purposes of comparisons.</summary>
		/// <returns>The empty list representation.</returns>
		public static ListenerList Empty
		{
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.WeakEventManager.ListenerList" /> class.</summary>
		public ListenerList()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.WeakEventManager.ListenerList" /> class with the specified initial capacity.</summary>
		/// <param name="capacity">The number of items that should be allocated in the initial list.</param>
		public ListenerList(int capacity)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		internal Listener GetListener(int index)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		/// <summary>Adds a <see cref="T:System.Windows.IWeakEventListener" /> object to the <see cref="T:System.Windows.WeakEventManager.ListenerList" />.</summary>
		/// <param name="listener">The listener element to add to the <see cref="T:System.Windows.WeakEventManager.ListenerList" />.</param>
		public void Add(IWeakEventListener listener)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		/// <summary>Removes the first occurrence of a listener item from the <see cref="T:System.Windows.WeakEventManager.ListenerList" />. </summary>
		/// <param name="listener">The item to remove.</param>
		public void Remove(IWeakEventListener listener)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		/// <summary>Adds an event handler to the <see cref="T:System.Windows.WeakEventManager.ListenerList" />.</summary>
		/// <param name="handler">The event handler to add to the <see cref="T:System.Windows.WeakEventManager.ListenerList" />.</param>
		public void AddHandler(Delegate handler)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		/// <summary>Removes an event handler from the <see cref="T:System.Windows.WeakEventManager.ListenerList" />.</summary>
		/// <param name="handler">The event handler to remove from the <see cref="T:System.Windows.WeakEventManager.ListenerList" />.</param>
		public void RemoveHandler(Delegate handler)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		internal void Add(Listener listener)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		/// <summary>Checks to see whether the provided list is in use, and if so, sets the list reference parameter to a copy of that list rather than the original.</summary>
		/// <param name="list">The list to check for use state and potentially copy.</param>
		/// <returns>
		///     <see langword="true" /> if the provided list was in use at the time of call and therefore the <paramref name="list" /> parameter reference was reset to be a copy. <see langword="false" /> if the provided list was not in use, in which case the <paramref name="list" /> parameter reference remains unaltered.</returns>
		public static bool PrepareForWriting(ref ListenerList list)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		/// <summary>Delivers the event being managed to each listener in the <see cref="T:System.Windows.WeakEventManager.ListenerList" />.</summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="args">An object that contains the event data.</param>
		/// <param name="managerType">The type of the <see cref="T:System.Windows.WeakEventManager" /> that calls this method.</param>
		/// <returns>
		///     <see langword="true" /> if any of the listeners in the <see cref="T:System.Windows.WeakEventManager.ListenerList" /> refer to an object that has been garbage collected; otherwise, <see langword="false" />.</returns>
		public virtual bool DeliverEvent(object sender, EventArgs args, Type managerType)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		internal bool DeliverEvent(ref Listener listener, object sender, EventArgs args, Type managerType)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		/// <summary>Removes all entries from the list where the underlying reference target is a null reference.</summary>
		/// <returns>Returns <see langword="true" /> if any entries were purged; otherwise, <see langword="false" />.</returns>
		public bool Purge()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		/// <summary>Creates a modifiable clone of this <see cref="T:System.Windows.WeakEventManager.ListenerList" />. </summary>
		/// <returns>A modifiable clone of the current object. </returns>
		public virtual ListenerList Clone()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		/// <summary>Copies the current <see cref="T:System.Windows.WeakEventManager.ListenerList" /> to the specified <see cref="T:System.Windows.WeakEventManager.ListenerList" /></summary>
		/// <param name="newList">The object to copy to.</param>
		protected void CopyTo(ListenerList newList)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		/// <summary>Declares the list to be in use. This prevents direct changes to the list during iterations of the list items.</summary>
		/// <returns>
		///     <see langword="true" /> if the list was already declared to be in use; otherwise, <see langword="false" />.</returns>
		public bool BeginUse()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		/// <summary>Unlocks the locked state initiated by <see cref="M:System.Windows.WeakEventManager.ListenerList.BeginUse" />.</summary>
		public void EndUse()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		static ListenerList()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Provides a type-safe collection list for storing listeners for a <see cref="T:System.Windows.WeakEventManager" />. This class defines a type parameter for the event data that is used.</summary>
	/// <typeparam name="TEventArgs">The type that holds the event data.</typeparam>
	protected class ListenerList<TEventArgs> : ListenerList where TEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.WeakEventManager.ListenerList`1" /> class.</summary>
		public ListenerList()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.WeakEventManager.ListenerList`1" /> class with the specified initial capacity.</summary>
		/// <param name="capacity">The number of items that should be allocated in the initial list.</param>
		public ListenerList(int capacity)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		/// <summary>Delivers the event being managed to each listener in the <see cref="T:System.Windows.WeakEventManager.ListenerList`1" />.</summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">An object that contains the event data.</param>
		/// <param name="managerType">The type of the <see cref="T:System.Windows.WeakEventManager" /> that calls this method.</param>
		/// <returns>
		///     <see langword="true" /> if any of the listeners in the <see cref="T:System.Windows.WeakEventManager.ListenerList`1" /> refer to an object that has been garbage collected; otherwise, <see langword="false" />.</returns>
		public override bool DeliverEvent(object sender, EventArgs e, Type managerType)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		/// <summary>Creates a modifiable clone of this <see cref="T:System.Windows.WeakEventManager.ListenerList" />, making deep copies of the values.</summary>
		/// <returns>A modifiable clone of the current object. The cloned object's <see cref="P:System.Windows.Freezable.IsFrozen" /> property will be <see langword="false" /> even if the source's <see cref="P:System.Windows.Freezable.IsFrozen" /> property was <see langword="true" />.</returns>
		public override ListenerList Clone()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Establishes a read-lock on the underlying data table, and returns an <see cref="T:System.IDisposable" />.</summary>
	/// <returns>An object that can be used to establish a lock on the data table members and then be appropriately disposed with a <see langword="using" /> construct.</returns>
	protected IDisposable ReadLock
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Establishes a write-lock on the underlying data table, and returns an<see cref="T:System.IDisposable" />.</summary>
	/// <returns>An object that can be used to establish a lock on the data table members and then be appropriately disposed with a <see langword="using" /> construct.</returns>
	protected IDisposable WriteLock
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets the data being stored for the specified source.</summary>
	/// <param name="source">The zero-based index of the requested source.</param>
	/// <returns>Data being stored by the manager for this source.</returns>
	protected object this[object source]
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

	internal static MethodInfo DeliverEventMethodInfo
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Initializes base class values when it is used as the initializer by the constructor of a derived class.</summary>
	protected WeakEventManager()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static WeakEventManager()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a new object to contain listeners to an event.</summary>
	/// <returns>A new object to contain listeners to an event.</returns>
	protected virtual ListenerList NewListenerList()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>When overridden in a derived class, starts listening for the event being managed. After the <see cref="M:System.Windows.WeakEventManager.StartListening(System.Object)" /> method is first called, the manager should be in the state of calling <see cref="M:System.Windows.WeakEventManager.DeliverEvent(System.Object,System.EventArgs)" /> or <see cref="M:System.Windows.WeakEventManager.DeliverEventToList(System.Object,System.EventArgs,System.Windows.WeakEventManager.ListenerList)" /> whenever the relevant event from the provided source is handled.</summary>
	/// <param name="source">The source to begin listening on.</param>
	protected abstract void StartListening(object source);

	/// <summary>When overridden in a derived class, stops listening on the provided source for the event being managed.</summary>
	/// <param name="source">The source to stop listening on.</param>
	protected abstract void StopListening(object source);

	/// <summary>Returns the <see cref="T:System.Windows.WeakEventManager" /> implementation that is used for the provided type.</summary>
	/// <param name="managerType">The type to obtain the <see cref="T:System.Windows.WeakEventManager" /> for.</param>
	/// <returns>The matching <see cref="T:System.Windows.WeakEventManager" /> implementation.</returns>
	protected static WeakEventManager GetCurrentManager(Type managerType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Sets the current manager for the specified manager type.</summary>
	/// <param name="managerType">The type to set the new event manager.</param>
	/// <param name="manager">The new event manager.</param>
	protected static void SetCurrentManager(Type managerType, WeakEventManager manager)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static WeakEventManager GetCurrentManager(Type eventSourceType, string eventName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void SetCurrentManager(Type eventSourceType, string eventName, WeakEventManager manager)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Removes all listeners for the specified source.</summary>
	/// <param name="source">The source to remove listener information for.</param>
	protected void Remove(object source)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Adds the provided listener to the provided source for the event being managed.</summary>
	/// <param name="source">The source to attach listeners to.</param>
	/// <param name="listener">The listening class (which must implement <see cref="T:System.Windows.IWeakEventListener" />).</param>
	protected void ProtectedAddListener(object source, IWeakEventListener listener)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Removes a previously added listener from the provided source.</summary>
	/// <param name="source">The source to remove listeners from.</param>
	/// <param name="listener">The listening class (which must implement <see cref="T:System.Windows.IWeakEventListener" />).</param>
	protected void ProtectedRemoveListener(object source, IWeakEventListener listener)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Adds the specified delegate as an event handler of the specified source.</summary>
	/// <param name="source">The source object that the handler delegate subscribes to.</param>
	/// <param name="handler">The delegate that handles the event that is raised by <paramref name="source" />.</param>
	protected void ProtectedAddHandler(object source, Delegate handler)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Removes the previously added handler from the specified source.</summary>
	/// <param name="source">The source to remove the handler from. </param>
	/// <param name="handler">The delegate to remove from <paramref name="source" />.</param>
	protected void ProtectedRemoveHandler(object source, Delegate handler)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Delivers the event being managed to each listener.</summary>
	/// <param name="sender">The object on which the event is being handled.</param>
	/// <param name="args">An <see cref="T:System.EventArgs" /> that contains the event data for the event to deliver.</param>
	protected void DeliverEvent(object sender, EventArgs args)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Delivers the event being managed to each listener in the provided list.</summary>
	/// <param name="sender">The object on which the event is being handled.</param>
	/// <param name="args">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
	/// <param name="list">The provided <see cref="T:System.Windows.WeakEventManager.ListenerList" />.</param>
	protected void DeliverEventToList(object sender, EventArgs args, ListenerList list)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Requests that a purge of unused entries in the underlying listener list be performed on a lower priority thread.</summary>
	protected void ScheduleCleanup()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Removes inactive listener entries from the data list for the provided source. Returns <see langword="true" /> if some entries were actually removed from the list.</summary>
	/// <param name="source">The source for events being listened to.</param>
	/// <param name="data">The data to check. This object is expected to be a <see cref="T:System.Windows.WeakEventManager.ListenerList" /> implementation.</param>
	/// <param name="purgeAll">
	///       <see langword="true" /> to stop listening to <paramref name="source" />, and completely remove all entries from <paramref name="data" />.</param>
	/// <returns>
	///     <see langword="true" /> if some entries were actually removed; otherwise, <see langword="false" />.</returns>
	protected virtual bool Purge(object source, object data, bool purgeAll)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool PurgeInternal(object source, object data, bool purgeAll)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal static bool Cleanup()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal static void SetCleanupEnabled(bool value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
/// <summary>Provides a type-safe <see cref="T:System.Windows.WeakEventManager" /> that enables you to specify the event handler to use for the "weak event listener" pattern. This class defines a type parameter for the source of the event and a type parameter for the event data that is used.</summary>
/// <typeparam name="TEventSource">The type that raises the event.</typeparam>
/// <typeparam name="TEventArgs">The type that holds the event data.</typeparam>
public class WeakEventManager<TEventSource, TEventArgs> : WeakEventManager where TEventArgs : EventArgs
{
	/// <summary>Adds the specified event handler to the specified event.</summary>
	/// <param name="source">The source object that raises the specified event.</param>
	/// <param name="eventName">The name of the event to subscribe to.</param>
	/// <param name="handler">The delegate that handles the event.</param>
	public static void AddHandler(TEventSource source, string eventName, EventHandler<TEventArgs> handler)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Removes the specified event handler from the specified event.</summary>
	/// <param name="source">The source object that raises the specified event.</param>
	/// <param name="eventName">The name of the event to remove the handler from.</param>
	/// <param name="handler">The delegate to remove</param>
	public static void RemoveHandler(TEventSource source, string eventName, EventHandler<TEventArgs> handler)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a new object to contain listeners to an event.</summary>
	/// <returns>A new object to contain listeners to an event.</returns>
	protected override ListenerList NewListenerList()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Starts listening for the event on the specified object.</summary>
	/// <param name="source">The object to that raises the event.</param>
	protected override void StartListening(object source)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Stops listening for the event on the specified object.</summary>
	/// <param name="source">The object to that raises the event.</param>
	protected override void StopListening(object source)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	private WeakEventManager()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
