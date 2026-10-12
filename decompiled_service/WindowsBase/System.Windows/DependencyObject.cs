using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Markup;
using System.Windows.Threading;
using MS.Internal.ComponentModel;
using MS.Internal.WindowsBase;

namespace System.Windows;

/// <summary>Represents an object that participates in the dependency property system.</summary>
[TypeDescriptionProvider(typeof(DependencyObjectProvider))]
[NameScopeProperty("NameScope", typeof(NameScope))]
public class DependencyObject : DispatcherObject
{
	[FriendAccessAllowed]
	internal static readonly DependencyProperty DirectDependencyProperty;

	internal object _contextStorage;

	[FriendAccessAllowed]
	internal static readonly object ExpressionInAlternativeStore;

	internal static readonly UncommonField<object> DependentListMapField;

	internal static DependencyObjectType DType;

	/// <summary>Gets the <see cref="T:System.Windows.DependencyObjectType" /> that wraps the CLR type of this instance. </summary>
	/// <returns>A <see cref="T:System.Windows.DependencyObjectType" /> that wraps the CLR type of this instance. </returns>
	public DependencyObjectType DependencyObjectType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a value that indicates whether this instance is currently sealed (read-only).</summary>
	/// <returns>
	///     <see langword="true" /> if this instance is sealed; otherwise, <see langword="false" />.</returns>
	public bool IsSealed
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[FriendAccessAllowed]
	internal bool IsInheritanceContextSealed
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

	internal bool Freezable_Frozen
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

	internal bool Freezable_HasMultipleInheritanceContexts
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

	internal bool Freezable_UsingHandlerList
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

	internal bool Freezable_UsingContextList
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

	internal bool Freezable_UsingSingletonHandler
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

	internal bool Freezable_UsingSingletonContext
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

