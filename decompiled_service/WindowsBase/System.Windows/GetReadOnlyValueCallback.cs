using MS.Internal.WindowsBase;

namespace System.Windows;

[FriendAccessAllowed]
internal delegate object GetReadOnlyValueCallback(DependencyObject d, out BaseValueSourceInternal source);
