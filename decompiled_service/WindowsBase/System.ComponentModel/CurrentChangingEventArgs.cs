namespace System.ComponentModel;

/// <summary>Provides information for the <see cref="E:System.ComponentModel.ICollectionView.CurrentChanging" /> event.</summary>
public class CurrentChangingEventArgs : EventArgs
{
	/// <summary>Gets a value that indicates whether the event is cancelable.</summary>
	/// <returns>
	///     <see langword="true" /> if the event is cancelable, otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
	public bool IsCancelable
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets a value that indicates whether to cancel the event.</summary>
	/// <returns>
	///     <see langword="true" /> if the event is to be canceled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
	/// <exception cref="T:System.InvalidOperationException">If the value of <see cref="P:System.ComponentModel.CurrentChangingEventArgs.IsCancelable" /> is <see langword="false" />.</exception>
	public bool Cancel
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

	/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.CurrentChangingEventArgs" /> class.</summary>
	public CurrentChangingEventArgs()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.CurrentChangingEventArgs" /> class with the specified <paramref name="isCancelable" /> value.</summary>
	/// <param name="isCancelable">A value that indicates whether the event is cancelable.</param>
	public CurrentChangingEventArgs(bool isCancelable)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
