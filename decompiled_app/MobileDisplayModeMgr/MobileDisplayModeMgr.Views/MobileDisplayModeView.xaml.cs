using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using MobileDisplayModeMgr.Interfaces;

namespace MobileDisplayModeMgr.Views;

public partial class MobileDisplayModeView : UserControl, IComponentConnector
{
	public MobileDisplayModeView(IMobileDisplayModeSettingsMgr settingsMgr)
	{
		InitializeComponent();
		DataContext = settingsMgr;
	}

	private void IntegerValidation_PreviewTextInput(object sender, TextCompositionEventArgs e)
	{
		Regex regex = new Regex("[^0-9]+");
		e.Handled = regex.IsMatch(e.Text);
	}

	private void RefreshTextBoxValue(object sender, RoutedEventArgs e)
	{
		BindingOperations.GetBindingExpression((TextBox)sender, TextBox.TextProperty).UpdateTarget();
	}

	private void ChangeFocus(object sender, MouseButtonEventArgs e)
	{
		((Grid)sender).Focus();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
