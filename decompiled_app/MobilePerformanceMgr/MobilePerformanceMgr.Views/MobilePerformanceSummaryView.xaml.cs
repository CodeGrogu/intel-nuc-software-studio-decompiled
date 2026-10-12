using System.Windows.Controls;
using System.Windows.Markup;
using MobilePerformanceMgr.Models;

namespace MobilePerformanceMgr.Views;

public partial class MobilePerformanceSummaryView : UserControl, IComponentConnector
{
	public MobilePerformanceSummaryView(IMobilePerformanceSettingsMgr settingsMgr)
	{
		InitializeComponent();
		DataContext = settingsMgr;
	}
}
