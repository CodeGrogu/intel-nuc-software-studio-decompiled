using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;

namespace SystemMonitorMgr.Views;

public partial class SystemMonitorButtonView : UserControl, IComponentConnector
{
	public static readonly DependencyProperty CustomIconProperty = DependencyProperty.Register("CustomIcon", typeof(BitmapSource), typeof(SystemMonitorButtonView));

	public BitmapSource CustomIcon
	{
		get
		{
			return (BitmapSource)GetValue(CustomIconProperty);
		}
		set
		{
			SetValue(CustomIconProperty, value);
		}
	}

	public SystemMonitorButtonView(ICustomResourceManager customResources)
	{
		InitializeComponent();
		CustomIcon = customResources.GetCustomIcon(IconType.SystemMonitor);
	}
}
