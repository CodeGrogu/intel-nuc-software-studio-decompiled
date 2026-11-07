using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NucControl.Core;

namespace NucControl.App;

internal sealed class LightingPresetsDialog : ContentDialog
{
    private readonly ControlSession session;
    private readonly LightingPresetStore store;
    private readonly ComboBox choices = new() { Header = "Saved presets", HorizontalAlignment = HorizontalAlignment.Stretch };
    private readonly TextBox name = new() { Header = "New preset name", MaxLength = 64 };
    private readonly TextBlock detail = new() { TextWrapping = TextWrapping.Wrap, MaxWidth = 340 };
    private readonly Button capture = new() { Content = "Save current lighting locally" };
    private readonly Button delete = new() { Content = "Delete selected preset" };
    private LightingPreset[] presets;
    private bool capturing;
    public LightingPreset? Selected => (choices.SelectedItem as ComboBoxItem)?.Tag as LightingPreset;
    public LightingPresetsDialog(ControlSession session, LightingPresetStore store)
    {
        this.session = session; this.store = store; presets = store.Load();
        Title = "Lighting presets"; PrimaryButtonText = "Review Apply"; CloseButtonText = "Close"; DefaultButton = ContentDialogButton.Close;
        var panel = new StackPanel { Spacing = 12 };
        panel.Children.Add(new TextBlock { Text = "Save the current active function for each light. Selecting a preset does not apply it. Presets never run at startup or save to BIOS.", TextWrapping = TextWrapping.Wrap, MaxWidth = 340 });
        panel.Children.Add(choices); panel.Children.Add(detail); panel.Children.Add(delete); panel.Children.Add(name); panel.Children.Add(capture);
        Content = new ScrollViewer { Content = panel, MaxHeight = 430 };
        choices.SelectionChanged += (_, _) => { detail.Text = Selected?.Summary ?? "No preset selected"; IsPrimaryButtonEnabled = Selected is not null; delete.IsEnabled = Selected is not null; };
        capture.Click += async (_, _) => await Capture();
        delete.Click += (_, _) =>
        {
            try { var selected = Selected; if (selected is null) return; var updated = presets.Where(p => p != selected).ToArray(); store.Save(updated); presets = updated; Reload(); }
            catch (Exception e) { detail.Text = e.Message; }
        };
        Closing += (_, e) => { if (capturing) e.Cancel = true; };
        Reload();
    }
    private void Reload(string? selected = null)
    {
        choices.Items.Clear();
        foreach (var preset in presets) choices.Items.Add(new ComboBoxItem { Content = preset.Name, Tag = preset });
        choices.SelectedItem = choices.Items.Cast<ComboBoxItem>().FirstOrDefault(c => ((LightingPreset)c.Tag).Name == selected);
        IsPrimaryButtonEnabled = Selected is not null; delete.IsEnabled = Selected is not null;
    }
    private async Task Capture()
    {
        var label = name.Text.Trim();
        if (label.Length == 0 || presets.Any(p => string.Equals(p.Name, label, StringComparison.OrdinalIgnoreCase))) { detail.Text = "Enter a new, unique preset name."; return; }
        capture.IsEnabled = false; delete.IsEnabled = false; choices.IsEnabled = false; name.IsEnabled = false; IsPrimaryButtonEnabled = false; capturing = true;
        try
        {
            var preset = await LightingPreset.CaptureAsync(label, session.State!, (z,m) => session.ReadAsync<LightingModeState>("GetLightingMode", "LightingMode", new { zone_id = z, indicator_mode = m }));
            var updated = presets.Append(preset).ToArray(); store.Save(updated); presets = updated; Reload(preset.Name); name.Text = "";
        }
        catch (Exception e) { detail.Text = e.Message; }
        finally { capture.IsEnabled = true; choices.IsEnabled = true; name.IsEnabled = true; capturing = false; delete.IsEnabled = Selected is not null; IsPrimaryButtonEnabled = Selected is not null; }
    }
}
