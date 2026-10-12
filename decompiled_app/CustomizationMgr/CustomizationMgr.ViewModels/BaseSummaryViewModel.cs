using System.Windows.Media.Imaging;
using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using Prism.Mvvm;

namespace CustomizationMgr.ViewModels;

public abstract class BaseSummaryViewModel : BindableBase
{
	public BitmapSource CustomIconSource { get; private set; }

	protected BaseSummaryViewModel(ICustomResourceManager resourceManager, IconType iconType)
	{
		CustomIconSource = resourceManager.GetCustomIcon(iconType);
	}
}
