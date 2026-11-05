using System.Text.Json;

namespace NucControl.Core;

public sealed class PreferenceStore(string directory)
{
    public Preferences Load()
    {
        try { return JsonSerializer.Deserialize<Preferences>(File.ReadAllText(Path.Combine(directory, "preferences.json")), Wire.Json) ?? new(); }
        catch (Exception e) when (e is IOException or JsonException or UnauthorizedAccessException) { return new(); }
    }
    public void Save(Preferences preferences)
    {
        Directory.CreateDirectory(directory);
        var file = Path.Combine(directory, "preferences.json");
        File.WriteAllText(file + ".tmp", JsonSerializer.Serialize(preferences, Wire.Json));
        File.Move(file + ".tmp", file, true);
    }
}
