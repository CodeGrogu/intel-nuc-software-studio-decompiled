namespace System.Windows.Input;

/// <summary>Represents a request to move focus to another control. </summary>
[Serializable]
public class TraversalRequest
{
	/// <summary> Gets or sets a value that indicates whether focus traversal has reached the end of child elements that can have focus. </summary>
	/// <returns>
	///     <see langword="true" /> if this traversal has reached the end of child elements that can have focus; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
	public bool Wrapped
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

	/// <summary>Gets the traversal direction. </summary>
	/// <returns>One of the traversal direction enumeration values.</returns>
	public FocusNavigationDirection FocusNavigationDirection
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Input.TraversalRequest" /> class. </summary>
	/// <param name="focusNavigationDirection">The intended direction of the focus traversal, as a value of the enumeration.</param>
	public TraversalRequest(FocusNavigationDirection focusNavigationDirection)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
