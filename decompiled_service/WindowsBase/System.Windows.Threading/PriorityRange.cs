namespace System.Windows.Threading;

internal struct PriorityRange
{
	public static readonly PriorityRange All;

	public static readonly PriorityRange None;

	private DispatcherPriority _min;

	private bool _isMinInclusive;

	private DispatcherPriority _max;

	private bool _isMaxInclusive;

	public DispatcherPriority Min
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public DispatcherPriority Max
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public bool IsMinInclusive
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public bool IsMaxInclusive
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public bool IsValid
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public PriorityRange(DispatcherPriority min, DispatcherPriority max)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public PriorityRange(DispatcherPriority min, bool isMinInclusive, DispatcherPriority max, bool isMaxInclusive)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public bool Contains(DispatcherPriority priority)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public bool Contains(PriorityRange priorityRange)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override bool Equals(object o)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public bool Equals(PriorityRange priorityRange)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public static bool operator ==(PriorityRange priorityRange1, PriorityRange priorityRange2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public static bool operator !=(PriorityRange priorityRange1, PriorityRange priorityRange2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static PriorityRange()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
