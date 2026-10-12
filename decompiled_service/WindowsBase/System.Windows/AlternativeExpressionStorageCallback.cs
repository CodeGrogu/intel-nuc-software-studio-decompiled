using MS.Internal.WindowsBase;

namespace System.Windows;

[FriendAccessAllowed]
internal delegate Expression AlternativeExpressionStorageCallback(DependencyObject d, DependencyProperty dp, PropertyMetadata metadata);
