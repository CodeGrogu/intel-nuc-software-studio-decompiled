using System.Windows.Controls;
using System.Windows.Markup;
using MobileLightBarLedMgr.Interfaces;

namespace MobileLightBarLedMgr.Views;

public partial class LightbarDetailView : UserControl, IComponentConnector
{
	private readonly IMobileLightbarSettingsMgr _settingsMgr;

	public LightbarDetailView(IMobileLightbarSettingsMgr settingsMgr)
	{
		InitializeComponent();
		_settingsMgr = settingsMgr;
		DataContext = _settingsMgr;
	}
}
