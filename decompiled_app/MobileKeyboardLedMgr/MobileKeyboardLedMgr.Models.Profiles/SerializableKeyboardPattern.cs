using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models.Profiles;

public class SerializableKeyboardPattern
{
	public string Id { get; set; }

	public string Name { get; set; }

	public List<KeyColorData> Keys { get; set; }

	public List<RGBKeyboardDirection> Directions { get; set; }

	public RGBKeyboardDirection SelectedDirection { get; set; }

	public RGBKeyboardEffect SelectedEffect { get; set; }

	public List<KeyValuePair<int, Color>> AvailableColors { get; set; }

	public bool Preset { get; set; }

	public int ACBrightness { get; set; }

	public int DCBrightness { get; set; }

	public int Speed { get; set; }

	public bool FrequencySupported { get; set; }

	public bool EditingEnabled { get; set; }

	public bool SpeedEnabled { get; set; }

	public List<SerializableColorPickerColors> SelectedColors { get; set; }

	public int TotalColorCount { get; set; }

	public bool KeyGroupEnabled { get; set; }

	public SerializableKeyboardPattern(KeyboardPattern pattern)
	{
		if (pattern == null)
		{
			return;
		}
		Id = pattern.Id;
		Name = pattern.Name;
		Keys = pattern.Keys.ToList();
		Directions = pattern.Directions.ToList();
		SelectedDirection = pattern.SelectedDirection;
		SelectedEffect = pattern.SelectedEffect;
		AvailableColors = pattern.AvailableColors.ToList();
		Preset = pattern.Preset;
		ACBrightness = pattern.ACBrightness;
		DCBrightness = pattern.DCBrightness;
		Speed = pattern.Speed;
		FrequencySupported = pattern.FrequencySupported;
		EditingEnabled = pattern.EditingEnabled;
		SpeedEnabled = pattern.SpeedEnabled;
		SelectedColors = new List<SerializableColorPickerColors>();
		foreach (ColorPickerColors selectedColor in pattern.SelectedColors)
		{
			SelectedColors.Add(new SerializableColorPickerColors(selectedColor));
		}
		TotalColorCount = pattern.TotalColorCount;
		KeyGroupEnabled = pattern.KeyGroupEnabled;
	}

	public override bool Equals(object obj)
	{
		if (obj is SerializableKeyboardPattern serializableKeyboardPattern && Id == serializableKeyboardPattern.Id && Name == serializableKeyboardPattern.Name && SelectedDirection == serializableKeyboardPattern.SelectedDirection && SelectedEffect == serializableKeyboardPattern.SelectedEffect && Preset == serializableKeyboardPattern.Preset && ACBrightness == serializableKeyboardPattern.ACBrightness && DCBrightness == serializableKeyboardPattern.DCBrightness && Speed == serializableKeyboardPattern.Speed && FrequencySupported == serializableKeyboardPattern.FrequencySupported && EditingEnabled == serializableKeyboardPattern.EditingEnabled && SpeedEnabled == serializableKeyboardPattern.SpeedEnabled && TotalColorCount == serializableKeyboardPattern.TotalColorCount && KeyGroupEnabled == serializableKeyboardPattern.KeyGroupEnabled && EqualityComparer<List<KeyColorData>>.Default.Equals(Keys, serializableKeyboardPattern.Keys) && EqualityComparer<List<RGBKeyboardDirection>>.Default.Equals(Directions, serializableKeyboardPattern.Directions) && EqualityComparer<List<KeyValuePair<int, Color>>>.Default.Equals(AvailableColors, serializableKeyboardPattern.AvailableColors))
		{
			return EqualityComparer<List<SerializableColorPickerColors>>.Default.Equals(SelectedColors, serializableKeyboardPattern.SelectedColors);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ((((((((((((((((unchecked(2144091145 * -1521134295) + Id.GetHashCode()) * -1521134295 + Name.GetHashCode()) * -1521134295 + SelectedDirection.GetHashCode()) * -1521134295 + SelectedEffect.GetHashCode()) * -1521134295 + Preset.GetHashCode()) * -1521134295 + ACBrightness.GetHashCode()) * -1521134295 + DCBrightness.GetHashCode()) * -1521134295 + Speed.GetHashCode()) * -1521134295 + FrequencySupported.GetHashCode()) * -1521134295 + EditingEnabled.GetHashCode()) * -1521134295 + SpeedEnabled.GetHashCode()) * -1521134295 + TotalColorCount.GetHashCode()) * -1521134295 + KeyGroupEnabled.GetHashCode()) * -1521134295 + EqualityComparer<List<KeyColorData>>.Default.GetHashCode(Keys).GetHashCode()) * -1521134295 + EqualityComparer<List<KeyValuePair<int, Color>>>.Default.GetHashCode(AvailableColors).GetHashCode()) * -1521134295 + EqualityComparer<List<SerializableColorPickerColors>>.Default.GetHashCode(SelectedColors).GetHashCode()) * -1521134295 + EqualityComparer<List<RGBKeyboardDirection>>.Default.GetHashCode(Directions).GetHashCode();
	}
}
