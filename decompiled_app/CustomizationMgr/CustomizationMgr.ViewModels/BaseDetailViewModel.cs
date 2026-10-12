using System.Windows.Media.Imaging;
using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using Prism.Mvvm;

namespace CustomizationMgr.ViewModels;

public abstract class BaseDetailViewModel : BindableBase
{
	public BitmapSource CustomIconSource { get; }

	protected BaseDetailViewModel(ICustomResourceManager resourceManager, IconType iconType)
	{
		CustomIconSource = resourceManager.GetCustomIcon(iconType);
	}
}
