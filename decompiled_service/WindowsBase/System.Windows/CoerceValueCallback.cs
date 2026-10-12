namespace System.Windows;

/// <summary>Provides a template for a method that is called whenever a dependency property value is being re-evaluated, or coercion is specifically requested.</summary>
/// <param name="d">The object that the property exists on. When the callback is invoked, the property system will pass this value.</param>
/// <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
/// <returns>The coerced value (with appropriate type). </returns>
public delegate object CoerceValueCallback(DependencyObject d, object baseValue);
