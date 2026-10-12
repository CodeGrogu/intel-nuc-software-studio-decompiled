using System.Diagnostics;

namespace Infrastructure.Logging;

public class NssServiceLogger
{
	private const string LOG_SOURCE = "IntelNucSoftwareStudioServiceLogSource";

	private const string LOG_NAME = "Intel NUC Software Studio Service Log";

	private static EventLog _nssLog;

	public NssServiceLogger()
	{
		_nssLog = new EventLog();
		if (!EventLog.SourceExists("IntelNucSoftwareStudioServiceLogSource"))
		{
			EventLog.CreateEventSource("IntelNucSoftwareStudioServiceLogSource", "Intel NUC Software Studio Service Log");
		}
		_nssLog.Source = "IntelNucSoftwareStudioServiceLogSource";
		_nssLog.Log = "Intel NUC Software Studio Service Log";
	}

	public void Log(string Message, LogEventId Id, EventLogEntryType Type = EventLogEntryType.Information)
	{
		_nssLog.WriteEntry(Message, Type, (int)Id);
	}
}
