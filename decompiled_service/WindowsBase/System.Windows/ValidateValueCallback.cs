namespace System.Windows;

/// <summary>Represents a method used as a callback that validates the effective value of a  dependency property.</summary>
/// <param name="value">The value to be validated.</param>
/// <returns>
///     <see langword="true" /> if the value was validated; <see langword="false" /> if the submitted value was invalid.</returns>
public delegate bool ValidateValueCallback(object value);
