using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using Infrastructure.Converters;
using Prism.Commands;
using Prism.Regions;

namespace CustomizationMgr.ViewModels;

public abstract class NavBaseSummaryViewModel : BaseSummaryViewModel
{
	private readonly IRegionManager _regionMgr;

	private readonly string _cardTitleString;

	private readonly string _detailCardViewName;

	protected DelegateCommand _openDetailView;

	public DelegateCommand OpenDetailView
	{
		get
		{
			return _openDetailView;
		}
		set
		{
			SetProperty(ref _openDetailView, value, "OpenDetailView");
		}
	}

	protected NavBaseSummaryViewModel(IRegionManager regionMgr, ICustomResourceManager resourceManager, string cardTitleString, string detailCardViewName, IconType iconType)
		: base(resourceManager, iconType)
	{
		_regionMgr = regionMgr;
		_cardTitleString = cardTitleString;
		_detailCardViewName = detailCardViewName;
		_openDetailView = new DelegateCommand(NavToDetailView);
	}

	private void NavToDetailView()
	{
		NavigationParameters navigationParameters = new NavigationParameters { 
		{
			"CardTitle",
			ResourceDictionaryConverter.GetString(_cardTitleString)
		} };
		_regionMgr.Regions["MainContentRegion"].RequestNavigate("DetailView", navigationParameters);
		_regionMgr.Regions["DetailContentRegion"].RequestNavigate(_detailCardViewName);
	}
}
