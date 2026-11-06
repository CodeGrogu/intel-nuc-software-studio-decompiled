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
    private readonly PreferenceStore preferences;
    private readonly LightingPresetStore lightingPresets;
    private string themePreference = "System";
    private readonly DispatcherTimer timer = new() { Interval = TimeSpan.FromSeconds(2) };
    private readonly TaskbarIcon tray;
    private readonly DesktopIntegration.Subclass powerCallback;
    private readonly LightingDraft draft;
    private readonly Dictionary<int, AdvancedLightingDraft> advancedDrafts = [];
    private PowerLimits? customLimitsDraft;
    private bool loading, busy, polling, changingZone, exiting, connected, modalOpen;
    private int selectedZone => draft.SelectedZone;
    private int ticks;
    private bool visible = true;
    private static readonly (int Id, string Name)[] ZoneNames = [(0, "Power button"), (2, "Skull"), (4, "Front left"), (5, "Front middle"), (6, "Front right")];

    public MainWindow(IBackend backend, bool demo)
    {
        StartupLog.Write("Window XAML loading");
        InitializeComponent();
        StartupLog.Write("Window XAML loaded");
        this.backend = backend; this.demo = demo; session = new(backend);
        preferences = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NucControl", demo ? "Demo" : ""));
        lightingPresets = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NucControl", demo ? "Demo" : ""));
        var saved = preferences.Load();
        draft = new(saved.SelectedZone);
        themePreference = saved.Theme is "Light" or "Dark" ? saved.Theme : "System";
        Scroller.RequestedTheme = themePreference switch { "Light" => ElementTheme.Light, "Dark" => ElementTheme.Dark, _ => ElementTheme.Default };
        var scale = DesktopIntegration.GetDpiForWindow(WinRT.Interop.WindowNative.GetWindowHandle(this)) / 96.0;
        AppWindow.Resize(new SizeInt32((int)(480 * scale), (int)(680 * scale)));
        StartupLog.Write("Window icon loading");
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
        StartupLog.Write("Tray icon creating");
        tray.ForceCreate(enablesEfficiencyMode: false);
        StartupLog.Write("Tray icon created");
        SettingsButton.Click += async (_, _) => await ShowSettings();
        ReconnectButton.Click += async (_, _) => await SafeRefresh();
        CustomPowerButton.Click += async (_, _) => await ShowCustomPower();
        Fixed70Button.Click += async (_, _) => await Mutate(Wire.Request("FixedFan70", new { }));
        AdvancedLightingButton.Click += async (_, _) => await ShowAdvancedLighting();
        LightingPresetsButton.Click += async (_, _) => await ShowLightingPresets();
        ApplyButton.Click += async (_, _) => await ApplyLighting();
        DiscardButton.Click += (_, _) => { draft.Discard(); RenderDraft(); };
        ColorButton.Click += async (_, _) => await PickColor();
        Brightness.ValueChanged += (_, _) => { if (!loading) { draft.SetBrightness((int)Brightness.Value); RenderDraft(); } };
        Zones.SelectionChanged += async (_, _) => await ChangeZone();
        timer.Tick += async (_, _) =>
        {
            if (!visible || busy || polling || modalOpen) return;
            polling = true; UpdateEnabled();
            try { if (session.PendingId is not null || ++ticks % 5 == 0) await Refresh(); else await RefreshTelemetry(); }
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
        if (busy || session.PendingId is not null) { Show(); SetNotice("Refresh to reconcile the current operation before exiting.", InfoBarSeverity.Warning); return; }
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
        polling = true; UpdateEnabled();
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
        if (state is null) { PowerDetail.Text = session.Message; FanDetail.Text = "Controls unavailable"; return; }
        var kinds = state.PowerPresets.Select(p => p.PresetKind).ToArray();
        if (!PowerButtons.Children.OfType<ToggleButton>().Select(b => (string)b.Tag).SequenceEqual(kinds)) PowerButtons.Children.Clear();
        for (var i = 0; i < state.PowerPresets.Length; i++)
        {
            var preset = state.PowerPresets[i];
            var current = state.MatchesPowerPreset(preset);
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
        PowerDetail.Text = state.PowerUnavailable ?? state.PowerReadback?.Display ?? "Power readback unavailable";
        if (state.PowerReadback is not null && !state.PowerPresets.Any(p => state.MatchesPowerPreset(p))) PowerDetail.Text += " \u00b7 Another setting";
        Fixed70Button.IsChecked = state.FixedFan70;
        FanDetail.Text = state.FanUnavailable ?? (state.FixedFan70 ? "Reviewed fixed 70% command active; both RPM are monitored" : "Stock firmware preset · both physical fans");
        changingZone = true;
        var zones = ZoneNames.Where(z => (state.LedZoneMask & (1u << z.Id)) != 0).ToArray();
        if (!Zones.Items.Cast<ComboBoxItem>().Select(z => (int)z.Tag).SequenceEqual(zones.Select(z => z.Id)))
        {
            Zones.Items.Clear();
            foreach (var zone in zones) Zones.Items.Add(new ComboBoxItem { Content = zone.Name, Tag = zone.Id });
        }
        Zones.SelectedItem = Zones.Items.Cast<ComboBoxItem>().FirstOrDefault(z => (int)z.Tag == selectedZone) ?? (draft.IsDirty ? null : Zones.Items.FirstOrDefault());
        changingZone = false;
        if (Zones.SelectedItem is ComboBoxItem selected) await ReadLighting((int)selected.Tag, draft.IsDirty);
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
    private async Task ReadLighting(int zone, bool retainDraft = false)
    {
        var read = draft.LoadAsync(zone, id => session.ReadAsync<LightingState>("GetAwakeLighting", "AwakeLighting", new { zone_id = id }), retainDraft);
        RenderDraft(); // LoadAsync invalidates the previous snapshot before its first await.
        try { if (await read) RenderDraft(); }
        catch (Exception e) { RenderDraft(); LightDetail.Text = $"This light is unavailable: {e.Message}"; }
    }
    private void RenderDraft()
    {
        loading = true;
        BrightnessLabel.Text = draft.BrightnessLabel;
        Brightness.Visibility = draft.Brightness.HasValue ? Visibility.Visible : Visibility.Collapsed;
        if (draft.Brightness is int brightness) Brightness.Value = brightness;
        Swatch.Visibility = draft.Color is null ? Visibility.Collapsed : Visibility.Visible;
        ColorLabel.Text = draft.Color is null ? "Color unavailable" : "Color";
        if (draft.Color is { } color) Swatch.Background = new SolidColorBrush(ColorHelper.FromArgb(255, (byte)color.R, (byte)color.G, (byte)color.B));
        if (draft.IsLoading) LightDetail.Text = "Reading selected light...";
        else if (draft.Snapshot is { } state)
        {
            var role = state.ActiveRole switch { 0 => "Power state", 1 => "Disk activity", 2 => "Ethernet activity", 3 => "Wi-Fi activity", 4 => "Software", 5 => "Power limit", 6 => "Disabled", _ => "Unknown" };
            LightDetail.Text = state.Unavailable ?? (state.ActiveRole == 0 ? "Awake color and brightness" : $"Current function: {role}. Apply replaces it with manual power-state lighting, including its stored sleep behavior.");
        }
        else LightDetail.Text = "Lighting readback unavailable";
        loading = false; UpdateEnabled();
    }
    private async Task ChangeZone()
    {
        if (changingZone || Zones.SelectedItem is not ComboBoxItem selected) return;
        var next = (int)selected.Tag;
        if (next == selectedZone) return;
        if (draft.IsDirty)
        {
            var dialog = new ContentDialog { XamlRoot = Root.XamlRoot, Title = "Unsaved lighting", Content = "Apply this light's changes before choosing another?", PrimaryButtonText = "Apply", SecondaryButtonText = "Discard", CloseButtonText = "Cancel", DefaultButton = ContentDialogButton.Close };
            var result = await ShowDialog(dialog);
            if (result == ContentDialogResult.Primary) await ApplyLighting();
            if (result == ContentDialogResult.None || (result == ContentDialogResult.Primary && draft.IsDirty))
            {
                changingZone = true; Zones.SelectedItem = Zones.Items.Cast<ComboBoxItem>().FirstOrDefault(z => (int)z.Tag == selectedZone); changingZone = false; return;
            }
        }
        changingZone = true; Zones.SelectedItem = Zones.Items.Cast<ComboBoxItem>().FirstOrDefault(z => (int)z.Tag == next); changingZone = false;
        await ReadLighting(next);
        preferences.Save(new(DesktopIntegration.StartupEnabled(), selectedZone, themePreference));
        UpdateEnabled();
    }
    private async Task PickColor()
    {
        if (busy || polling || !draft.CanEdit || draft.Color is not { } color) return;
        var original = draft.Snapshot;
        var picker = new ColorPicker { Color = ColorHelper.FromArgb(255, (byte)color.R, (byte)color.G, (byte)color.B), IsAlphaEnabled = false, IsMoreButtonVisible = false, IsColorSliderVisible = true, IsColorChannelTextInputVisible = true, IsHexInputVisible = true, MaxWidth = 320 };
        var dialog = new ContentDialog { XamlRoot = Root.XamlRoot, Title = "Light color", Content = picker, PrimaryButtonText = "Use color", CloseButtonText = "Cancel", DefaultButton = ContentDialogButton.Primary };
        if (await ShowDialog(dialog) == ContentDialogResult.Primary && ReferenceEquals(original, draft.Snapshot) && draft.CanEdit)
        {
            draft.SetColor(new(picker.Color.R, picker.Color.G, picker.Color.B)); RenderDraft();
        }
    }
    private async Task ApplyLighting()
    {
        if (!draft.CanEdit || !draft.IsDirty) return;
        await Mutate(draft.CreateMutation());
    }

    private async Task<OperationStatus?> Mutate(JsonNode mutation)
    {
        if (busy || polling) return null;
        busy = true; UpdateEnabled();
        try
        {
            var result = await session.ApplyAsync(mutation);
            if (mutation["CustomPower"] is not null && result.Outcome == "Verified") customLimitsDraft = null;
            if (mutation["AdvancedLighting"] is not null && result.Outcome == "Verified") advancedDrafts.Remove(selectedZone);
            if ((mutation["Lighting"] is not null || mutation["AdvancedLighting"] is not null || mutation["LightingPreset"] is not null) && result.Outcome == "Verified") draft.Clear();
            await Refresh();
            SetNotice(result.Message + (mutation["AdvancedLighting"] is not null && result.Outcome != "Verified" ? " Advanced draft retained; reopen Advanced lighting to compare fresh readback." : ""), result.Outcome == "Verified" ? InfoBarSeverity.Success : InfoBarSeverity.Warning);
            return result;
        }
        catch (Exception e) { ShowError(e); if (mutation["AdvancedLighting"] is not null) SetNotice(Notice.Message + " Advanced draft retained for review after reconciliation.", InfoBarSeverity.Warning); return null; }
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
        var ready = connected && session.CanWrite && !busy && !polling && !draft.IsLoading;
        EnableChildren(PowerButtons, ready && session.State?.PowerUnavailable is null);
        EnableChildren(FanButtons, ready && session.State?.FanUnavailable is null);
        EnableChildren(LightEditor, ready && draft.CanEdit && Zones.SelectedItem is not null);
        CustomPowerButton.IsEnabled = ready && session.State?.CustomPowerBounds is not null && session.State.PowerReadback?.RawRegister is not null;
        Fixed70Button.IsEnabled = ready && session.State?.FixedFan70Available == true;
        AdvancedLightingButton.IsEnabled = ready && session.State?.AdvancedLightingAvailable == true && Zones.SelectedItem is not null && !draft.IsDirty;
        LightingPresetsButton.IsEnabled = ready && session.State?.AdvancedLightingAvailable == true && !draft.IsDirty && advancedDrafts.Count == 0;
        ApplyButton.IsEnabled = ready && draft.CanEdit && Zones.SelectedItem is not null && draft.IsDirty;
        DiscardButton.IsEnabled = draft.IsDirty && !busy && !polling && !draft.IsLoading;
        Zones.IsEnabled = !busy && !polling && !draft.IsLoading && session.PendingId is null && session.State is not null;
        ReconnectButton.IsEnabled = !busy && !polling && !draft.IsLoading;
        DraftLabel.Text = draft.IsDirty ? "Unsaved changes" : "";
    }
    private async Task ShowAdvancedLighting()
    {
        if (busy || polling || draft.IsDirty || !session.CanWrite || Zones.SelectedItem is null) return;
        JsonNode? mutation = null;
        busy = true; UpdateEnabled();
        try
        {
            var retained = advancedDrafts.TryGetValue(selectedZone, out var advanced);
            advanced ??= new();
            var dialog = new AdvancedLightingDialog(session, selectedZone, advanced) { XamlRoot = Root.XamlRoot };
            await dialog.InitializeAsync();
            if (await ShowDialog(dialog) == ContentDialogResult.Primary) { mutation = dialog.Mutation; advancedDrafts[selectedZone] = advanced; }
            else if (retained && !advanced.IsDirty && !advanced.CanApply) advancedDrafts.Remove(selectedZone);
        }
        catch (Exception e) { ShowError(e); }
        finally { busy = false; UpdateEnabled(); }
        if (mutation is not null) await Mutate(mutation);
    }
    private async Task ShowLightingPresets()
    {
        if (busy || polling || !session.CanWrite || draft.IsDirty || advancedDrafts.Count != 0) return;
        JsonNode? mutation = null;
        busy = true; UpdateEnabled();
        try
        {
            var dialog = new LightingPresetsDialog(session, lightingPresets) { XamlRoot = Root.XamlRoot };
            if (await ShowDialog(dialog) == ContentDialogResult.Primary && dialog.Selected is { } preset)
            {
                mutation = await preset.CreateMutationAsync(session.State!, (z,m) => session.ReadAsync<LightingModeState>("GetLightingMode", "LightingMode", new { zone_id = z, indicator_mode = m }));
                var preview = new ContentDialog { XamlRoot = Root.XamlRoot, Title = $"Apply {preset.Name}?", PrimaryButtonText = "Apply", CloseButtonText = "Cancel", DefaultButton = ContentDialogButton.Close,
                    Content = new ScrollViewer { MaxHeight = 400, Content = new TextBlock { Text = preset.Summary + "\n\nAll zones are checked before writing. A partial failure stops the batch without rollback. The saved preset remains available for review; it is never retried automatically.", TextWrapping = TextWrapping.Wrap, MaxWidth = 340 } } };
                if (await ShowDialog(preview) != ContentDialogResult.Primary) mutation = null;
            }
        }
        catch (Exception e) { ShowError(e); }
        finally { busy = false; UpdateEnabled(); }
        if (mutation is not null)
        {
            var result = await Mutate(mutation);
            if (result?.Outcome != "Verified") SetNotice(Notice.Message + " Saved preset retained. After reconciliation, review actual values in Advanced lighting before applying again.", InfoBarSeverity.Warning);
        }
    }
    private async Task ShowCustomPower()
    {
        if (busy || polling || !session.CanWrite) return;
        JsonNode? mutation = null; busy = true; UpdateEnabled();
        try
        {
            await session.RefreshAsync();
            var dialog = new CustomPowerDialog(session.State ?? throw new BackendException("Power state unavailable"), customLimitsDraft) { XamlRoot = Root.XamlRoot };
            if (await ShowDialog(dialog) == ContentDialogResult.Primary) { mutation = dialog.Mutation; customLimitsDraft = dialog.Limits; }
        }
        catch (Exception e) { ShowError(e); }
        finally { busy = false; UpdateEnabled(); }
        if (mutation is not null) await Mutate(mutation);
    }
    private async Task SaveLighting()
    {
        if (busy || polling || !session.CanWrite) return;
        if (draft.IsDirty || advancedDrafts.Values.Any(d => d.IsDirty || d.CanApply)) { SetNotice("Apply or discard lighting drafts before saving current firmware settings.", InfoBarSeverity.Warning); return; }
        JsonNode? mutation = null; busy = true; UpdateEnabled();
        try
        {
            var snapshot = await session.ReadAsync<LightingSaveState>("GetLightingSaveState", "LightingSaveState");
            var content = new StackPanel { Spacing = 12, MinWidth = 320 };
            content.Children.Add(new TextBlock { Text = "Request BIOS save for ALL five lights, including inactive functions and existing Ethernet settings shown below. This does not apply drafts. Firmware can acknowledge the request; persistence across restart cannot be independently read back.", TextWrapping = TextWrapping.Wrap });
            foreach (var zone in snapshot.Zones)
            {
                content.Children.Add(new Expander { Header = ZoneNames.First(z => z.Id == zone.ZoneId).Name, HorizontalAlignment = HorizontalAlignment.Stretch,
                    Content = new TextBlock { Text = snapshot.Summary(zone), TextWrapping = TextWrapping.Wrap, MaxWidth = 340 } });
            }
            var dialog = new ContentDialog { XamlRoot = Root.XamlRoot, Title = "Save lighting to BIOS?", Content = new ScrollViewer { Content = content, MaxHeight = 450 }, PrimaryButtonText = "Request save", CloseButtonText = "Cancel", DefaultButton = ContentDialogButton.Close };
            if (await ShowDialog(dialog) == ContentDialogResult.Primary) mutation = Wire.Request("SaveLighting", new { expected = snapshot });
        }
        catch (Exception e) { ShowError(e); }
        finally { busy = false; UpdateEnabled(); }
        if (mutation is not null) await Mutate(mutation);
    }
    private async Task ShowSettings()
    {
        if (Root.XamlRoot is null || modalOpen) return;
        var startup = new CheckBox { Content = "Launch to tray when I sign in", IsChecked = DesktopIntegration.StartupEnabled(), IsEnabled = !demo };
        var content = new StackPanel { Spacing = 12 };
        content.Children.Add(startup);
        var theme = new ComboBox { Header = "Theme", ItemsSource = new[] { "System", "Light", "Dark" }, SelectedItem = themePreference, HorizontalAlignment = HorizontalAlignment.Stretch };
        content.Children.Add(theme);
        var requestSave = false;
        var save = new Button { Content = "Save current lighting to BIOS...", IsEnabled = session.CanWrite && session.State?.AdvancedLightingAvailable == true && !busy && !polling };
        content.Children.Add(save);
        content.Children.Add(new Expander { Header = "Device support", Content = new TextBlock { Text = "This BIOS supports awake and Modern Standby lighting. Software brightness and front-right Software blue have firmware defects. Quiet, custom curves, other fixed duties and zero-RPM remain unvalidated. Wi-Fi lighting and CEC/display controls are unavailable; Ethernet editing is deferred.", TextWrapping = TextWrapping.Wrap, MaxWidth = 330 } });
        content.Children.Add(new TextBlock { Text = "Closing the window keeps NUC Control in the tray. Startup never applies hardware settings.", TextWrapping = TextWrapping.Wrap });
        content.Children.Add(new TextBlock { Text = $"Backend {session.Info?.Version ?? "unavailable"} · Protocol {session.Info?.ProtocolRevision.ToString() ?? "—"}\n{session.State?.Bios}", TextWrapping = TextWrapping.Wrap });
        var dialog = new ContentDialog { XamlRoot = Root.XamlRoot, Title = "Settings", Content = content, PrimaryButtonText = "Save", CloseButtonText = "Cancel" };
        save.Click += (_, _) => { requestSave = true; dialog.Hide(); };
        if (await ShowDialog(dialog) == ContentDialogResult.Primary)
        {
            try
            {
                if (!demo) DesktopIntegration.SetStartup(startup.IsChecked == true);
                themePreference = theme.SelectedItem as string ?? "System";
                Scroller.RequestedTheme = themePreference switch { "Light" => ElementTheme.Light, "Dark" => ElementTheme.Dark, _ => ElementTheme.Default };
                preferences.Save(new(startup.IsChecked == true, selectedZone, themePreference));
            }
            catch (Exception e) { ShowError(e); }
        }
        if (requestSave) await SaveLighting();
    }
    private async Task<ContentDialogResult> ShowDialog(ContentDialog dialog)
    {
        if (modalOpen || Root.XamlRoot is null) return ContentDialogResult.None;
        modalOpen = true;
        dialog.RequestedTheme = Scroller.RequestedTheme;
        try { return await dialog.ShowAsync(); }
        finally { modalOpen = false; }
    }
}
