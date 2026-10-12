using System.Collections.Generic;
using System.Windows.Media;
using NucLedMgr.WMI.Models;

namespace NucLedMgr.WMI.Interfaces;

public interface IColor
{
	Color CurrentColor { get; set; }

	List<Color> SupportedColors { get; }

	void SetColor(List<ControlItem> controlItems);

	List<ControlItem> GetControlItems();
}
