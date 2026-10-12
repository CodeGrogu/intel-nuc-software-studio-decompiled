using System.Security;
using System.Security.Permissions;
using System.Windows.Input;

namespace System.Windows.Interop;

/// <summary>Manages keyboard focus within the container.  This interface implements keyboard message management in WPF-Win32 interoperation scenarios.</summary>
public interface IKeyboardInputSite
{
	/// <summary>Gets the keyboard sink associated with this site. </summary>
	/// <returns>The current site's <see cref="T:System.Windows.Interop.IKeyboardInputSink" /> interface.</returns>
	IKeyboardInputSink Sink { get; }

	/// <summary>Unregisters a child keyboard input sink from this site. </summary>
	[SecurityCritical]
	[UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	void Unregister();

	/// <summary>Called by a contained component when it has reached its last tab stop and has no further items to tab to. </summary>
	/// <param name="request">Specifies whether focus should be set to the first or the last tab stop.</param>
	/// <returns>If this method returns <see langword="true" />, the site has shifted focus to another component. If this method returns <see langword="false" />, focus is still within the calling component. The component should "wrap around" and set focus to its first contained tab stop.</returns>
	bool OnNoMoreTabStops(TraversalRequest request);
}
