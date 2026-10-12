using System.Drawing;
using System.Windows.Forms;
using Infrastructure.Converters;

namespace NucSoftwareStudio.Models.TrayApp;

public class TrayAppMenuRenderer : ToolStripProfessionalRenderer
{
	private const string INTEL_CLEAR_FONT_NAME = "Intel Clear";

	public const int ItemPadding = 10;

	public static readonly Font MenuFont = new Font("Intel Clear", 10f);

	public TrayAppMenuRenderer()
		: base(new TrayAppColorTable())
	{
	}

	protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
	{
	}

	protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
	{
		if (e == null)
		{
			return;
		}
		if (!e.Item.Selected)
		{
			base.OnRenderMenuItemBackground(e);
		}
		else if (!e.Item.Text.Contains(ResourceDictionaryConverter.GetString("Version")))
		{
			Pen pen = new Pen(TrayAppColorTable.TRAY_MENU_BACKGROUND_HOVER_COLOR);
			SolidBrush solidBrush = new SolidBrush(TrayAppColorTable.TRAY_MENU_BACKGROUND_HOVER_COLOR);
			if (e.Item.ForeColor != TrayAppColorTable.TRAY_MENU_TEXT_CURRENTLY_ACTIVE_COLOR)
			{
				e.Item.ForeColor = TrayAppColorTable.TRAY_MENU_TEXT_HOVER_COLOR;
			}
			Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
			e.Graphics.FillRectangle(solidBrush, rect);
			e.Graphics.DrawRectangle(pen, 0, 0, rect.Width, rect.Height);
			pen.Dispose();
			solidBrush.Dispose();
		}
	}

	protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
	{
		if (e != null)
		{
			e.TextFormat &= ~TextFormatFlags.HidePrefix;
			e.TextFormat |= TextFormatFlags.VerticalCenter;
			Rectangle textRectangle = e.TextRectangle;
			textRectangle.Offset(0, 10);
			e.TextRectangle = textRectangle;
			base.OnRenderItemText(e);
		}
	}
}
