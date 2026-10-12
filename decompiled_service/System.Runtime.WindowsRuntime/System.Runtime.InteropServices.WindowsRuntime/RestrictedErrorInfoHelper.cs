namespace System.Runtime.InteropServices.WindowsRuntime;

internal static class RestrictedErrorInfoHelper
{
	[DllImport("combase.dll", PreserveSig = false)]
	private static extern IRestrictedErrorInfo GetRestrictedErrorInfo();

	internal static Exception AttachRestrictedErrorInfo(Exception e)
	{
		if (e != null)
		{
			try
			{
				IRestrictedErrorInfo restrictedErrorInfo = GetRestrictedErrorInfo();
				if (restrictedErrorInfo != null)
				{
					restrictedErrorInfo.GetErrorDetails(out var _, out var error, out var restrictedDescription, out var capabilitySid);
					if (e.HResult == error)
					{
						restrictedErrorInfo.GetReference(out var reference);
						e.AddExceptionDataForRestrictedErrorInfo(restrictedDescription, reference, capabilitySid, restrictedErrorInfo);
					}
				}
			}
			catch
			{
			}
		}
		return e;
	}
}
