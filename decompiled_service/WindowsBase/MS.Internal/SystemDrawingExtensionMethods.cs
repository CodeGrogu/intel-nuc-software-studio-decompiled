using System;
using System.IO;
using System.Security;

namespace MS.Internal;

internal abstract class SystemDrawingExtensionMethods
{
	internal abstract bool IsBitmap(object data);

	internal abstract bool IsImage(object data);

	internal abstract bool IsMetafile(object data);

	[SecurityCritical]
	internal abstract IntPtr GetHandleFromMetafile(object data);

	internal abstract object GetMetafileFromHemf(IntPtr hMetafile);

	internal abstract object GetBitmap(object data);

	[SecurityCritical]
	internal abstract IntPtr GetHBitmap(object data, out int width, out int height);

	[SecurityCritical]
	internal abstract IntPtr GetHBitmapFromBitmap(object data);

	[SecurityCritical]
	internal abstract IntPtr ConvertMetafileToHBitmap(IntPtr handle);

	internal abstract Stream GetCommentFromGifStream(Stream stream);

	internal abstract CodeAccessPermission NewSafePrintingPermission();

	internal abstract CodeAccessPermission NewDefaultPrintingPermission();

	internal abstract void SaveMetafileToImageStream(MemoryStream metafileStream, Stream imageStream);

	[SecurityCritical]
	internal abstract object GetBitmapFromBitmapSource(object source);

	protected SystemDrawingExtensionMethods()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
