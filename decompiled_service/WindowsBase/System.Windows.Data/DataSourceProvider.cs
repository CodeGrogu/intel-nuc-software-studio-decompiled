using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace System.Windows.Data;

/// <summary>Common base class and contract for <see cref="T:System.Windows.Data.DataSourceProvider" /> objects, which are factories that execute some queries to produce a single object or a list of objects that you can use as binding source objects.</summary>
public abstract class DataSourceProvider : INotifyPropertyChanged, ISupportInitialize
{
	/// <summary>Gets or sets a value that indicates whether to prevent or delay the automatic loading of data.</summary>
	/// <returns>
	///     <see langword="false" /> to prevent or delay the automatic loading of data; otherwise, <see langword="true" />. The default value is <see langword="true" />.</returns>
	[DefaultValue(true)]
	public bool IsInitialLoadEnabled
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

	/// <summary>Gets the underlying data object.</summary>
	/// <returns>A value of type <see cref="T:System.Object" /> that is the underlying data object.</returns>
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object Data
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the error of the last query operation.</summary>
	/// <returns>A value of type <see cref="T:System.Exception" /> that is the error of the last query operation, or <see langword="null" /> if there was no error.</returns>
	public Exception Error
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a value that indicates whether there is an outstanding <see cref="M:System.Windows.Data.DataSourceProvider.DeferRefresh" /> in use.</summary>
	/// <returns>
	///     <see langword="true" /> if there is an outstanding <see cref="M:System.Windows.Data.DataSourceProvider.DeferRefresh" /> in use; otherwise, <see langword="false" />.</returns>
	protected bool IsRefreshDeferred
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets the current <see cref="T:System.Windows.Threading.Dispatcher" /> object to the UI thread to use.</summary>
	/// <returns>The current <see cref="T:System.Windows.Threading.Dispatcher" /> object to the UI thread to use. By default, this is the <see cref="T:System.Windows.Threading.Dispatcher" /> object that is associated with the thread on which this instance was created.</returns>
	protected Dispatcher Dispatcher
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

	/// <summary>Occurs when the <see cref="P:System.Windows.Data.DataSourceProvider.Data" /> property has a new value.</summary>
	public event EventHandler DataChanged
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

	/// <summary>Occurs when a property value changes.</summary>
	event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
	{
		add
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		remove
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Occurs when a property value changes.</summary>
	protected virtual event PropertyChangedEventHandler PropertyChanged
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.DataSourceProvider" /> class. This is a protected constructor.</summary>
	protected DataSourceProvider()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Starts the initial query to the underlying data model. The result is returned on the <see cref="P:System.Windows.Data.DataSourceProvider.Data" /> property.</summary>
	public void InitialLoad()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initiates a refresh operation to the underlying data model. The result is returned on the <see cref="P:System.Windows.Data.DataSourceProvider.Data" /> property.</summary>
	public void Refresh()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Enters a defer cycle that you can use to change properties of the provider and delay automatic refresh.</summary>
	/// <returns>An <see cref="T:System.IDisposable" /> object that you can use to dispose of the calling object.</returns>
	public virtual IDisposable DeferRefresh()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
	void ISupportInitialize.BeginInit()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
	void ISupportInitialize.EndInit()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>When overridden in a derived class, this base class calls this method when <see cref="M:System.Windows.Data.DataSourceProvider.InitialLoad" /> or <see cref="M:System.Windows.Data.DataSourceProvider.Refresh" /> has been called. The base class delays the call if refresh is deferred or initial load is disabled.</summary>
	protected virtual void BeginQuery()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Derived classes call this method to indicate that a query has finished.</summary>
	/// <param name="newData">The data that is the result of the query.</param>
	protected void OnQueryFinished(object newData)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Derived classes call this method to indicate that a query has finished.</summary>
	/// <param name="newData">The data that is the result of the query.</param>
	/// <param name="error">The error that occurred while running the query. This value is <see langword="null" /> if there is no error.</param>
	/// <param name="completionWork">Optional delegate that is used to execute completion work on the UI thread, for example, to set additional properties.</param>
	/// <param name="callbackArguments">Optional arguments to send as a parameter with the <paramref name="completionWork" /> delegate.</param>
	protected virtual void OnQueryFinished(object newData, Exception error, DispatcherOperationCallback completionWork, object callbackArguments)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Raises the <see cref="E:System.Windows.Data.DataSourceProvider.PropertyChanged" /> event with the provided arguments.</summary>
	/// <param name="e">Arguments of the event being raised.</param>
	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Indicates that initialization of this object is about to begin; no implicit <see cref="M:System.Windows.Data.DataSourceProvider.Refresh" /> occurs until the matched <see cref="M:System.Windows.Data.DataSourceProvider.EndInit" /> method is called.</summary>
	protected virtual void BeginInit()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Indicates that the initialization of this object has completed; this causes a <see cref="M:System.Windows.Data.DataSourceProvider.Refresh" /> if no other <see cref="M:System.Windows.Data.DataSourceProvider.DeferRefresh" /> is outstanding.</summary>
	protected virtual void EndInit()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static DataSourceProvider()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
