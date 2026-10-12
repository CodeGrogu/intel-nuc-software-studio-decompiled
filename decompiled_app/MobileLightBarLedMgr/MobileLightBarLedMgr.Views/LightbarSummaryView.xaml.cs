using System.Windows.Controls;
using System.Windows.Markup;
using MobileLightBarLedMgr.Interfaces;

namespace MobileLightBarLedMgr.Views;

public partial class LightbarSummaryView : UserControl, IComponentConnector
{
	public LightbarSummaryView(IMobileLightbarSettingsMgr settingsMgr)
	{
		InitializeComponent();
		DataContext = settingsMgr;
	}
}
