using System;
using System.Security;

namespace MS.Utility;

internal sealed class ClassicTraceProvider : TraceProvider
{
	[SecurityCritical]
	internal ClassicTraceProvider()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal override void Register(Guid providerGuid)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	~ClassicTraceProvider()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal unsafe override uint EventWrite(EventTrace.Event eventID, EventTrace.Keyword keywords, EventTrace.Level level, int argc, EventData* argv)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
