using System.Windows;
using MS.Internal.WindowsBase;

namespace MS.Internal;

[FriendAccessAllowed]
internal abstract class DefaultValueFactory
{
	internal abstract object DefaultValue { get; }

	internal abstract object CreateDefaultValue(DependencyObject owner, DependencyProperty property);

	protected DefaultValueFactory()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
