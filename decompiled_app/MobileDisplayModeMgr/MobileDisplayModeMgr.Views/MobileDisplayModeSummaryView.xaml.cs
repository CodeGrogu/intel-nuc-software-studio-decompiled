using System.Windows.Controls;
using System.Windows.Markup;
using MobileDisplayModeMgr.Interfaces;

namespace MobileDisplayModeMgr.Views;

public partial class MobileDisplayModeSummaryView : UserControl, IComponentConnector
{
	public MobileDisplayModeSummaryView(IMobileDisplayModeSettingsMgr settingsMgr)
	{
		InitializeComponent();
		DataContext = settingsMgr;
	}
}
