using System.Windows.Media;

namespace NucSoftwareStudio.Models;

public class OsdMessageData
{
	public DrawingImage Icon { get; }

	public string Message { get; }

	public OsdMessageData(string message, DrawingImage icon)
	{
		Message = message;
		Icon = icon;
	}
}
