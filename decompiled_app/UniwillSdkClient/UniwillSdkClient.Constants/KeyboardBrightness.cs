using System.Collections.Generic;

namespace UniwillSdkClient.Constants;

public static class KeyboardBrightness
{
	public const int OFF = 0;

	public const int HALF = 50;

	public const int FULL = 100;

	public static readonly List<int> FUNCTION_KEY_VALUES = new List<int> { 0, 50, 100 };
}
