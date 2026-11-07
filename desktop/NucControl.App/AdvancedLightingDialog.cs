using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NucControl.Core;
using System.Text.Json.Nodes;

namespace NucControl.App;

/// A read-before-edit dialog. The Core draft owns target identity and stale-read rejection.
public sealed class AdvancedLightingDialog : ContentDialog
{
    private readonly ControlSession session;
    private readonly int zone;
    private readonly AdvancedLightingDraft draft;
    private readonly ComboBox role = new() { Header = "Function", HorizontalAlignment = HorizontalAlignment.Stretch };
    private readonly ComboBox slot = new() { Header = "Power state", HorizontalAlignment = HorizontalAlignment.Stretch, ItemsSource = new[] { "Awake", "Modern Standby" }, SelectedIndex = 0 };
    private readonly CheckBox activate = new() { Content = "Use this function for this light", IsChecked = true };
    private readonly TextBlock notice = new() { TextWrapping = TextWrapping.Wrap };
    private readonly StackPanel fields = new() { Spacing = 8 };
    private readonly Button discard = new() { Content = "Reset draft", IsEnabled = false };
    private readonly HashSet<int> invalid = [];
    private bool initializing = true;
    public JsonNode? Mutation { get; private set; }

    public AdvancedLightingDialog(ControlSession session, int zone, AdvancedLightingDraft draft)
    {
        this.session = session; this.zone = zone; this.draft = draft;
        Title = "Advanced lighting"; PrimaryButtonText = "Apply"; CloseButtonText = "Cancel"; IsPrimaryButtonEnabled = false;
        foreach (var item in AdvancedLighting.Roles) role.Items.Add(new ComboBoxItem { Content = item.Name, Tag = item.Id });
        role.SelectedItem = role.Items.Cast<ComboBoxItem>().First(i => (int)i.Tag == (draft.SelectedMode >= 0 ? draft.SelectedMode : 0));
        activate.IsChecked = draft.Activate;
        var content = new StackPanel { Spacing = 12, MinWidth = 300 };
        content.Children.Add(role); content.Children.Add(slot); content.Children.Add(activate); content.Children.Add(notice);
        content.Children.Add(fields); content.Children.Add(discard);
        content.Children.Add(new TextBlock { Text = "Awake RGB is shared with this light's other functions. Changes stay within this selected light. Apply does not save to BIOS.", TextWrapping = TextWrapping.Wrap });
        Content = new ScrollViewer { Content = content, MaxHeight = 480, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
        role.SelectionChanged += async (_, _) => { if (!initializing) await Load(); };
        slot.SelectionChanged += (_, _) => { if (!initializing) RenderFields(); };
        activate.Checked += (_, _) => Update(); activate.Unchecked += (_, _) => Update();
        discard.Click += (_, _) => { draft.Discard(); activate.IsChecked = false; RenderFields(); };
        PrimaryButtonClick += (_, args) => { if (!draft.CanApply || invalid.Count != 0) { args.Cancel = true; return; } Mutation = draft.CreateMutation(); };
        initializing = false;
    }
    public Task InitializeAsync() => Load(retainChanges: true);
    private async Task Load(bool retainChanges = false)
    {
        if (role.SelectedItem is not ComboBoxItem selected) return;
        var mode = (int)selected.Tag;
        var pending = draft.LoadAsync(zone, mode, (id, indicator) => session.ReadAsync<LightingModeState>("GetLightingMode", "LightingMode", new { zone_id = id, indicator_mode = indicator }), retainChanges);
        fields.Children.Clear(); notice.Text = "Reading selected function..."; Update();
        try { if (await pending) RenderFields(); }
        catch (Exception e) { notice.Text = e.Message; Update(); }
    }
    private void RenderFields()
    {
        fields.Children.Clear();
        invalid.Clear();
        if (draft.Snapshot is not { } state) { Update(); return; }
        slot.Visibility = state.IndicatorMode == 0 ? Visibility.Visible : Visibility.Collapsed;
        notice.Text = state.Unavailable ?? $"Current function: {AdvancedLighting.Roles.FirstOrDefault(r => r.Id == state.ActiveRole).Name ?? "Other"}. {AdvancedLighting.Notice(state.IndicatorMode)}";
        foreach (var field in AdvancedLighting.Fields(state.IndicatorMode, slot.SelectedIndex == 1))
        {
            var value = draft.Value(field.Index);
            var enabled = AdvancedLighting.CanEdit(state, field);
            var label = value != state.Value(field.Index) ? $"{field.Label} (current: {state.Value(field.Index)?.ToString() ?? "unavailable"})" : field.Label;
            if (value is null)
            {
                fields.Children.Add(new TextBlock { Text = $"{field.Label}: unavailable", TextWrapping = TextWrapping.Wrap }); continue;
            }
            if (value < 0 || value > field.Maximum)
            {
                fields.Children.Add(new TextBlock { Text = $"{field.Label}: unrecognized stored value {value}", TextWrapping = TextWrapping.Wrap }); continue;
            }
            if (field.Choices is { } choices)
            {
                var input = new ComboBox { Header = label, ItemsSource = choices, SelectedIndex = value.Value < choices.Length ? value.Value : -1, IsEnabled = enabled, HorizontalAlignment = HorizontalAlignment.Stretch };
                input.SelectionChanged += (_, _) => { if (input.SelectedIndex >= 0) { draft.SetValue(field, input.SelectedIndex); Update(); } };
                fields.Children.Add(input);
            }
            else
            {
                var input = new NumberBox { Header = enabled ? label : $"{label} (read-only)", Value = value.Value, Minimum = 0, Maximum = field.Maximum, SmallChange = 1, SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline, IsEnabled = enabled };
                input.ValueChanged += (_, _) =>
                {
                    if (double.IsFinite(input.Value) && input.Value == Math.Truncate(input.Value) && input.Value >= 0 && input.Value <= field.Maximum)
                    { invalid.Remove(field.Index); draft.SetValue(field, (int)input.Value); }
                    else invalid.Add(field.Index);
                    Update();
                };
                fields.Children.Add(input);
            }
        }
        Update();
    }
    private void Update()
    {
        draft.Activate = activate.IsChecked == true;
        IsPrimaryButtonEnabled = draft.CanApply && invalid.Count == 0;
        role.IsEnabled = !draft.IsLoading && !draft.IsDirty;
        slot.IsEnabled = !draft.IsLoading;
        activate.IsEnabled = !draft.IsLoading && draft.Snapshot?.Unavailable is null;
        discard.IsEnabled = draft.IsDirty || draft.CanApply;
    }
}
