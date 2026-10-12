using System.Globalization;
using System.Windows.Controls;

namespace MobileDisplayModeMgr.ValidationRules;

public class IntValidationRule : ValidationRule
{
	public int Min { get; set; }

	public int Max { get; set; }

	public int Interval { get; set; }

	public override ValidationResult Validate(object value, CultureInfo cultureInfo)
	{
		int? num;
		try
		{
			num = int.Parse((string)value);
		}
		catch
		{
			return new ValidationResult(isValid: false, "Invalid Input");
		}
		if (num.HasValue && num >= Min && num <= Max)
		{
			if (Interval == 0 || num % Interval == 0)
			{
				return ValidationResult.ValidResult;
			}
			return new ValidationResult(isValid: false, "Invalid Input: input must be an interval of " + Interval);
		}
		return new ValidationResult(isValid: false, "Invalid Input");
	}
}
