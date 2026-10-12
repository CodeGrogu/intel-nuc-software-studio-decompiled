using System;
using System.Security;
using MS.Internal;

namespace MS.Utility;

internal abstract class TraceProvider
{
	protected bool _enabled;

	protected EventTrace.Level _level;

	protected EventTrace.Keyword _keywords;

	protected EventTrace.Keyword _matchAllKeyword;

	protected SecurityCriticalDataForSet<ulong> _registrationHandle;

	internal EventTrace.Keyword Keywords
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal EventTrace.Keyword MatchAllKeywords
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal EventTrace.Level Level
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[SecurityCritical]
	internal TraceProvider()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal abstract void Register(Guid providerGuid);

	[SecurityCritical]
	internal unsafe abstract uint EventWrite(EventTrace.Event eventID, EventTrace.Keyword keywords, EventTrace.Level level, int argc, EventData* argv);

	internal uint TraceEvent(EventTrace.Event eventID, EventTrace.Keyword keywords, EventTrace.Level level)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool IsEnabled(EventTrace.Keyword keyword, EventTrace.Level level)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal uint TraceEvent(EventTrace.Event eventID, EventTrace.Keyword keywords, EventTrace.Level level, object eventData)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal uint TraceEvent(EventTrace.Event eventID, EventTrace.Keyword keywords, EventTrace.Level level, params object[] eventPayload)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
