using System;

namespace MS.Internal.Security.RightsManagement;

internal delegate int CallbackDelegate(StatusMessage status, int hr, IntPtr pvParam, IntPtr pvContext);
