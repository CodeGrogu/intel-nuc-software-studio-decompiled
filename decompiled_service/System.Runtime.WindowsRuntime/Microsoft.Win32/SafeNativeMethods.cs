using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Microsoft.Win32;

[SuppressUnmanagedCodeSecurity]
internal class SafeNativeMethods
{
	public const int LOCALE_NAME_MAX_LENGTH = 85;

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern int ResolveLocaleName(string lpNameToResolve, StringBuilder lpLocaleName, int cchLocaleName);
}