	internal bool Animatable_IsResourceInvalidationNecessary
	{
		[FriendAccessAllowed]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[FriendAccessAllowed]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IAnimatable_HasAnimatedProperties
	{
		[FriendAccessAllowed]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[FriendAccessAllowed]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal virtual DependencyObject InheritanceContext
	{
		[FriendAccessAllowed]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal virtual bool HasMultipleInheritanceContexts
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool CanBeInheritanceContext
	{
		[FriendAccessAllowed]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[FriendAccessAllowed]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal EffectiveValueEntry[] EffectiveValues
	{
		[FriendAccessAllowed]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal uint EffectiveValuesCount
	{
		[FriendAccessAllowed]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		private set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal uint InheritableEffectiveValuesCount
	{
		[FriendAccessAllowed]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal DependencyObject InheritanceParent
	{
		[FriendAccessAllowed]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsSelfInheritanceParent
	{
		[FriendAccessAllowed]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[FriendAccessAllowed]
	internal virtual int EffectiveValuesInitialSize
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal event EventHandler InheritanceContextChanged
	{
		[FriendAccessAllowed]
		add
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[FriendAccessAllowed]
		remove
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.DependencyObject" /> class. </summary>
	public DependencyObject()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal virtual void Seal()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether a provided <see cref="T:System.Windows.DependencyObject" /> is equivalent to the current <see cref="T:System.Windows.DependencyObject" />.</summary>
	/// <param name="obj">The <see cref="T:System.Windows.DependencyObject" />  to compare to the current instance.</param>
	/// <returns>
	///     <see langword="true" /> if the two instances are the same; otherwise, <see langword="false" />.</returns>
	public sealed override bool Equals(object obj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Gets a hash code for this <see cref="T:System.Windows.DependencyObject" />.</summary>
	/// <returns>A signed 32-bit integer hash code.</returns>
	public sealed override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the current effective value of a dependency property on this instance of a <see cref="T:System.Windows.DependencyObject" />. </summary>
	/// <param name="dp">The <see cref="T:System.Windows.DependencyProperty" /> identifier of the property to retrieve the value for.</param>
	/// <returns>Returns the current effective value.</returns>
	/// <exception cref="T:System.InvalidOperationException">The specified <paramref name="dp" /> or its value was invalid, or the specified <paramref name="dp" /> does not exist.</exception>
	public object GetValue(DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal EffectiveValueEntry GetValueEntry(EntryIndex entryIndex, DependencyProperty dp, PropertyMetadata metadata, RequestFlags requests)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Sets the local value of a dependency property, specified by its dependency property identifier. </summary>
	/// <param name="dp">The identifier of the dependency property to set.</param>
	/// <param name="value">The new local value.</param>
	/// <exception cref="T:System.InvalidOperationException">Attempted to modify a read-only dependency property, or a property on a sealed <see cref="T:System.Windows.DependencyObject" />.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="value" /> was not the correct type as registered for the <paramref name="dp" /> property.</exception>
	public void SetValue(DependencyProperty dp, object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Sets the value of a dependency property without changing its value source. </summary>
	/// <param name="dp">The identifier of the dependency property to set.</param>
	/// <param name="value">The new local value.</param>
	/// <exception cref="T:System.InvalidOperationException">Attempted to modify a read-only dependency property, or a property on a sealed <see cref="T:System.Windows.DependencyObject" />.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="value" /> was not the correct type as registered for the <paramref name="dp" /> property.</exception>
	public void SetCurrentValue(DependencyProperty dp, object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal void SetValue(DependencyProperty dp, bool value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal void SetCurrentValue(DependencyProperty dp, bool value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal void SetValueInternal(DependencyProperty dp, object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal void SetCurrentValueInternal(DependencyProperty dp, object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal void SetDeferredValue(DependencyProperty dp, DeferredReference deferredReference)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal void SetCurrentDeferredValue(DependencyProperty dp, DeferredReference deferredReference)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal void SetMutableDefaultValue(DependencyProperty dp, object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal void SetValue(DependencyPropertyKey dp, bool value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Sets the local value of a read-only dependency property, specified by the <see cref="T:System.Windows.DependencyPropertyKey" /> identifier of the dependency property. </summary>
	/// <param name="key">The <see cref="T:System.Windows.DependencyPropertyKey" /> identifier of the property to set.</param>
	/// <param name="value">The new local value.</param>
	public void SetValue(DependencyPropertyKey key, object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal bool ProvideSelfAsInheritanceContext(object value, DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal bool ProvideSelfAsInheritanceContext(DependencyObject doValue, DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal bool RemoveSelfAsInheritanceContext(object value, DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal bool RemoveSelfAsInheritanceContext(DependencyObject doValue, DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Clears the local value of a property. The property to be cleared is specified by a <see cref="T:System.Windows.DependencyProperty" /> identifier. </summary>
	/// <param name="dp">The dependency property to be cleared, identified by a <see cref="T:System.Windows.DependencyProperty" /> object reference.</param>
	/// <exception cref="T:System.InvalidOperationException">Attempted to call <see cref="M:System.Windows.DependencyObject.ClearValue(System.Windows.DependencyProperty)" /> on a sealed <see cref="T:System.Windows.DependencyObject" />.</exception>
	public void ClearValue(DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Clears the local value of a read-only property. The property to be cleared is specified by a <see cref="T:System.Windows.DependencyPropertyKey" />. </summary>
	/// <param name="key">The key for the dependency property to be cleared.</param>
	/// <exception cref="T:System.InvalidOperationException">Attempted to call <see cref="M:System.Windows.DependencyObject.ClearValue(System.Windows.DependencyProperty)" /> on a sealed <see cref="T:System.Windows.DependencyObject" />.</exception>
	public void ClearValue(DependencyPropertyKey key)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool ContainsValue(DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void ChangeExpressionSources(Expression expr, DependencyObject d, DependencyProperty dp, DependencySource[] newSources)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Coerces the value of the specified dependency property. This is accomplished by invoking any <see cref="T:System.Windows.CoerceValueCallback" /> function specified in property metadata for the dependency property as it exists on the calling <see cref="T:System.Windows.DependencyObject" />.</summary>
	/// <param name="dp">The identifier for the dependency property to coerce.</param>
	/// <exception cref="T:System.InvalidOperationException">The specified <paramref name="dp" /> or its value were invalid or do not exist.</exception>
	public void CoerceValue(DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal void InvalidateSubProperty(DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal void NotifySubPropertyChange(DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Re-evaluates the effective value for the specified dependency property</summary>
	/// <param name="dp">The <see cref="T:System.Windows.DependencyProperty" /> identifier of the property to invalidate.</param>
	public void InvalidateProperty(DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void InvalidateProperty(DependencyProperty dp, bool preserveCurrentValue)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal UpdateResult UpdateEffectiveValue(EntryIndex entryIndex, DependencyProperty dp, PropertyMetadata metadata, EffectiveValueEntry oldEntry, ref EffectiveValueEntry newEntry, bool coerceWithDeferredReference, bool coerceWithCurrentValue, OperationType operationType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal void NotifyPropertyChange(DependencyPropertyChangedEventArgs args)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal virtual void EvaluateBaseValueCore(DependencyProperty dp, PropertyMetadata metadata, ref EffectiveValueEntry newEntry)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal virtual void EvaluateAnimatedValueCore(DependencyProperty dp, PropertyMetadata metadata, ref EffectiveValueEntry newEntry)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.DependencyObject" /> has been updated. The specific dependency property that changed is reported in the event data. </summary>
	/// <param name="e">Event data that will contain the dependency property identifier of interest, the property metadata for the type, and old and new values.</param>
	protected virtual void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a value that indicates whether serialization processes should serialize the value for the provided dependency property.</summary>
	/// <param name="dp">The identifier for the dependency property that should be serialized.</param>
	/// <returns>
	///     <see langword="true" /> if the dependency property that is supplied should be value-serialized; otherwise, <see langword="false" />.</returns>
	protected internal virtual bool ShouldSerializeProperty(DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal BaseValueSourceInternal GetValueSource(DependencyProperty dp, PropertyMetadata metadata, out bool hasModifiers)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal BaseValueSourceInternal GetValueSource(DependencyProperty dp, PropertyMetadata metadata, out bool hasModifiers, out bool isExpression, out bool isAnimated, out bool isCoerced, out bool isCurrent)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the local value of a dependency property, if it exists. </summary>
	/// <param name="dp">The <see cref="T:System.Windows.DependencyProperty" /> identifier of the property to retrieve the value for.</param>
	/// <returns>Returns the local value, or returns the sentinel value <see cref="F:System.Windows.DependencyProperty.UnsetValue" /> if no local value is set.</returns>
	public object ReadLocalValue(DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal object ReadLocalValueEntry(EntryIndex entryIndex, DependencyProperty dp, bool allowDeferredReferences)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a specialized enumerator for determining which dependency properties have locally set values on this <see cref="T:System.Windows.DependencyObject" />. </summary>
	/// <returns>A specialized local value enumerator.</returns>
	public LocalValueEnumerator GetLocalValueEnumerator()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void UpdateSourceDependentLists(DependencyObject d, DependencyProperty dp, DependencySource[] sources, Expression expr, bool add)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void ValidateSources(DependencyObject d, DependencySource[] newSources, Expression expr)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal static void RegisterForAlternativeExpressionStorage(AlternativeExpressionStorageCallback getExpressionCore, out AlternativeExpressionStorageCallback getExpression)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool HasAnyExpression()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal bool HasExpression(EntryIndex entryIndex, DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal virtual void AddInheritanceContext(DependencyObject context, DependencyProperty property)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal virtual void RemoveInheritanceContext(DependencyObject context, DependencyProperty property)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal virtual bool ShouldProvideInheritanceContext(DependencyObject target, DependencyProperty property)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal void OnInheritanceContextChanged(EventArgs args)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal virtual void OnInheritanceContextChangedCore(EventArgs args)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal static bool IsTreeWalkOperation(OperationType operation)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[Conditional("DEBUG")]
	internal void Debug_AssertNoInheritanceContextListeners()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal void BeginPropertyInitialization()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal void EndPropertyInitialization()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal void SetIsSelfInheritanceParent()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal void SynchronizeInheritanceParent(DependencyObject parent)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal EntryIndex LookupEntry(int targetIndex)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void SetEffectiveValue(EntryIndex entryIndex, DependencyProperty dp, PropertyMetadata metadata, EffectiveValueEntry newEntry, EffectiveValueEntry oldEntry)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal void SetEffectiveValue(EntryIndex entryIndex, DependencyProperty dp, int targetIndex, PropertyMetadata metadata, object value, BaseValueSourceInternal valueSource)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void UnsetEffectiveValue(EntryIndex entryIndex, DependencyProperty dp, PropertyMetadata metadata)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static DependencyObject()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
