using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using CustomizationMgr.Enums;
using CustomizationMgr.Models;

namespace CustomizationMgr.Interfaces;

public interface ICustomResourceManager
{
	CustomAppSettings CustomAppSettings { get; }

	KeyValuePair<AppLogoType, BitmapSource> AppLogo();

	BitmapSource GetCustomIcon(IconType type);

	bool IsFirstLaunch();

	void LoadComponentStyleDictionary(string SourceUriString);

	void LoadComponentStrings(string AssemblyName);

	void LoadCustomResources();

	void AddKnownCustomSettingType(Type type, object defaultSetting);

	void AddKnownCustomSettingTypes(List<Type> types);

	void GenerateDefaultFile();

	string GetCustomDefaultCultureId();
}
