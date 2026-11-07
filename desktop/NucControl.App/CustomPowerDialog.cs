using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NucControl.Core;
using System.Text.Json.Nodes;

namespace NucControl.App;
public sealed class CustomPowerDialog : ContentDialog
{
    private readonly CustomPowerBounds bounds;
    private readonly PowerReadback expected;
    private readonly NumberBox pl1;
    private readonly NumberBox pl2;
    private readonly ComboBox tau;
    public PowerLimits? Limits { get; private set; }
    public JsonNode? Mutation { get; private set; }
    public CustomPowerDialog(PanelState state, PowerLimits? retained)
    {
        bounds = state.CustomPowerBounds ?? throw new BackendException("Custom power unavailable");
        expected = state.PowerReadback ?? throw new BackendException("Power readback unavailable");
        var current = retained ?? state.PowerLimits;
        Title = "Custom power limits"; PrimaryButtonText = "Apply"; CloseButtonText = "Cancel";
        var content = new StackPanel { Spacing = 12, MinWidth = 300 };
        content.Children.Add(new TextBlock { Text = $"Current: {expected.Display}", TextWrapping = TextWrapping.Wrap });
        pl1 = new NumberBox { Header = $"Sustained power ({bounds.MinimumPl1Watts}-{bounds.MaximumPl1Watts} W)", Value = current?.Pl1Watts ?? double.NaN, SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline };
        pl2 = new NumberBox { Header = $"Turbo power ({bounds.MinimumPl2Watts}-{bounds.MaximumPl2Watts} W)", Value = current?.Pl2Watts ?? double.NaN, SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline };
        tau = new ComboBox { Header = "Time window (seconds)", ItemsSource = bounds.TauSeconds, SelectedItem = current?.TauSeconds, HorizontalAlignment = HorizontalAlignment.Stretch };
        content.Children.Add(pl1); content.Children.Add(pl2); content.Children.Add(tau);
        content.Children.Add(new TextBlock { Text = "Choose whole watts within the firmware's maximum-performance limits. Turbo power must be at least sustained power. No setting is applied until Apply.", TextWrapping = TextWrapping.Wrap });
        Content = content;
        pl1.ValueChanged += (_, _) => Validate(); pl2.ValueChanged += (_, _) => Validate(); tau.SelectionChanged += (_, _) => Validate();
        PrimaryButtonClick += (_, args) => { Validate(); if (Limits is null) { args.Cancel = true; return; } Mutation = bounds.CreateMutation(expected, Limits); };
        Validate();
    }
    private void Validate()
    {
        Limits = null;
        if (double.IsFinite(pl1.Value) && pl1.Value == Math.Truncate(pl1.Value) && pl1.Value >= 0 && pl1.Value <= 255
            && double.IsFinite(pl2.Value) && pl2.Value == Math.Truncate(pl2.Value) && pl2.Value >= 0 && pl2.Value <= 255 && tau.SelectedItem is int seconds)
        {
            var proposed = new PowerLimits((int)pl1.Value, (int)pl2.Value, seconds);
            if (bounds.Allows(proposed)) Limits = proposed;
        }
        IsPrimaryButtonEnabled = Limits is not null;
    }
}
