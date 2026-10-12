using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Markup;
using MobileKeyboardLedMgr.Interfaces;

namespace MobileKeyboardLedMgr.Views;

public partial class KeyboardSummaryView : UserControl, IComponentConnector
{
	public KeyboardSummaryView(IKeyboardProfileManager keyboardProfileMgr)
	{
		InitializeComponent();
		DataContext = keyboardProfileMgr;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
