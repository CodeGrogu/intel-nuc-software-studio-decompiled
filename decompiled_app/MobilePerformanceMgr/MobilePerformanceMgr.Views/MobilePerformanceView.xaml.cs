using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using MobilePerformanceMgr.Models;

namespace MobilePerformanceMgr.Views;

public partial class MobilePerformanceView : UserControl, IComponentConnector
{
	public MobilePerformanceView(IMobilePerformanceSettingsMgr settingsMgr)
	{
		InitializeComponent();
		DataContext = settingsMgr;
		Focusable = true;
		Loaded += (object s, RoutedEventArgs e) =>
		{
			Keyboard.Focus(this);
		};
	}
}
