namespace System.IO;

[Flags]
internal enum HANDLE_SHARING_OPTIONS : uint
{
	HSO_SHARE_NONE = 0u,
	HSO_SHARE_READ = 1u,
	HSO_SHARE_WRITE = 2u,
	HSO_SHARE_DELETE = 4u
}
