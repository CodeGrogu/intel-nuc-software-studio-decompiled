using MS.Internal.WindowsBase;

namespace System.Windows;

/// <summary>Defines an object that has a modifiable state and a read-only (frozen) state. Classes that derive from <see cref="T:System.Windows.Freezable" /> provide detailed change notification, can be made immutable, and can clone themselves. </summary>
public abstract class Freezable : DependencyObject, ISealable
{
	/// <summary>Gets a value that indicates whether the object can be made unmodifiable. </summary>
	/// <returns>
	///     <see langword="true" /> if the current object can be made unmodifiable or is already unmodifiable; otherwise, <see langword="false" />.</returns>
	public bool CanFreeze
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a value that indicates whether the object is currently modifiable. </summary>
	/// <returns>
	///     <see langword="true" /> if the object is frozen and cannot be modified; <see langword="false" /> if the object can be modified.</returns>
	public bool IsFrozen
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsFrozenInternal
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	bool ISealable.CanSeal
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	bool ISealable.IsSealed
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal override DependencyObject InheritanceContext
	{
		[FriendAccessAllowed]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal override bool HasMultipleInheritanceContexts
	{
		[FriendAccessAllowed]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Occurs when the <see cref="T:System.Windows.Freezable" /> or an object it contains is modified. </summary>
	public event EventHandler Changed
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

	internal event EventHandler ChangedInternal
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

	/// <summary>Initializes a new instance of a <see cref="T:System.Windows.Freezable" /> derived class. </summary>
	protected Freezable()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a modifiable clone of the <see cref="T:System.Windows.Freezable" />, making deep copies of the object's values. When copying the object's dependency properties, this method copies expressions (which might no longer resolve) but not animations or their current values. </summary>
	/// <returns>A modifiable clone of the current object. The cloned object's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="false" /> even if the source's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true." /></returns>
	public Freezable Clone()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a modifiable clone (deep copy) of the <see cref="T:System.Windows.Freezable" /> using its current values.</summary>
	/// <returns>A modifiable clone of the current object. The cloned object's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="false" /> even if the source's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true." /></returns>
	public Freezable CloneCurrentValue()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a frozen copy of the <see cref="T:System.Windows.Freezable" />, using base (non-animated) property values. Because the copy is frozen, any frozen sub-objects are copied by reference. </summary>
	/// <returns>A frozen copy of the <see cref="T:System.Windows.Freezable" />. The copy's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is set to <see langword="true" />. </returns>
	/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Freezable" /> cannot be frozen because it contains expressions or animated properties.</exception>
	public Freezable GetAsFrozen()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a frozen copy of the <see cref="T:System.Windows.Freezable" /> using current property values. Because the copy is frozen, any frozen sub-objects are copied by reference.</summary>
	/// <returns>A frozen copy of the <see cref="T:System.Windows.Freezable" />. The copy's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is set to <see langword="true" />.</returns>
	public Freezable GetCurrentValueAsFrozen()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Makes the current object unmodifiable and sets its <see cref="P:System.Windows.Freezable.IsFrozen" /> property to <see langword="true" />. </summary>
	/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Freezable" /> cannot be made unmodifiable. </exception>
	public void Freeze()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Overrides the <see cref="T:System.Windows.DependencyObject" /> implementation of <see cref="M:System.Windows.DependencyObject.OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs)" /> to also invoke any <see cref="E:System.Windows.Freezable.Changed" /> handlers in response to a changing dependency property of type <see cref="T:System.Windows.Freezable" />.</summary>
	/// <param name="e">Event data that contains information about which property changed, and its old and new values.</param>
	protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Freezable" /> class. </summary>
	/// <returns>The new instance.</returns>
	protected Freezable CreateInstance()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>When implemented in a derived class, creates a new instance of the <see cref="T:System.Windows.Freezable" /> derived class. </summary>
	/// <returns>The new instance.</returns>
	protected abstract Freezable CreateInstanceCore();

	/// <summary>Makes the instance a clone (deep copy) of the specified <see cref="T:System.Windows.Freezable" /> using base (non-animated) property values. </summary>
	/// <param name="sourceFreezable">The object to clone.</param>
	protected virtual void CloneCore(Freezable sourceFreezable)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Makes the instance a modifiable clone (deep copy) of the specified <see cref="T:System.Windows.Freezable" /> using current property values.</summary>
	/// <param name="sourceFreezable">The <see cref="T:System.Windows.Freezable" /> to be cloned.</param>
	protected virtual void CloneCurrentValueCore(Freezable sourceFreezable)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Makes the instance a frozen clone of the specified <see cref="T:System.Windows.Freezable" /> using base (non-animated) property values.</summary>
	/// <param name="sourceFreezable">The instance to copy.</param>
	protected virtual void GetAsFrozenCore(Freezable sourceFreezable)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Makes the current instance a frozen clone of the specified <see cref="T:System.Windows.Freezable" />. If the object has animated dependency properties, their current animated values are copied.</summary>
	/// <param name="sourceFreezable">The <see cref="T:System.Windows.Freezable" /> to copy and freeze.</param>
	protected virtual void GetCurrentValueAsFrozenCore(Freezable sourceFreezable)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Makes the <see cref="T:System.Windows.Freezable" /> object unmodifiable or tests whether it can be made unmodifiable.</summary>
	/// <param name="isChecking">
	///       <see langword="true" /> to return an indication of whether the object can be frozen (without actually freezing it); <see langword="false" /> to actually freeze the object.</param>
	/// <returns>If <paramref name="isChecking" /> is <see langword="true" />, this method returns <see langword="true" /> if the <see cref="T:System.Windows.Freezable" /> can be made unmodifiable, or <see langword="false" /> if it cannot be made unmodifiable. If <paramref name="isChecking" /> is <see langword="false" />, this method returns <see langword="true" /> if the if the specified <see cref="T:System.Windows.Freezable" /> is now unmodifiable, or <see langword="false" /> if it cannot be made unmodifiable. </returns>
	protected virtual bool FreezeCore(bool isChecking)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Called when the current <see cref="T:System.Windows.Freezable" /> object is modified. </summary>
	protected virtual void OnChanged()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Ensures that the <see cref="T:System.Windows.Freezable" /> is being accessed from a valid thread. Inheritors of <see cref="T:System.Windows.Freezable" /> must call this method at the beginning of any API that reads data members that are not dependency properties.</summary>
	protected void ReadPreamble()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Verifies that the <see cref="T:System.Windows.Freezable" /> is not frozen and that it is being accessed from a valid threading context. <see cref="T:System.Windows.Freezable" /> inheritors should call this method at the beginning of any API that writes to data members that are not dependency properties. </summary>
	/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Freezable" /> instance is frozen and cannot have its members written to.</exception>
	protected void WritePreamble()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Raises the <see cref="E:System.Windows.Freezable.Changed" /> event for the <see cref="T:System.Windows.Freezable" /> and invokes its <see cref="M:System.Windows.Freezable.OnChanged" /> method. Classes that derive from <see cref="T:System.Windows.Freezable" /> should call this method at the end of any API that modifies class members that are not stored as dependency properties.</summary>
	protected void WritePostscript()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Ensures that appropriate context pointers are established for a <see cref="T:System.Windows.DependencyObjectType" /> data member that has just been set.</summary>
	/// <param name="oldValue">The previous value of the data member.</param>
	/// <param name="newValue">The current value of the data member.</param>
	protected void OnFreezablePropertyChanged(DependencyObject oldValue, DependencyObject newValue)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
	/// <param name="oldValue">The previous value of the data member.</param>
	/// <param name="newValue">The current value of the data member.</param>
	/// <param name="property">The property that changed. </param>
	protected void OnFreezablePropertyChanged(DependencyObject oldValue, DependencyObject newValue, DependencyProperty property)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>If the <paramref name="isChecking" /> parameter is <see langword="true" />, this method indicates whether the specified <see cref="T:System.Windows.Freezable" /> can be made unmodifiable. If the <paramref name="isChecking" /> parameter is <see langword="false" />, this method attempts to make the specified <see cref="T:System.Windows.Freezable" /> unmodifiable and indicates whether the operation succeeded.</summary>
	/// <param name="freezable">The object to check or make unmodifiable. If <paramref name="isChecking" /> is <see langword="true" />, the object is checked to determine whether it can be made unmodifiable. If <paramref name="isChecking" /> is <see langword="false" />, the object is made unmodifiable, if possible.</param>
	/// <param name="isChecking">
	///       <see langword="true" /> to return an indication of whether the object can be frozen (without actually freezing it); <see langword="false" /> to actually freeze the object.</param>
	/// <returns>If <paramref name="isChecking" /> is <see langword="true" />, this method returns <see langword="true" /> if the specified <see cref="T:System.Windows.Freezable" /> can be made unmodifiable, or <see langword="false" /> if it cannot be made unmodifiable. If <paramref name="isChecking" /> is <see langword="false" />, this method returns <see langword="true" /> if the specified <see cref="T:System.Windows.Freezable" /> is now unmodifiable, or <see langword="false" /> if it cannot be made unmodifiable. </returns>
	/// <exception cref="T:System.InvalidOperationException">When <paramref name="isChecking" /> is <see langword="false" />, the attempt to make <paramref name="freezable" /> unmodifiable was unsuccessful; the object is now in an unknown state (it might be partially frozen).  </exception>
	protected internal static bool Freeze(Freezable freezable, bool isChecking)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	void ISealable.Seal()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void ClearContextAndHandlers()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void FireChanged()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal override void Seal()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool Freeze(bool isChecking)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal override void AddInheritanceContext(DependencyObject context, DependencyProperty property)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal override void RemoveInheritanceContext(DependencyObject context, DependencyProperty property)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void AddContextInformation(DependencyObject context, DependencyProperty property)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static Freezable()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
