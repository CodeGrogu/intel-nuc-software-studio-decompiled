using CommunityToolkit.Mvvm.Input;
using H.NotifyIcon;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using NucControl.Core;
using System.Text.Json.Nodes;
using Windows.Graphics;
using Windows.UI;

namespace NucControl.App;

public sealed partial class MainWindow : Window
{
    private readonly IBackend backend;
    private readonly ControlSession session;
    private readonly bool demo;
    private readonly PreferenceStore preferences = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NucControl"));
    private readonly DispatcherTimer timer = new() { Interval = TimeSpan.FromSeconds(2) };
    private readonly TaskbarIcon tray;
    private readonly DesktopIntegration.Subclass powerCallback;
    private LightingState? lighting;
    private int[] rgb = [0, 0, 255];
    private bool dirty, loading, busy, polling, changingZone, exiting, connected, modalOpen;
    private int selectedZone = 2;
    private int ticks;
    private bool visible = true;
    private static readonly (int Id, string Name)[] ZoneNames = [(0, "Power button"), (2, "Skull"), (4, "Front left"), (5, "Front middle"), (6, "Front right")];

    public MainWindow(IBackend backend, bool demo)
    {
        InitializeComponent();
        this.backend = backend; this.demo = demo; session = new(backend);
        selectedZone = preferences.Load().SelectedZone;
        var scale = DesktopIntegration.GetDpiForWindow(WinRT.Interop.WindowNative.GetWindowHandle(this)) / 96.0;
        AppWindow.Resize(new SizeInt32((int)(480 * scale), (int)(680 * scale)));
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets", "NucControl.ico"));
        AppWindow.Closing += (_, e) => { if (!exiting) { e.Cancel = true; Hide(); } };
        Activated += async (_, e) =>
        {
            if (e.WindowActivationState != WindowActivationState.Deactivated && !visible) visible = true;
            if (e.WindowActivationState != WindowActivationState.Deactivated) await SafeRefresh();
        };
        var menu = new MenuFlyout();
        void Item(string text, Action action) { var item = new MenuFlyoutItem { Text = text }; item.Click += (_, _) => action(); menu.Items.Add(item); }
        Item("Open", Show); Item("Settings", () => { Show(); _ = ShowSettings(); });
        menu.Items.Add(new MenuFlyoutSeparator()); Item("Exit", Exit);
        tray = new TaskbarIcon
        {
            ToolTipText = demo ? "NUC Control · Demo" : "NUC Control",
            ContextMenuMode = ContextMenuMode.SecondWindow,
            ContextFlyout = menu,
            IconSource = new BitmapImage(new Uri(Path.Combine(AppContext.BaseDirectory, "Assets", "NucControl.ico"))),
            LeftClickCommand = new RelayCommand(Show)
        };
        tray.ForceCreate(enablesEfficiencyMode: false);
        SettingsButton.Click += async (_, _) => await ShowSettings();
        ReconnectButton.Click += async (_, _) => await SafeRefresh();
        ApplyButton.Click += async (_, _) => await ApplyLighting();
        DiscardButton.Click += (_, _) => { if (lighting is not null) LoadDraft(lighting); };
        ColorButton.Click += async (_, _) => await PickColor();
        Brightness.ValueChanged += (_, _) => { if (!loading) { dirty = true; UpdateEnabled(); } BrightnessLabel.Text = $"Brightness · {Brightness.Value:0}%"; };
        Zones.SelectionChanged += async (_, _) => await ChangeZone();
        timer.Tick += async (_, _) =>
        {
            if (!visible || busy || polling || modalOpen) return;
            polling = true;
            try { if (++ticks % 5 == 0) await Refresh(); else await RefreshTelemetry(); }
            catch (Exception e) { ShowError(e); }
            finally { polling = false; UpdateEnabled(); }
        };
        powerCallback = (hwnd, message, wparam, lparam, id, data) =>
        {
            if (message == 0x24 && lparam != 0) { DesktopIntegration.SetMinimumSize(hwnd, lparam); return 0; }
            if (message == 0x218 && (wparam == 7 || wparam == 18)) DispatcherQueue.TryEnqueue(async () => { if (visible) await SafeRefresh(); });
            return DesktopIntegration.DefSubclassProc(hwnd, message, wparam, lparam);
        };
        DesktopIntegration.SetWindowSubclass(WinRT.Interop.WindowNative.GetWindowHandle(this), powerCallback, 1, 0);
        Root.Loaded += async (_, _) => { timer.Start(); await SafeRefresh(); };
        UpdateEnabled();
    }
    public void Show() { visible = true; AppWindow.Show(); Activate(); }
    public void Hide() { visible = false; AppWindow.Hide(); }
    private void Exit()
    {
        if (busy) { Show(); SetNotice("Wait for the current operation to finish before exiting.", InfoBarSeverity.Warning); return; }
        exiting = true; timer.Stop(); tray.Dispose(); Close(); Application.Current.Exit();
    }
    private void SetNotice(string message, InfoBarSeverity severity = InfoBarSeverity.Informational)
    {
        Notice.Message = demo ? $"Demo · {message}" : message; Notice.Severity = severity; Notice.IsOpen = true;
    }
    private void ShowError(Exception e)
    {
        connected = false;
        CpuReading.Text = "Unavailable"; GpuReading.Text = "Unavailable";
        SetNotice(session.PendingId is null ? $"{e.Message} Refresh to reconnect." : "Write outcome unknown. Refresh to reconcile; the command will not be retried.", InfoBarSeverity.Warning);
        EnableChildren(PowerButtons, false); EnableChildren(FanButtons, false); EnableChildren(LightEditor, false); ApplyButton.IsEnabled = false;
    }
    private async Task SafeRefresh()
    {
        if (busy || polling || modalOpen) return;
        polling = true;
        try { await Refresh(); }
        catch (Exception e) { ShowError(e); }
        finally { polling = false; UpdateEnabled(); }
    }
    private async Task Refresh()
    {
        await session.RefreshAsync();
        connected = true;
        SetNotice(session.Message, session.Info?.ProtocolRevision == 6 ? InfoBarSeverity.Informational : InfoBarSeverity.Warning);
        var state = session.State;
        if (state is null) { PowerDetail.Text = "Update the backend to enable controls."; FanDetail.Text = "Controls unavailable"; lighting = null; return; }
        var kinds = state.PowerPresets.Select(p => p.PresetKind).ToArray();
        if (!PowerButtons.Children.OfType<ToggleButton>().Select(b => (string)b.Tag).SequenceEqual(kinds)) PowerButtons.Children.Clear();
        for (var i = 0; i < state.PowerPresets.Length; i++)
        {
            var preset = state.PowerPresets[i];
            var current = state.PowerLimits == new PowerLimits(preset.Pl1Watts, preset.Pl2Watts, preset.TauSeconds);
            if (i < PowerButtons.Children.Count) { ((ToggleButton)PowerButtons.Children[i]).IsChecked = current; continue; }
            var button = new ToggleButton { Tag = preset.PresetKind, Content = preset.PresetKind switch { "LowPower" => "Low power", "MaxPerformance" => "Performance", _ => "Balanced" }, IsChecked = current, HorizontalAlignment = HorizontalAlignment.Stretch };
            ToolTipService.SetToolTip(button, $"{preset.Pl1Watts} W sustained · {preset.Pl2Watts} W turbo · {preset.TauSeconds} s");
            Grid.SetColumn(button, i); PowerButtons.Children.Add(button);
            button.Click += async (_, _) => await Mutate(Wire.Request("Power", new { preset = preset.PresetKind }));
        }
        foreach (var (name, index) in new[] { ("Balanced", 0), ("Cool", 1) })
        {
            if (index < FanButtons.Children.Count) { ((ToggleButton)FanButtons.Children[index]).IsChecked = state.FanPreset == name; continue; }
            var button = new ToggleButton { Content = name, IsChecked = state.FanPreset == name, HorizontalAlignment = HorizontalAlignment.Stretch };
            Grid.SetColumn(button, index); FanButtons.Children.Add(button);
            button.Click += async (_, _) => await Mutate(Wire.Request("Fan", new { preset = name }));
        }
        PowerDetail.Text = state.PowerUnavailable ?? (state.PowerLimits is { } p ? $"{p.Pl1Watts} W sustained · {p.Pl2Watts} W turbo · {p.TauSeconds} s" : "Power readback unavailable");
        if (state.PowerLimits is { } actual && !state.PowerPresets.Any(p => actual == new PowerLimits(p.Pl1Watts, p.Pl2Watts, p.TauSeconds))) PowerDetail.Text += " · Another setting";
        FanDetail.Text = state.FanUnavailable ?? "Stock firmware preset · both physical fans";
        changingZone = true;
        var zones = ZoneNames.Where(z => (state.LedZoneMask & (1u << z.Id)) != 0).ToArray();
        if (!Zones.Items.Cast<ComboBoxItem>().Select(z => (int)z.Tag).SequenceEqual(zones.Select(z => z.Id)))
        {
            Zones.Items.Clear();
            foreach (var zone in zones) Zones.Items.Add(new ComboBoxItem { Content = zone.Name, Tag = zone.Id });
        }
        Zones.SelectedItem = Zones.Items.Cast<ComboBoxItem>().FirstOrDefault(z => (int)z.Tag == selectedZone) ?? Zones.Items.FirstOrDefault();
        changingZone = false;
        if (!dirty && Zones.SelectedItem is ComboBoxItem selected) { selectedZone = (int)selected.Tag; await ReadLighting(); }
        await RefreshTelemetry();
        UpdateEnabled();
    }
    private async Task RefreshTelemetry()
    {
        if (session.Info?.ProtocolRevision != 6) return;
        var telemetry = await session.ReadAsync<TelemetryStatus>("GetTelemetryStatus", "TelemetryStatus");
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        CpuReading.Text = $"{telemetry.CpuTemperature.Display(" °C", now)} · {telemetry.CpuFan.Display(" RPM", now)}";
        GpuReading.Text = $"{telemetry.GpuTemperature.Display(" °C", now)} · {telemetry.GpuFan.Display(" RPM", now)}";
        ToolTipService.SetToolTip(CpuReading, $"{telemetry.CpuTemperature.Source}; {telemetry.CpuFan.Source}");
        ToolTipService.SetToolTip(GpuReading, $"{telemetry.GpuTemperature.Source}; {telemetry.GpuFan.Source}");
    }
    private async Task ReadLighting()
    {
        try
        {
            lighting = await session.ReadAsync<LightingState>("GetAwakeLighting", "AwakeLighting", new { zone_id = selectedZone });
            LoadDraft(lighting);
        }
        catch (Exception e)
        {
            lighting = null;
            LightDetail.Text = $"This light is unavailable: {e.Message}";
            UpdateEnabled();
        }
    }
    private void LoadDraft(LightingState state)
    {
        loading = true;
        Brightness.Value = state.Value(0) ?? 0;
        rgb = [state.Value(3) ?? 0, state.Value(4) ?? 0, state.Value(5) ?? 0];
        Swatch.Background = new SolidColorBrush(ColorHelper.FromArgb(255, (byte)rgb[0], (byte)rgb[1], (byte)rgb[2]));
        var role = state.ActiveRole switch { 0 => "Power state", 1 => "Disk activity", 2 => "Ethernet activity", 3 => "Wi-Fi activity", 4 => "Software", 5 => "Power limit", 6 => "Disabled", _ => "Unknown" };
        LightDetail.Text = state.Unavailable ?? (state.ActiveRole == 0 ? "Awake color and brightness" : $"Current function: {role}. Apply replaces it with manual power-state lighting, including its stored sleep behavior.");
        dirty = false; loading = false; UpdateEnabled();
    }
    private async Task ChangeZone()
    {
        if (changingZone || Zones.SelectedItem is not ComboBoxItem selected) return;
        var next = (int)selected.Tag;
        if (next == selectedZone) return;
        if (dirty)
        {
            var dialog = new ContentDialog { XamlRoot = Root.XamlRoot, Title = "Unsaved lighting", Content = "Apply this light's changes before choosing another?", PrimaryButtonText = "Apply", SecondaryButtonText = "Discard", CloseButtonText = "Cancel", DefaultButton = ContentDialogButton.Close };
            var result = await ShowDialog(dialog);
            if (result == ContentDialogResult.Primary) await ApplyLighting();
            if (result == ContentDialogResult.None || (result == ContentDialogResult.Primary && dirty))
            {
                changingZone = true; Zones.SelectedItem = Zones.Items.Cast<ComboBoxItem>().First(z => (int)z.Tag == selectedZone); changingZone = false; return;
            }
        }
        selectedZone = next;
        try { await ReadLighting(); preferences.Save(new(DesktopIntegration.StartupEnabled(), selectedZone)); }
        catch (Exception e) { lighting = null; ShowError(e); }
        UpdateEnabled();
    }
    private async Task PickColor()
    {
        var picker = new ColorPicker { Color = ColorHelper.FromArgb(255, (byte)rgb[0], (byte)rgb[1], (byte)rgb[2]), IsAlphaEnabled = false, IsMoreButtonVisible = false, IsColorSliderVisible = true, IsColorChannelTextInputVisible = true, IsHexInputVisible = true, MaxWidth = 320 };
        var dialog = new ContentDialog { XamlRoot = Root.XamlRoot, Title = "Light color", Content = picker, PrimaryButtonText = "Use color", CloseButtonText = "Cancel", DefaultButton = ContentDialogButton.Primary };
        if (await ShowDialog(dialog) == ContentDialogResult.Primary)
        {
            rgb = [picker.Color.R, picker.Color.G, picker.Color.B]; Swatch.Background = new SolidColorBrush(picker.Color); dirty = true; UpdateEnabled();
        }
    }
    private async Task ApplyLighting()
    {
        if (lighting is null || !dirty) return;
        var original = lighting;
        var changedRgb = Enumerable.Range(0, 3).Any(i => rgb[i] != original.Value(i + 3));
        var patch = new { expected = original, brightness = (int)Brightness.Value == original.Value(0) ? (int?)null : (int)Brightness.Value, rgb = changedRgb ? rgb : null };
        var status = await Mutate(Wire.Request("Lighting", new { patch }));
        if (status?.Outcome == "Verified") { dirty = false; await SafeRefresh(); }
    }
    private async Task<OperationStatus?> Mutate(JsonNode mutation)
    {
        if (busy || polling) return null;
        busy = true; UpdateEnabled();
        try
        {
            var result = await session.ApplyAsync(mutation);
            await Refresh();
            if (mutation["Lighting"] is not null && result.Outcome != "Verified" && lighting is not null)
            {
                lighting = await session.ReadAsync<LightingState>("GetAwakeLighting", "AwakeLighting", new { zone_id = selectedZone });
                LightDetail.Text = $"Readback: role {lighting.ActiveRole}, brightness {lighting.Value(0)?.ToString() ?? "unknown"}%, RGB {lighting.Value(3)}/{lighting.Value(4)}/{lighting.Value(5)}. Draft retained.";
            }
            SetNotice(result.Message, result.Outcome == "Verified" ? InfoBarSeverity.Success : InfoBarSeverity.Warning);
            return result;
        }
        catch (Exception e) { ShowError(e); return null; }
        finally { busy = false; UpdateEnabled(); }
    }
    private static void EnableChildren(Panel panel, bool enabled)
    {
        foreach (var child in panel.Children)
        {
            if (child is Control control) control.IsEnabled = enabled;
            if (child is Panel nested) EnableChildren(nested, enabled);
        }
    }
    private void UpdateEnabled()
    {
        var ready = connected && session.CanWrite && !busy && !polling;
        EnableChildren(PowerButtons, ready && session.State?.PowerUnavailable is null);
        EnableChildren(FanButtons, ready && session.State?.FanUnavailable is null);
        EnableChildren(LightEditor, ready && lighting?.Editable == true);
        ApplyButton.IsEnabled = ready && lighting?.Editable == true && dirty;
        DiscardButton.IsEnabled = dirty && !busy;
        Zones.IsEnabled = !busy && !polling && session.State is not null;
        ReconnectButton.IsEnabled = !busy && !polling;
        DraftLabel.Text = dirty ? "Unsaved changes" : "";
    }
    private async Task ShowSettings()
    {
        if (Root.XamlRoot is null || modalOpen) return;
        var startup = new CheckBox { Content = "Launch to tray when I sign in", IsChecked = DesktopIntegration.StartupEnabled(), IsEnabled = !demo };
        var content = new StackPanel { Spacing = 12 };
        content.Children.Add(startup);
        content.Children.Add(new TextBlock { Text = "Closing the window keeps NUC Control in the tray. Startup never applies hardware settings.", TextWrapping = TextWrapping.Wrap });
        content.Children.Add(new TextBlock { Text = $"Backend {session.Info?.Version ?? "unavailable"} · Protocol {session.Info?.ProtocolRevision.ToString() ?? "—"}\n{session.State?.Bios}", TextWrapping = TextWrapping.Wrap });
        var dialog = new ContentDialog { XamlRoot = Root.XamlRoot, Title = "Settings", Content = content, PrimaryButtonText = "Save", CloseButtonText = "Cancel" };
        if (await ShowDialog(dialog) == ContentDialogResult.Primary && !demo)
        {
            try { DesktopIntegration.SetStartup(startup.IsChecked == true); preferences.Save(new(startup.IsChecked == true, selectedZone)); }
            catch (Exception e) { ShowError(e); }
        }
    }
    private async Task<ContentDialogResult> ShowDialog(ContentDialog dialog)
    {
        if (modalOpen || Root.XamlRoot is null) return ContentDialogResult.None;
        modalOpen = true;
        try { return await dialog.ShowAsync(); }
        finally { modalOpen = false; }
    }
}
