using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using Prism.Regions;

namespace CustomizationMgr.ViewModels;

public abstract class NavAwareBaseDetailViewModel : BaseDetailViewModel, INavigationAware
{
	protected NavAwareBaseDetailViewModel(ICustomResourceManager resourceManager, IconType iconType)
		: base(resourceManager, iconType)
	{
	}

	public bool IsNavigationTarget(NavigationContext navigationContext)
	{
		return true;
	}

	public void OnNavigatedFrom(NavigationContext navigationContext)
	{
	}

	public void OnNavigatedTo(NavigationContext navigationContext)
	{
	}
}
