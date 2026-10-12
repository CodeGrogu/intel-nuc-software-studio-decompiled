using System.Drawing;
using System.Windows.Forms;

namespace NucSoftwareStudio.Models.TrayApp;

public class TrayAppColorTable : ProfessionalColorTable
{
	public static readonly Color TRAY_MENU_BACKGROUND_COLOR = Color.FromArgb(12, 21, 29);

	public static readonly Color TRAY_MENU_TEXT_COLOR = Color.White;

	public static readonly Color TRAY_MENU_BACKGROUND_HOVER_COLOR = Color.FromArgb(61, 61, 61);

	public static readonly Color TRAY_MENU_TEXT_HOVER_COLOR = Color.White;

	public static readonly Color TRAY_MENU_TEXT_CURRENTLY_ACTIVE_COLOR = Color.FromArgb(0, 174, 239);

	public override Color MenuItemBorder => TRAY_MENU_BACKGROUND_COLOR;

	public override Color MenuItemSelected => TRAY_MENU_BACKGROUND_COLOR;

	public override Color ToolStripDropDownBackground => TRAY_MENU_BACKGROUND_COLOR;

	public override Color ImageMarginGradientBegin => TRAY_MENU_BACKGROUND_COLOR;

	public override Color ImageMarginGradientMiddle => TRAY_MENU_BACKGROUND_COLOR;

	public override Color ImageMarginGradientEnd => TRAY_MENU_BACKGROUND_COLOR;
}
