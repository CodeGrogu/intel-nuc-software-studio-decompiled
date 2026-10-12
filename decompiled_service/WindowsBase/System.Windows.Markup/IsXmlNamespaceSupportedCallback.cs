using MS.Internal.WindowsBase;

namespace System.Windows.Markup;

[FriendAccessAllowed]
internal delegate bool IsXmlNamespaceSupportedCallback(string xmlNamespace, out string newXmlNamespace);
