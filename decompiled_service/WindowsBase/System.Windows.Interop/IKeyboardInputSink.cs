using System.Security;
using System.Security.Permissions;
using System.Windows.Input;

namespace System.Windows.Interop;

/// <summary>Provides a keyboard sink for components that manages tabbing, accelerators, and mnemonics across interop boundaries and between HWNDs. This interface implements keyboard message management in WPF-Win32 interoperation scenarios.</summary>
public interface IKeyboardInputSink
{
	/// <summary>Gets or sets a reference to the component's container's <see cref="T:System.Windows.Interop.IKeyboardInputSite" /> interface. </summary>
	/// <returns>A reference to the container's <see cref="T:System.Windows.Interop.IKeyboardInputSite" /> interface.</returns>
	IKeyboardInputSite KeyboardInputSite
	{
		get;
		[SecurityCritical]
		[UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		set;
	}

	/// <summary>Registers the <see cref="T:System.Windows.Interop.IKeyboardInputSink" /> interface of a contained component. </summary>
	/// <param name="sink">The <see cref="T:System.Windows.Interop.IKeyboardInputSink" /> sink of the contained component.</param>
	/// <returns>The <see cref="T:System.Windows.Interop.IKeyboardInputSite" /> site of the contained component.</returns>
	[SecurityCritical]
	[UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	IKeyboardInputSite RegisterKeyboardInputSink(IKeyboardInputSink sink);

	/// <summary>Processes keyboard input at the keydown message level.</summary>
	/// <param name="msg">The message and associated data. Do not modify this structure. It is passed by reference for performance reasons only.</param>
	/// <param name="modifiers">Modifier keys.</param>
	/// <returns>
	///     <see langword="true" /> if the message was handled by the method implementation; otherwise, <see langword="false" />.</returns>
	[SecurityCritical]
	[UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	bool TranslateAccelerator(ref MSG msg, ModifierKeys modifiers);

	/// <summary>Sets focus on either the first tab stop or the last tab stop of the sink. </summary>
	/// <param name="request">Specifies whether focus should be set to the first or the last tab stop.</param>
	/// <returns>
	///     <see langword="true" /> if the focus has been set as requested; <see langword="false" />, if there are no tab stops.</returns>
	bool TabInto(TraversalRequest request);

	/// <summary>Called when one of the mnemonics (access keys) for this sink is invoked. </summary>
	/// <param name="msg">The message for the mnemonic and associated data. Do not modify this message structure. It is passed by reference for performance reasons only.</param>
	/// <param name="modifiers">Modifier keys.</param>
	/// <returns>
	///     <see langword="true" /> if the message was handled; otherwise, <see langword="false" />.</returns>
	[SecurityCritical]
	[UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	bool OnMnemonic(ref MSG msg, ModifierKeys modifiers);

	/// <summary>Processes WM_CHAR, WM_SYSCHAR, WM_DEADCHAR, and WM_SYSDEADCHAR input messages before <see cref="M:System.Windows.Interop.IKeyboardInputSink.OnMnemonic(System.Windows.Interop.MSG@,System.Windows.Input.ModifierKeys)" /> is called. </summary>
	/// <param name="msg">The message and associated data. Do not modify this structure. It is passed by reference for performance reasons only.</param>
	/// <param name="modifiers">Modifier keys.</param>
	/// <returns>
	///     <see langword="true" /> if the message was processed and <see cref="M:System.Windows.Interop.IKeyboardInputSink.OnMnemonic(System.Windows.Interop.MSG@,System.Windows.Input.ModifierKeys)" /> should not be called; otherwise, <see langword="false" />.</returns>
	[SecurityCritical]
	[UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	bool TranslateChar(ref MSG msg, ModifierKeys modifiers);

	/// <summary>Gets a value that indicates whether the sink or one of its contained components has focus. </summary>
	/// <returns>
	///     <see langword="true" /> if the sink or one of its contained components has focus; otherwise, <see langword="false" />.</returns>
	bool HasFocusWithin();
}
