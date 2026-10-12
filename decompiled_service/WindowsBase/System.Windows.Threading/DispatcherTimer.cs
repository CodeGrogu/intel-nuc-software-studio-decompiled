using System.Runtime.CompilerServices;

namespace System.Windows.Threading;

/// <summary>A timer that is integrated into the <see cref="T:System.Windows.Threading.Dispatcher" /> queue which is processed at a specified interval of time and at a specified priority. </summary>
public class DispatcherTimer
{
	internal int _dueTimeInTicks;

	/// <summary>Gets the <see cref="T:System.Windows.Threading.Dispatcher" /> associated with this <see cref="T:System.Windows.Threading.DispatcherTimer" />. </summary>
	/// <returns>The dispatcher.</returns>
	public Dispatcher Dispatcher
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets a value that indicates whether the timer is running. </summary>
	/// <returns>
	///     <see langword="true" /> if the timer is enabled; otherwise, <see langword="false" />.  The default is <see langword="false" />.</returns>
	public bool IsEnabled
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

	/// <summary>Gets or sets the period of time between timer ticks. </summary>
	/// <returns>The period of time between ticks. The default is 00:00:00.</returns>
	/// <exception cref="T:System.ArgumentOutOfRangeException">
	///         <paramref name="interval" /> is less than 0 or greater than <see cref="F:System.Int32.MaxValue" /> milliseconds.</exception>
	public TimeSpan Interval
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

	/// <summary>Gets or sets a user-defined data object. </summary>
	/// <returns>The user-defined data.  The default is <see langword="null" />.</returns>
	public object Tag
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

	/// <summary>Occurs when the timer interval has elapsed. </summary>
	public event EventHandler Tick
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Threading.DispatcherTimer" /> class.</summary>
	public DispatcherTimer()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Threading.DispatcherTimer" /> class which processes timer events at the specified priority.</summary>
	/// <param name="priority">The priority at which to invoke the timer.</param>
	public DispatcherTimer(DispatcherPriority priority)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Threading.DispatcherTimer" /> class which runs on the specified <see cref="T:System.Windows.Threading.Dispatcher" /> at the specified priority.</summary>
	/// <param name="priority">The priority at which to invoke the timer.</param>
	/// <param name="dispatcher">The dispatcher the timer is associated with.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="dispatcher" /> is <see langword="null" />.</exception>
	public DispatcherTimer(DispatcherPriority priority, Dispatcher dispatcher)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Threading.DispatcherTimer" /> class which uses the specified time interval, priority, event handler, and <see cref="T:System.Windows.Threading.Dispatcher" />.</summary>
	/// <param name="interval">The period of time between ticks.</param>
	/// <param name="priority">The priority at which to invoke the timer.</param>
	/// <param name="callback">The event handler to call when the <see cref="E:System.Windows.Threading.DispatcherTimer.Tick" /> event occurs.</param>
	/// <param name="dispatcher">The dispatcher the timer is associated with.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="dispatcher" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="callback" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentOutOfRangeException">
	///         <paramref name="interval" /> is less than 0 or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
	public DispatcherTimer(TimeSpan interval, DispatcherPriority priority, EventHandler callback, Dispatcher dispatcher)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Starts the <see cref="T:System.Windows.Threading.DispatcherTimer" />. </summary>
	public void Start()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Stops the <see cref="T:System.Windows.Threading.DispatcherTimer" />. </summary>
	public void Stop()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Promote()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
