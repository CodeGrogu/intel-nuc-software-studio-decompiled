using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace System.Windows;

/// <summary>Provides a startup screen for a Windows Presentation Foundation (WPF) application. </summary>
[SecurityCritical(SecurityCriticalScope.Everything)]
[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
public class SplashScreen
{
	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.SplashScreen" /> class with the specified resource. </summary>
	/// <param name="resourceName">The name of the embedded resource.</param>
	[SecurityCritical]
	public SplashScreen(string resourceName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.SplashScreen" /> class with the specified resource assembly. </summary>
	/// <param name="resourceAssembly">The assembly that contains the embedded resource.</param>
	/// <param name="resourceName">The name of the embedded resource.</param>
	[SecurityCritical]
	public SplashScreen(Assembly resourceAssembly, string resourceName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Displays the splash screen. </summary>
	/// <param name="autoClose">
	///       <see langword="true" /> to automatically close the splash screen; <see langword="false" /> to close the splash screen manually.</param>
	/// <exception cref="T:System.IO.IOException">The resource specified in the constructor could not be found. </exception>
	[SecurityCritical]
	public void Show(bool autoClose)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Displays the splash screen.</summary>
	/// <param name="autoClose">
	///       <see langword="true" /> to automatically close the splash screen; <see langword="false" /> to close the splash screen manually.</param>
	/// <param name="topMost">
	///       <see langword="true" /> if the splash screen window should use the WS_EX_TOPMOST style; otherwise <see langword="false" />.</param>
	[SecurityCritical]
	public void Show(bool autoClose, bool topMost)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Closes the splash screen. </summary>
	/// <param name="fadeoutDuration">A <see cref="T:System.TimeSpan" /> that specifies how long it will take for the splash screen to fade after the close operation has been initiated. </param>
	[SecurityCritical]
	public void Close(TimeSpan fadeoutDuration)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
