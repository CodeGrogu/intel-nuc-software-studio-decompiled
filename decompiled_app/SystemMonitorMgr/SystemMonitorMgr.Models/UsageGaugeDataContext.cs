using System;
using System.Windows;
using SystemMonitorMgr.Converters;
using SystemMonitorMgr.Enums;
using Infrastructure.Converters;
using Prism.Events;
using Prism.Mvvm;

namespace SystemMonitorMgr.Models;

public class UsageGaugeDataContext : BindableBase
{
	private const int BASE_THICKNESS = 8;

	private const int USAGE_THICKNESS_INCR = 10;

	private const int DEFAULT_CONTROL_RADIUS = 150;

	private string _percentageString;

	private readonly bool _showLabel;

	private string _label;

	private int? _temp;

	private readonly double _radius;

	private int _usageThickness;

	private double _width;

	private double _height;

	private Thickness _margin;

	private Point _startPoint;

	private Point _endPoint;

	private Size _arcSize;

	private bool _isLargeArc;

	private double _baseWidth;

	private double _baseHeight;

	private int _baseThickness;

	private Thickness _baseMargin;

	private Point _baseStartPoint;

	private Point _baseEndPoint;

	private Size _baseArcSize;

	private bool _displayGauge;

	public SystemDataType Type { get; private set; }

	public string PercentageString
	{
		get
		{
			return _percentageString;
		}
		set
		{
			SetProperty(ref _percentageString, value, "PercentageString");
		}
	}

	public string Label
	{
		get
		{
			return _label;
		}
		set
		{
			SetProperty(ref _label, value, "Label");
		}
	}

	public int? Temp
	{
		get
		{
			return _temp;
		}
		set
		{
			SetProperty(ref _temp, value, "Temp");
			RaisePropertyChanged("TempString");
		}
	}

	public string TempString
	{
		get
		{
			if (_temp.HasValue)
			{
				return _temp + ResourceDictionaryConverter.GetString("Celsius");
			}
			return "";
		}
	}

	public int UsageThickness
	{
		get
		{
			return _usageThickness;
		}
		set
		{
			SetProperty(ref _usageThickness, value, "UsageThickness");
		}
	}

	public double Width
	{
		get
		{
			return _width;
		}
		set
		{
			SetProperty(ref _width, value, "Width");
		}
	}

	public double Height
	{
		get
		{
			return _height;
		}
		set
		{
			SetProperty(ref _height, value, "Height");
		}
	}

	public Thickness Margin
	{
		get
		{
			return _margin;
		}
		set
		{
			SetProperty(ref _margin, value, "Margin");
		}
	}

	public Point StartPoint
	{
		get
		{
			return _startPoint;
		}
		set
		{
			SetProperty(ref _startPoint, value, "StartPoint");
		}
	}

	public Point EndPoint
	{
		get
		{
			return _endPoint;
		}
		set
		{
			SetProperty(ref _endPoint, value, "EndPoint");
		}
	}

	public Size ArcSize
	{
		get
		{
			return _arcSize;
		}
		set
		{
			SetProperty(ref _arcSize, value, "ArcSize");
		}
	}

	public bool IsLargeArc
	{
		get
		{
			return _isLargeArc;
		}
		set
		{
			SetProperty(ref _isLargeArc, value, "IsLargeArc");
		}
	}

	public int BaseThickness
	{
		get
		{
			return _baseThickness;
		}
		set
		{
			SetProperty(ref _baseThickness, value, "BaseThickness");
		}
	}

	public double BaseWidth
	{
		get
		{
			return _baseWidth;
		}
		set
		{
			SetProperty(ref _baseWidth, value, "BaseWidth");
		}
	}

	public double BaseHeight
	{
		get
		{
			return _baseHeight;
		}
		set
		{
			SetProperty(ref _baseHeight, value, "BaseHeight");
		}
	}

	public Thickness BaseMargin
	{
		get
		{
			return _baseMargin;
		}
		set
		{
			SetProperty(ref _baseMargin, value, "BaseMargin");
		}
	}

	public Point BaseStartPoint
	{
		get
		{
			return _baseStartPoint;
		}
		set
		{
			SetProperty(ref _baseStartPoint, value, "BaseStartPoint");
		}
	}

	public Point BaseEndPoint
	{
		get
		{
			return _baseEndPoint;
		}
		set
		{
			SetProperty(ref _baseEndPoint, value, "BaseEndPoint");
		}
	}

	public Size BaseArcSize
	{
		get
		{
			return _baseArcSize;
		}
		set
		{
			SetProperty(ref _baseArcSize, value, "BaseArcSize");
		}
	}

	public bool DisplayGauge
	{
		get
		{
			return _displayGauge;
		}
		set
		{
			SetProperty(ref _displayGauge, value, "DisplayGauge");
		}
	}

	public UsageGaugeDataContext(SystemDataType type, PubSubEvent<UsageData> usageChangeEvent, double percentage = 0.0, int temp = 0)
	{
		Type = type;
		usageChangeEvent?.Subscribe(UpdateUsage);
		_label = UsageGaugeLabelConverter.LabelFromType(Type);
		_temp = temp;
		_radius = 150.0;
		_usageThickness = 18;
		_baseThickness = 8;
		_baseStartPoint = new Point(_radius, 0.0);
		_baseEndPoint = new Point(_radius, 0.0);
		_baseEndPoint.X -= 0.01;
		_baseArcSize = new Size(_radius, _radius);
		_baseWidth = _radius * 2.0 + (double)UsageThickness;
		_baseHeight = _radius * 2.0 + (double)UsageThickness;
		_baseMargin = new Thickness(UsageThickness, UsageThickness, 0.0, 0.0);
		_width = _radius * 2.0 + (double)UsageThickness;
		_height = _radius * 2.0 + (double)UsageThickness;
		_margin = new Thickness(UsageThickness, UsageThickness, 0.0, 0.0);
		_displayGauge = true;
		RenderArc(percentage);
	}

	private void UpdateUsage(UsageData data)
	{
		if (data.Type == Type)
		{
			RenderArc(data.Usage);
			Temp = data.Temp;
			DisplayGauge = true;
		}
	}

	public void RenderArc(double percentage)
	{
		PercentageString = percentage + ResourceDictionaryConverter.GetString("PercentSymbol");
		double num = percentage * 360.0 / 100.0;
		Point startPoint = new Point(_radius, 0.0);
		Point cartesianCoords = GetCartesianCoords(num, _radius);
		cartesianCoords.X += _radius;
		cartesianCoords.Y += _radius;
		if (startPoint.X == Math.Round(cartesianCoords.X) && startPoint.Y == Math.Round(cartesianCoords.Y))
		{
			cartesianCoords.X -= 0.01;
		}
		bool isLargeArc = num > 180.0;
		Size arcSize = new Size(_radius, _radius);
		StartPoint = startPoint;
		EndPoint = cartesianCoords;
		ArcSize = arcSize;
		IsLargeArc = isLargeArc;
	}

	private Point GetCartesianCoords(double angle, double radius)
	{
		double num = Math.PI / 180.0 * (angle - 90.0);
		double x = radius * Math.Cos(num);
		double y = radius * Math.Sin(num);
		return new Point(x, y);
	}
}
