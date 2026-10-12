using System.Collections.Generic;
using System.Windows.Media;
using Newtonsoft.Json;
using NucLedMgr.WMI.Interfaces;
using Prism.Mvvm;

namespace NucLedMgr.WMI.Models.ColorTypes;

[JsonObject]
public class SingleColor : BindableBase, IColor
{
	private Color _color;

	public Color CurrentColor
	{
		get
		{
			return _color;
		}
		set
		{
		}
	}

	public List<Color> SupportedColors { get; private set; }

	public SingleColor(Color color)
	{
		_color = color;
	}

	public List<ControlItem> GetControlItems()
	{
		return new List<ControlItem>();
	}

	public void InitializeColor(List<ControlItem> controlItems, Color color)
	{
		_color = color;
		SupportedColors = new List<Color> { _color };
	}

	public void SetColor(List<ControlItem> controlItems)
	{
	}
}
