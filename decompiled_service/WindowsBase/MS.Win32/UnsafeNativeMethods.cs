using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using Accessibility;
using MS.Internal.Interop;
using MS.Internal.WindowsBase;
using Microsoft.Win32.SafeHandles;

namespace MS.Win32;

[FriendAccessAllowed]
internal sealed class UnsafeNativeMethods
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal class ShellExecuteInfo
	{
		public int cbSize;

		public ShellExecuteFlags fMask;

		public IntPtr hwnd;

		public string lpVerb;

		public string lpFile;

		public string lpParameters;

		public string lpDirectory;

		public int nShow;

		public IntPtr hInstApp;

		public IntPtr lpIDList;

		public string lpClass;

		public IntPtr hkeyClass;

		public int dwHotKey;

		public IntPtr hIcon;

		public IntPtr hProcess;

		public ShellExecuteInfo()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[Flags]
	internal enum ShellExecuteFlags
	{
		SEE_MASK_CLASSNAME = 1,
		SEE_MASK_CLASSKEY = 3,
		SEE_MASK_NOCLOSEPROCESS = 0x40,
		SEE_MASK_FLAG_DDEWAIT = 0x100,
		SEE_MASK_DOENVSUBST = 0x200,
		SEE_MASK_FLAG_NO_UI = 0x400,
		SEE_MASK_UNICODE = 0x4000,
		SEE_MASK_NO_CONSOLE = 0x8000,
		SEE_MASK_ASYNCOK = 0x100000,
		SEE_MASK_HMONITOR = 0x200000,
		SEE_MASK_NOZONECHECKS = 0x800000,
		SEE_MASK_NOQUERYCLASSSTORE = 0x1000000,
		SEE_MASK_WAITFORINPUTIDLE = 0x2000000
	}

	[SecurityCritical]
	public delegate bool EnumChildrenCallback(IntPtr hwnd, IntPtr lParam);

	public enum EXTENDED_NAME_FORMAT
	{
		NameUnknown = 0,
		NameFullyQualifiedDN = 1,
		NameSamCompatible = 2,
		NameDisplay = 3,
		NameUniqueId = 6,
		NameCanonical = 7,
		NameUserPrincipal = 8,
		NameCanonicalEx = 9,
		NameServicePrincipal = 10
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("00000122-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleDropTarget
	{
		[PreserveSig]
		int OleDragEnter([In][MarshalAs(UnmanagedType.Interface)] object pDataObj, [In][MarshalAs(UnmanagedType.U4)] int grfKeyState, [In][MarshalAs(UnmanagedType.U8)] long pt, [In][Out] ref int pdwEffect);

		[PreserveSig]
		int OleDragOver([In][MarshalAs(UnmanagedType.U4)] int grfKeyState, [In][MarshalAs(UnmanagedType.U8)] long pt, [In][Out] ref int pdwEffect);

		[PreserveSig]
		int OleDragLeave();

		[PreserveSig]
		int OleDrop([In][MarshalAs(UnmanagedType.Interface)] object pDataObj, [In][MarshalAs(UnmanagedType.U4)] int grfKeyState, [In][MarshalAs(UnmanagedType.U8)] long pt, [In][Out] ref int pdwEffect);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("00000121-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleDropSource
	{
		[PreserveSig]
		int OleQueryContinueDrag(int fEscapePressed, [In][MarshalAs(UnmanagedType.U4)] int grfKeyState);

		[PreserveSig]
		int OleGiveFeedback([In][MarshalAs(UnmanagedType.U4)] int dwEffect);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("B196B289-BAB4-101A-B69C-00AA00341D07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleControlSite
	{
		[PreserveSig]
		int OnControlInfoChanged();

		[PreserveSig]
		int LockInPlaceActive(int fLock);

		[PreserveSig]
		int GetExtendedControl([MarshalAs(UnmanagedType.IDispatch)] out object ppDisp);

		[PreserveSig]
		int TransformCoords([In][Out] NativeMethods.POINT pPtlHimetric, [In][Out] NativeMethods.POINTF pPtfContainer, [In][MarshalAs(UnmanagedType.U4)] int dwFlags);

		[PreserveSig]
		int TranslateAccelerator([In] ref MSG pMsg, [In][MarshalAs(UnmanagedType.U4)] int grfModifiers);

		[PreserveSig]
		int OnFocus(int fGotFocus);

		[PreserveSig]
		int ShowPropertyFrame();
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("00000118-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleClientSite
	{
		[PreserveSig]
		int SaveObject();

		[PreserveSig]
		int GetMoniker([In][MarshalAs(UnmanagedType.U4)] int dwAssign, [In][MarshalAs(UnmanagedType.U4)] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] out object moniker);

		[PreserveSig]
		int GetContainer(out IOleContainer container);

		[PreserveSig]
		int ShowObject();

		[PreserveSig]
		int OnShowWindow(int fShow);

		[PreserveSig]
		int RequestNewObjectLayout();
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("00000119-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleInPlaceSite
	{
		IntPtr GetWindow();

		[PreserveSig]
		int ContextSensitiveHelp(int fEnterMode);

		[PreserveSig]
		int CanInPlaceActivate();

		[PreserveSig]
		int OnInPlaceActivate();

		[PreserveSig]
		int OnUIActivate();

		[PreserveSig]
		int GetWindowContext([MarshalAs(UnmanagedType.Interface)] out IOleInPlaceFrame ppFrame, [MarshalAs(UnmanagedType.Interface)] out IOleInPlaceUIWindow ppDoc, [Out] NativeMethods.COMRECT lprcPosRect, [Out] NativeMethods.COMRECT lprcClipRect, [In][Out] NativeMethods.OLEINPLACEFRAMEINFO lpFrameInfo);

		[PreserveSig]
		int Scroll(NativeMethods.SIZE scrollExtant);

		[PreserveSig]
		int OnUIDeactivate(int fUndoable);

		[PreserveSig]
		int OnInPlaceDeactivate();

		[PreserveSig]
		int DiscardUndoState();

		[PreserveSig]
		int DeactivateAndUndo();

		[PreserveSig]
		int OnPosRectChange([In] NativeMethods.COMRECT lprcPosRect);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("9BFBBC02-EFF1-101A-84ED-00AA00341D07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPropertyNotifySink
	{
		void OnChanged(int dispID);

		[PreserveSig]
		int OnRequestEdit(int dispID);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("00000100-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumUnknown
	{
		[PreserveSig]
		int Next([In][MarshalAs(UnmanagedType.U4)] int celt, [Out] IntPtr rgelt, IntPtr pceltFetched);

		[PreserveSig]
		int Skip([In][MarshalAs(UnmanagedType.U4)] int celt);

		void Reset();

		void Clone(out IEnumUnknown ppenum);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("0000011B-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleContainer
	{
		[PreserveSig]
		int ParseDisplayName([In][MarshalAs(UnmanagedType.Interface)] object pbc, [In][MarshalAs(UnmanagedType.BStr)] string pszDisplayName, [Out][MarshalAs(UnmanagedType.LPArray)] int[] pchEaten, [Out][MarshalAs(UnmanagedType.LPArray)] object[] ppmkOut);

		[PreserveSig]
		int EnumObjects([In][MarshalAs(UnmanagedType.U4)] int grfFlags, out IEnumUnknown ppenum);

		[PreserveSig]
		int LockContainer(bool fLock);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("00000116-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleInPlaceFrame
	{
		IntPtr GetWindow();

		[PreserveSig]
		int ContextSensitiveHelp(int fEnterMode);

		[PreserveSig]
		int GetBorder([Out] NativeMethods.COMRECT lprectBorder);

		[PreserveSig]
		int RequestBorderSpace([In] NativeMethods.COMRECT pborderwidths);

		[PreserveSig]
		int SetBorderSpace([In] NativeMethods.COMRECT pborderwidths);

		[PreserveSig]
		int SetActiveObject([In][MarshalAs(UnmanagedType.Interface)] IOleInPlaceActiveObject pActiveObject, [In][MarshalAs(UnmanagedType.LPWStr)] string pszObjName);

		[PreserveSig]
		int InsertMenus([In] IntPtr hmenuShared, [In][Out] NativeMethods.tagOleMenuGroupWidths lpMenuWidths);

		[PreserveSig]
		int SetMenu([In] IntPtr hmenuShared, [In] IntPtr holemenu, [In] IntPtr hwndActiveObject);

		[PreserveSig]
		int RemoveMenus([In] IntPtr hmenuShared);

		[PreserveSig]
		int SetStatusText([In][MarshalAs(UnmanagedType.LPWStr)] string pszStatusText);

		[PreserveSig]
		int EnableModeless(bool fEnable);

		[PreserveSig]
		int TranslateAccelerator([In] ref MSG lpmsg, [In][MarshalAs(UnmanagedType.U2)] short wID);
	}

	public enum OLECMDID
	{
		OLECMDID_SAVE = 3,
		OLECMDID_SAVEAS = 4,
		OLECMDID_PRINT = 6,
		OLECMDID_PRINTPREVIEW = 7,
		OLECMDID_PAGESETUP = 8,
		OLECMDID_PROPERTIES = 10,
		OLECMDID_CUT = 11,
		OLECMDID_COPY = 12,
		OLECMDID_PASTE = 13,
		OLECMDID_SELECTALL = 17,
		OLECMDID_REFRESH = 22,
		OLECMDID_STOP = 23
	}

	public enum OLECMDEXECOPT
	{
		OLECMDEXECOPT_DODEFAULT,
		OLECMDEXECOPT_PROMPTUSER,
		OLECMDEXECOPT_DONTPROMPTUSER,
		OLECMDEXECOPT_SHOWHELP
	}

	public enum OLECMDF
	{
		OLECMDF_SUPPORTED = 1,
		OLECMDF_ENABLED = 2,
		OLECMDF_LATCHED = 4,
		OLECMDF_NINCHED = 8,
		OLECMDF_INVISIBLE = 0x10,
		OLECMDF_DEFHIDEONCTXTMENU = 0x20
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("00000115-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleInPlaceUIWindow
	{
		IntPtr GetWindow();

		[PreserveSig]
		int ContextSensitiveHelp(int fEnterMode);

		[PreserveSig]
		int GetBorder([Out] NativeMethods.RECT lprectBorder);

		[PreserveSig]
		int RequestBorderSpace([In] NativeMethods.RECT pborderwidths);

		[PreserveSig]
		int SetBorderSpace([In] NativeMethods.RECT pborderwidths);

		void SetActiveObject([In][MarshalAs(UnmanagedType.Interface)] IOleInPlaceActiveObject pActiveObject, [In][MarshalAs(UnmanagedType.LPWStr)] string pszObjName);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("00000117-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleInPlaceActiveObject
	{
		[PreserveSig]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		int GetWindow(out IntPtr hwnd);

		void ContextSensitiveHelp(int fEnterMode);

		[PreserveSig]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		int TranslateAccelerator([In] ref MSG lpmsg);

		void OnFrameWindowActivate(int fActivate);

		void OnDocWindowActivate(int fActivate);

		void ResizeBorder([In] NativeMethods.RECT prcBorder, [In] IOleInPlaceUIWindow pUIWindow, bool fFrameWindow);

		void EnableModeless(int fEnable);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("00000114-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleWindow
	{
		[PreserveSig]
		int GetWindow(out IntPtr hwnd);

		void ContextSensitiveHelp(int fEnterMode);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("00000113-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleInPlaceObject
	{
		[PreserveSig]
		int GetWindow(out IntPtr hwnd);

		void ContextSensitiveHelp(int fEnterMode);

		void InPlaceDeactivate();

		[PreserveSig]
		int UIDeactivate();

		void SetObjectRects([In] NativeMethods.COMRECT lprcPosRect, [In] NativeMethods.COMRECT lprcClipRect);

		void ReactivateAndUndo();
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("00000112-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleObject
	{
		[PreserveSig]
		int SetClientSite([In][MarshalAs(UnmanagedType.Interface)] IOleClientSite pClientSite);

		IOleClientSite GetClientSite();

		[PreserveSig]
		int SetHostNames([In][MarshalAs(UnmanagedType.LPWStr)] string szContainerApp, [In][MarshalAs(UnmanagedType.LPWStr)] string szContainerObj);

		[PreserveSig]
		int Close(int dwSaveOption);

		[PreserveSig]
		int SetMoniker([In][MarshalAs(UnmanagedType.U4)] int dwWhichMoniker, [In][MarshalAs(UnmanagedType.Interface)] object pmk);

		[PreserveSig]
		int GetMoniker([In][MarshalAs(UnmanagedType.U4)] int dwAssign, [In][MarshalAs(UnmanagedType.U4)] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] out object moniker);

		[PreserveSig]
		int InitFromData([In][MarshalAs(UnmanagedType.Interface)] IDataObject pDataObject, int fCreation, [In][MarshalAs(UnmanagedType.U4)] int dwReserved);

		[PreserveSig]
		int GetClipboardData([In][MarshalAs(UnmanagedType.U4)] int dwReserved, out IDataObject data);

		[PreserveSig]
		int DoVerb(int iVerb, [In] IntPtr lpmsg, [In][MarshalAs(UnmanagedType.Interface)] IOleClientSite pActiveSite, int lindex, IntPtr hwndParent, [In] NativeMethods.COMRECT lprcPosRect);

		[PreserveSig]
		int EnumVerbs(out IEnumOLEVERB e);

		[PreserveSig]
		int OleUpdate();

		[PreserveSig]
		int IsUpToDate();

		[PreserveSig]
		int GetUserClassID([In][Out] ref Guid pClsid);

		[PreserveSig]
		int GetUserType([In][MarshalAs(UnmanagedType.U4)] int dwFormOfType, [MarshalAs(UnmanagedType.LPWStr)] out string userType);

		[PreserveSig]
		int SetExtent([In][MarshalAs(UnmanagedType.U4)] int dwDrawAspect, [In] NativeMethods.SIZE pSizel);

		[PreserveSig]
		int GetExtent([In][MarshalAs(UnmanagedType.U4)] int dwDrawAspect, [Out] NativeMethods.SIZE pSizel);

		[PreserveSig]
		int Advise(IAdviseSink pAdvSink, out int cookie);

		[PreserveSig]
		int Unadvise([In][MarshalAs(UnmanagedType.U4)] int dwConnection);

		[PreserveSig]
		int EnumAdvise(out IEnumSTATDATA e);

		[PreserveSig]
		int GetMiscStatus([In][MarshalAs(UnmanagedType.U4)] int dwAspect, out int misc);

		[PreserveSig]
		int SetColorScheme([In] NativeMethods.tagLOGPALETTE pLogpal);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("1C2056CC-5EF4-101B-8BC8-00AA003E3B29")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleInPlaceObjectWindowless
	{
		[PreserveSig]
		int SetClientSite([In][MarshalAs(UnmanagedType.Interface)] IOleClientSite pClientSite);

		[PreserveSig]
		int GetClientSite(out IOleClientSite site);

		[PreserveSig]
		int SetHostNames([In][MarshalAs(UnmanagedType.LPWStr)] string szContainerApp, [In][MarshalAs(UnmanagedType.LPWStr)] string szContainerObj);

		[PreserveSig]
		int Close(int dwSaveOption);

		[PreserveSig]
		int SetMoniker([In][MarshalAs(UnmanagedType.U4)] int dwWhichMoniker, [In][MarshalAs(UnmanagedType.Interface)] object pmk);

		[PreserveSig]
		int GetMoniker([In][MarshalAs(UnmanagedType.U4)] int dwAssign, [In][MarshalAs(UnmanagedType.U4)] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] out object moniker);

		[PreserveSig]
		int InitFromData([In][MarshalAs(UnmanagedType.Interface)] IDataObject pDataObject, int fCreation, [In][MarshalAs(UnmanagedType.U4)] int dwReserved);

		[PreserveSig]
		int GetClipboardData([In][MarshalAs(UnmanagedType.U4)] int dwReserved, out IDataObject data);

		[PreserveSig]
		int DoVerb(int iVerb, [In] IntPtr lpmsg, [In][MarshalAs(UnmanagedType.Interface)] IOleClientSite pActiveSite, int lindex, IntPtr hwndParent, [In] NativeMethods.RECT lprcPosRect);

		[PreserveSig]
		int EnumVerbs(out IEnumOLEVERB e);

		[PreserveSig]
		int OleUpdate();

		[PreserveSig]
		int IsUpToDate();

		[PreserveSig]
		int GetUserClassID([In][Out] ref Guid pClsid);

		[PreserveSig]
		int GetUserType([In][MarshalAs(UnmanagedType.U4)] int dwFormOfType, [MarshalAs(UnmanagedType.LPWStr)] out string userType);

		[PreserveSig]
		int SetExtent([In][MarshalAs(UnmanagedType.U4)] int dwDrawAspect, [In] NativeMethods.SIZE pSizel);

		[PreserveSig]
		int GetExtent([In][MarshalAs(UnmanagedType.U4)] int dwDrawAspect, [Out] NativeMethods.SIZE pSizel);

		[PreserveSig]
		int Advise([In][MarshalAs(UnmanagedType.Interface)] IAdviseSink pAdvSink, out int cookie);

		[PreserveSig]
		int Unadvise([In][MarshalAs(UnmanagedType.U4)] int dwConnection);

		[PreserveSig]
		int EnumAdvise(out IEnumSTATDATA e);

		[PreserveSig]
		int GetMiscStatus([In][MarshalAs(UnmanagedType.U4)] int dwAspect, out int misc);

		[PreserveSig]
		int SetColorScheme([In] NativeMethods.tagLOGPALETTE pLogpal);

		[PreserveSig]
		int OnWindowMessage([In][MarshalAs(UnmanagedType.U4)] int msg, [In][MarshalAs(UnmanagedType.U4)] int wParam, [In][MarshalAs(UnmanagedType.U4)] int lParam, [Out][MarshalAs(UnmanagedType.U4)] int plResult);

		[PreserveSig]
		int GetDropTarget([Out][MarshalAs(UnmanagedType.Interface)] object ppDropTarget);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("B196B288-BAB4-101A-B69C-00AA00341D07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleControl
	{
		[PreserveSig]
		int GetControlInfo([Out] NativeMethods.tagCONTROLINFO pCI);

		[PreserveSig]
		int OnMnemonic([In] ref MSG pMsg);

		[PreserveSig]
		int OnAmbientPropertyChange(int dispID);

		[PreserveSig]
		int FreezeEvents(int bFreeze);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("B196B286-BAB4-101A-B69C-00AA00341D07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IConnectionPoint
	{
		[PreserveSig]
		int GetConnectionInterface(out Guid iid);

		[PreserveSig]
		int GetConnectionPointContainer([MarshalAs(UnmanagedType.Interface)] ref IConnectionPointContainer pContainer);

		[PreserveSig]
		int Advise([In][MarshalAs(UnmanagedType.Interface)] object pUnkSink, ref int cookie);

		[PreserveSig]
		int Unadvise(int cookie);

		[PreserveSig]
		int EnumConnections(out object pEnum);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("00020404-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumVariant
	{
		[PreserveSig]
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		int Next([In][MarshalAs(UnmanagedType.U4)] int celt, [In][Out] IntPtr rgvar, [Out][MarshalAs(UnmanagedType.LPArray)] int[] pceltFetched);

		void Skip([In][MarshalAs(UnmanagedType.U4)] int celt);

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void Reset();

		void Clone([Out][MarshalAs(UnmanagedType.LPArray)] IEnumVariant[] ppenum);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("00000104-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumOLEVERB
	{
		[PreserveSig]
		int Next([MarshalAs(UnmanagedType.U4)] int celt, [Out] NativeMethods.tagOLEVERB rgelt, [Out][MarshalAs(UnmanagedType.LPArray)] int[] pceltFetched);

		[PreserveSig]
		int Skip([In][MarshalAs(UnmanagedType.U4)] int celt);

		void Reset();

		void Clone(out IEnumOLEVERB ppenum);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("0000000C-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IStream
	{
		int Read(IntPtr buf, int len);

		int Write(IntPtr buf, int len);

		[return: MarshalAs(UnmanagedType.I8)]
		long Seek([In][MarshalAs(UnmanagedType.I8)] long dlibMove, int dwOrigin);

		void SetSize([In][MarshalAs(UnmanagedType.I8)] long libNewSize);

		[return: MarshalAs(UnmanagedType.I8)]
		long CopyTo([In][MarshalAs(UnmanagedType.Interface)] IStream pstm, [In][MarshalAs(UnmanagedType.I8)] long cb, [Out][MarshalAs(UnmanagedType.LPArray)] long[] pcbRead);

		void Commit(int grfCommitFlags);

		void Revert();

		void LockRegion([In][MarshalAs(UnmanagedType.I8)] long libOffset, [In][MarshalAs(UnmanagedType.I8)] long cb, int dwLockType);

		void UnlockRegion([In][MarshalAs(UnmanagedType.I8)] long libOffset, [In][MarshalAs(UnmanagedType.I8)] long cb, int dwLockType);

		void Stat([Out] NativeMethods.STATSTG pStatstg, int grfStatFlag);

		[return: MarshalAs(UnmanagedType.Interface)]
		IStream Clone();
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("B196B284-BAB4-101A-B69C-00AA00341D07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IConnectionPointContainer
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		object EnumConnectionPoints();

		[PreserveSig]
		int FindConnectionPoint([In] ref Guid guid, [MarshalAs(UnmanagedType.Interface)] out IConnectionPoint ppCP);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("B196B285-BAB4-101A-B69C-00AA00341D07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumConnectionPoints
	{
		[PreserveSig]
		int Next(int cConnections, out IConnectionPoint pCp, out int pcFetched);

		[PreserveSig]
		int Skip(int cSkip);

		void Reset();

		IEnumConnectionPoints Clone();
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("00020400-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDispatch
	{
		int GetTypeInfoCount();

		[return: MarshalAs(UnmanagedType.Interface)]
		ITypeInfo GetTypeInfo([In][MarshalAs(UnmanagedType.U4)] int iTInfo, [In][MarshalAs(UnmanagedType.U4)] int lcid);

		[PreserveSig]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		MS.Internal.Interop.HRESULT GetIDsOfNames([In] ref Guid riid, [In][MarshalAs(UnmanagedType.LPArray)] string[] rgszNames, [In][MarshalAs(UnmanagedType.U4)] int cNames, [In][MarshalAs(UnmanagedType.U4)] int lcid, [Out][MarshalAs(UnmanagedType.LPArray)] int[] rgDispId);

		[PreserveSig]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		MS.Internal.Interop.HRESULT Invoke(int dispIdMember, [In] ref Guid riid, [In][MarshalAs(UnmanagedType.U4)] int lcid, [In][MarshalAs(UnmanagedType.U4)] int dwFlags, [In][Out] NativeMethods.DISPPARAMS pDispParams, out object pVarResult, [In][Out] NativeMethods.EXCEPINFO pExcepInfo, [Out][MarshalAs(UnmanagedType.LPArray)] IntPtr[] pArgErr);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("A6EF9860-C720-11D0-9337-00A0C90DCAA9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDispatchEx : IDispatch
	{
		new int GetTypeInfoCount();

		[return: MarshalAs(UnmanagedType.Interface)]
		new ITypeInfo GetTypeInfo([In][MarshalAs(UnmanagedType.U4)] int iTInfo, [In][MarshalAs(UnmanagedType.U4)] int lcid);

		[PreserveSig]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		new MS.Internal.Interop.HRESULT GetIDsOfNames([In] ref Guid riid, [In][MarshalAs(UnmanagedType.LPArray)] string[] rgszNames, [In][MarshalAs(UnmanagedType.U4)] int cNames, [In][MarshalAs(UnmanagedType.U4)] int lcid, [Out][MarshalAs(UnmanagedType.LPArray)] int[] rgDispId);

		[PreserveSig]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		new MS.Internal.Interop.HRESULT Invoke(int dispIdMember, [In] ref Guid riid, [In][MarshalAs(UnmanagedType.U4)] int lcid, [In][MarshalAs(UnmanagedType.U4)] int dwFlags, [In][Out] NativeMethods.DISPPARAMS pDispParams, out object pVarResult, [In][Out] NativeMethods.EXCEPINFO pExcepInfo, [Out][MarshalAs(UnmanagedType.LPArray)] IntPtr[] pArgErr);

		[PreserveSig]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		MS.Internal.Interop.HRESULT GetDispID(string name, int nameProperties, out int dispId);

		[PreserveSig]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		MS.Internal.Interop.HRESULT InvokeEx(int dispId, [MarshalAs(UnmanagedType.U4)] int lcid, [MarshalAs(UnmanagedType.U4)] int flags, [In][Out] NativeMethods.DISPPARAMS dispParams, out object result, [In][Out] NativeMethods.EXCEPINFO exceptionInfo, IServiceProvider serviceProvider);

		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		void DeleteMemberByName(string name, int flags);

		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		void DeleteMemberByDispID(int dispId);

		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		int GetMemberProperties(int dispId, int propFlags);

		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		string GetMemberName(int dispId);

		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		int GetNextDispID(int enumFlags, int dispId);

		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetNameSpaceParent();
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IServiceProvider
	{
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object QueryService(ref Guid service, ref Guid riid);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E")]
	[TypeLibType(TypeLibTypeFlags.FHidden | TypeLibTypeFlags.FDual | TypeLibTypeFlags.FOleAutomation)]
	public interface IWebBrowser2
	{
		[DispId(100)]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		void GoBack();

		[DispId(101)]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		void GoForward();

		[DispId(102)]
		void GoHome();

		[DispId(103)]
		void GoSearch();

		[DispId(104)]
		void Navigate([In] string Url, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers);

		[DispId(-550)]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		void Refresh();

		[DispId(105)]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		void Refresh2([In] ref object level);

		[DispId(106)]
		void Stop();

		[DispId(200)]
		object Application
		{
			[return: MarshalAs(UnmanagedType.IDispatch)]
			get;
		}

		[DispId(201)]
		object Parent
		{
			[return: MarshalAs(UnmanagedType.IDispatch)]
			get;
		}

		[DispId(202)]
		object Container
		{
			[return: MarshalAs(UnmanagedType.IDispatch)]
			get;
		}

		[DispId(203)]
		object Document
		{
			[SuppressUnmanagedCodeSecurity]
			[SecurityCritical]
			[return: MarshalAs(UnmanagedType.IDispatch)]
			get;
		}

		[DispId(204)]
		bool TopLevelContainer { get; }

		[DispId(205)]
		string Type { get; }

		[DispId(206)]
		int Left { get; set; }

		[DispId(207)]
		int Top { get; set; }

		[DispId(208)]
		int Width { get; set; }

		[DispId(209)]
		int Height { get; set; }

		[DispId(210)]
		string LocationName { get; }

		[DispId(211)]
		string LocationURL
		{
			[SuppressUnmanagedCodeSecurity]
			[SecurityCritical]
			get;
		}

		[DispId(212)]
		bool Busy { get; }

		[DispId(300)]
		void Quit();

		[DispId(301)]
		void ClientToWindow(out int pcx, out int pcy);

		[DispId(302)]
		void PutProperty([In] string property, [In] object vtValue);

		[DispId(303)]
		object GetProperty([In] string property);

		[DispId(0)]
		string Name { get; }

		[DispId(-515)]
		int HWND { get; }

		[DispId(400)]
		string FullName { get; }

		[DispId(401)]
		string Path { get; }

		[DispId(402)]
		bool Visible { get; set; }

		[DispId(403)]
		bool StatusBar { get; set; }

		[DispId(404)]
		string StatusText { get; set; }

		[DispId(405)]
		int ToolBar { get; set; }

		[DispId(406)]
		bool MenuBar { get; set; }

		[DispId(407)]
		bool FullScreen { get; set; }

		[DispId(500)]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		void Navigate2([In] ref object URL, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers);

		[DispId(501)]
		OLECMDF QueryStatusWB([In] OLECMDID cmdID);

		[DispId(502)]
		void ExecWB([In] OLECMDID cmdID, [In] OLECMDEXECOPT cmdexecopt, ref object pvaIn, IntPtr pvaOut);

		[DispId(503)]
		void ShowBrowserBar([In] ref object pvaClsid, [In] ref object pvarShow, [In] ref object pvarSize);

		[DispId(-525)]
		NativeMethods.WebBrowserReadyState ReadyState { get; }

		[DispId(550)]
		bool Offline { get; set; }

		[DispId(551)]
		bool Silent { get; set; }

		[DispId(552)]
		bool RegisterAsBrowser { get; set; }

		[DispId(553)]
		bool RegisterAsDropTarget { get; set; }

		[DispId(554)]
		bool TheaterMode { get; set; }

		[DispId(555)]
		bool AddressBar { get; set; }

		[DispId(556)]
		bool Resizable { get; set; }
	}

	[ComImport]
	[Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D")]
	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	public interface DWebBrowserEvents2
	{
		[DispId(102)]
		void StatusTextChange([In] string text);

		[DispId(108)]
		void ProgressChange([In] int progress, [In] int progressMax);

		[DispId(105)]
		void CommandStateChange([In] long command, [In] bool enable);

		[DispId(106)]
		void DownloadBegin();

		[DispId(104)]
		void DownloadComplete();

		[DispId(113)]
		void TitleChange([In] string text);

		[DispId(112)]
		void PropertyChange([In] string szProperty);

		[DispId(225)]
		void PrintTemplateInstantiation([In][MarshalAs(UnmanagedType.IDispatch)] object pDisp);

		[DispId(226)]
		void PrintTemplateTeardown([In][MarshalAs(UnmanagedType.IDispatch)] object pDisp);

		[DispId(227)]
		void UpdatePageStatus([In][MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object nPage, [In] ref object fDone);

		[DispId(250)]
		void BeforeNavigate2([In][MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers, [In][Out] ref bool cancel);

		[DispId(251)]
		void NewWindow2([In][Out][MarshalAs(UnmanagedType.IDispatch)] ref object pDisp, [In][Out] ref bool cancel);

		[DispId(252)]
		void NavigateComplete2([In][MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL);

		[DispId(259)]
		void DocumentComplete([In][MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL);

		[DispId(253)]
		void OnQuit();

		[DispId(254)]
		void OnVisible([In] bool visible);

		[DispId(255)]
		void OnToolBar([In] bool toolBar);

		[DispId(256)]
		void OnMenuBar([In] bool menuBar);

		[DispId(257)]
		void OnStatusBar([In] bool statusBar);

		[DispId(258)]
		void OnFullScreen([In] bool fullScreen);

		[DispId(260)]
		void OnTheaterMode([In] bool theaterMode);

		[DispId(262)]
		void WindowSetResizable([In] bool resizable);

		[DispId(264)]
		void WindowSetLeft([In] int left);

		[DispId(265)]
		void WindowSetTop([In] int top);

		[DispId(266)]
		void WindowSetWidth([In] int width);

		[DispId(267)]
		void WindowSetHeight([In] int height);

		[DispId(263)]
		void WindowClosing([In] bool isChildWindow, [In][Out] ref bool cancel);

		[DispId(268)]
		void ClientToHostWindow([In][Out] ref long cx, [In][Out] ref long cy);

		[DispId(269)]
		void SetSecureLockIcon([In] int secureLockIcon);

		[DispId(270)]
		void FileDownload([In][Out] ref bool ActiveDocument, [In][Out] ref bool cancel);

		[DispId(271)]
		void NavigateError([In][MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL, [In] ref object frame, [In] ref object statusCode, [In][Out] ref bool cancel);

		[DispId(272)]
		void PrivacyImpactedStateChange([In] bool bImpacted);

		[DispId(282)]
		void SetPhishingFilterStatus(uint phishingFilterStatus);

		[DispId(283)]
		void WindowStateChanged(uint dwFlags, uint dwValidFlagsMask);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("BD3F23C0-D43E-11CF-893B-00AA00BDCE1A")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IDocHostUIHandler
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int ShowContextMenu([In][MarshalAs(UnmanagedType.U4)] int dwID, [In] NativeMethods.POINT pt, [In][MarshalAs(UnmanagedType.Interface)] object pcmdtReserved, [In][MarshalAs(UnmanagedType.Interface)] object pdispReserved);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetHostInfo([In][Out] NativeMethods.DOCHOSTUIINFO info);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int ShowUI([In][MarshalAs(UnmanagedType.I4)] int dwID, [In] IOleInPlaceActiveObject activeObject, [In] NativeMethods.IOleCommandTarget commandTarget, [In] IOleInPlaceFrame frame, [In] IOleInPlaceUIWindow doc);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int HideUI();

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int UpdateUI();

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int EnableModeless([In][MarshalAs(UnmanagedType.Bool)] bool fEnable);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int OnDocWindowActivate([In][MarshalAs(UnmanagedType.Bool)] bool fActivate);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int OnFrameWindowActivate([In][MarshalAs(UnmanagedType.Bool)] bool fActivate);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int ResizeBorder([In] NativeMethods.COMRECT rect, [In] IOleInPlaceUIWindow doc, bool fFrameWindow);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int TranslateAccelerator([In] ref MSG msg, [In] ref Guid group, [In][MarshalAs(UnmanagedType.I4)] int nCmdID);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetOptionKeyPath([Out][MarshalAs(UnmanagedType.LPArray)] string[] pbstrKey, [In][MarshalAs(UnmanagedType.U4)] int dw);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetDropTarget([In][MarshalAs(UnmanagedType.Interface)] IOleDropTarget pDropTarget, [MarshalAs(UnmanagedType.Interface)] out IOleDropTarget ppDropTarget);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetExternal([MarshalAs(UnmanagedType.IDispatch)] out object ppDispatch);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int TranslateUrl([In][MarshalAs(UnmanagedType.U4)] int dwTranslate, [In][MarshalAs(UnmanagedType.LPWStr)] string strURLIn, [MarshalAs(UnmanagedType.LPWStr)] out string pstrURLOut);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int FilterDataObject(IDataObject pDO, out IDataObject ppDORet);
	}

	[ComImport]
	[Guid("3050F21F-98B5-11CF-BB82-00AA00BDCE0B")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	internal interface IHTMLElementCollection
	{
		string toString();

		void SetLength(int p);

		int GetLength();

		[return: MarshalAs(UnmanagedType.Interface)]
		object Get_newEnum();

		[return: MarshalAs(UnmanagedType.IDispatch)]
		object Item(object idOrName, object index);

		[return: MarshalAs(UnmanagedType.Interface)]
		object Tags(object tagName);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("626FC520-A41E-11CF-A731-00A0C9082637")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	internal interface IHTMLDocument
	{
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		object GetScript();
	}

	[ComImport]
	[Guid("332C4425-26CB-11D0-B483-00C04FD90119")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	internal interface IHTMLDocument2 : IHTMLDocument
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		new object GetScript();

		IHTMLElementCollection GetAll();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetBody();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetActiveElement();

		IHTMLElementCollection GetImages();

		IHTMLElementCollection GetApplets();

		IHTMLElementCollection GetLinks();

		IHTMLElementCollection GetForms();

		IHTMLElementCollection GetAnchors();

		void SetTitle(string p);

		string GetTitle();

		IHTMLElementCollection GetScripts();

		void SetDesignMode(string p);

		string GetDesignMode();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetSelection();

		string GetReadyState();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetFrames();

		IHTMLElementCollection GetEmbeds();

		IHTMLElementCollection GetPlugins();

		void SetAlinkColor(object c);

		object GetAlinkColor();

		void SetBgColor(object c);

		object GetBgColor();

		void SetFgColor(object c);

		object GetFgColor();

		void SetLinkColor(object c);

		object GetLinkColor();

		void SetVlinkColor(object c);

		object GetVlinkColor();

		string GetReferrer();

		IHTMLLocation GetLocation();

		string GetLastModified();

		void SetUrl(string p);

		string GetUrl();

		void SetDomain(string p);

		string GetDomain();

		void SetCookie(string p);

		string GetCookie();

		void SetExpando(bool p);

		bool GetExpando();

		void SetCharset(string p);

		string GetCharset();

		void SetDefaultCharset(string p);

		string GetDefaultCharset();

		string GetMimeType();

		string GetFileSize();

		string GetFileCreatedDate();

		string GetFileModifiedDate();

		string GetFileUpdatedDate();

		string GetSecurity();

		string GetProtocol();

		string GetNameProp();

		int Write([In][MarshalAs(UnmanagedType.SafeArray)] object[] psarray);

		int WriteLine([In][MarshalAs(UnmanagedType.SafeArray)] object[] psarray);

		[return: MarshalAs(UnmanagedType.Interface)]
		object Open(string mimeExtension, object name, object features, object replace);

		void Close();

		void Clear();

		bool QueryCommandSupported(string cmdID);

		bool QueryCommandEnabled(string cmdID);

		bool QueryCommandState(string cmdID);

		bool QueryCommandIndeterm(string cmdID);

		string QueryCommandText(string cmdID);

		object QueryCommandValue(string cmdID);

		bool ExecCommand(string cmdID, bool showUI, object value);

		bool ExecCommandShowHelp(string cmdID);

		[return: MarshalAs(UnmanagedType.Interface)]
		object CreateElement(string eTag);

		void SetOnhelp(object p);

		object GetOnhelp();

		void SetOnclick(object p);

		object GetOnclick();

		void SetOndblclick(object p);

		object GetOndblclick();

		void SetOnkeyup(object p);

		object GetOnkeyup();

		void SetOnkeydown(object p);

		object GetOnkeydown();

		void SetOnkeypress(object p);

		object GetOnkeypress();

		void SetOnmouseup(object p);

		object GetOnmouseup();

		void SetOnmousedown(object p);

		object GetOnmousedown();

		void SetOnmousemove(object p);

		object GetOnmousemove();

		void SetOnmouseout(object p);

		object GetOnmouseout();

		void SetOnmouseover(object p);

		object GetOnmouseover();

		void SetOnreadystatechange(object p);

		object GetOnreadystatechange();

		void SetOnafterupdate(object p);

		object GetOnafterupdate();

		void SetOnrowexit(object p);

		object GetOnrowexit();

		void SetOnrowenter(object p);

		object GetOnrowenter();

		void SetOndragstart(object p);

		object GetOndragstart();

		void SetOnselectstart(object p);

		object GetOnselectstart();

		[return: MarshalAs(UnmanagedType.Interface)]
		object ElementFromPoint(int x, int y);

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetParentWindow();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetStyleSheets();

		void SetOnbeforeupdate(object p);

		object GetOnbeforeupdate();

		void SetOnerrorupdate(object p);

		object GetOnerrorupdate();

		string toString();

		[return: MarshalAs(UnmanagedType.Interface)]
		object CreateStyleSheet(string bstrHref, int lIndex);
	}

	[ComImport]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[Guid("163BB1E0-6E00-11CF-837A-48DC04C10000")]
	internal interface IHTMLLocation
	{
		void SetHref(string p);

		string GetHref();

		void SetProtocol(string p);

		string GetProtocol();

		void SetHost(string p);

		string GetHost();

		void SetHostname(string p);

		string GetHostname();

		void SetPort(string p);

		string GetPort();

		void SetPathname(string p);

		string GetPathname();

		void SetSearch(string p);

		string GetSearch();

		void SetHash(string p);

		string GetHash();

		void Reload(bool flag);

		void Replace(string bstr);

		void Assign(string bstr);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("3050f6cf-98b5-11cf-bb82-00aa00bdce0b")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	internal interface IHTMLWindow4
	{
		[return: MarshalAs(UnmanagedType.IDispatch)]
		object CreatePopup([In] ref object reserved);

		[return: MarshalAs(UnmanagedType.Interface)]
		object frameElement();
	}

	internal static class ArrayToVARIANTHelper
	{
		[SecurityCritical]
		[SecurityTreatAsSafe]
		static ArrayToVARIANTHelper()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		[SecurityCritical]
		public static IntPtr ArrayToVARIANTVector(object[] args)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		[SecurityCritical]
		public static void FreeVARIANTVector(IntPtr mem, int len)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("7FD52380-4E07-101B-AE2D-08002B2EC713")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IPersistStreamInit
	{
		void GetClassID(out Guid pClassID);

		[PreserveSig]
		int IsDirty();

		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		void Load([In][MarshalAs(UnmanagedType.Interface)] System.Runtime.InteropServices.ComTypes.IStream pstm);

		void Save([In][MarshalAs(UnmanagedType.Interface)] IStream pstm, [In][MarshalAs(UnmanagedType.Bool)] bool fClearDirty);

		void GetSizeMax([Out][MarshalAs(UnmanagedType.LPArray)] long pcbSize);

		void InitNew();
	}

	[Flags]
	internal enum BrowserNavConstants : uint
	{
		OpenInNewWindow = 1u,
		NoHistory = 2u,
		NoReadFromCache = 4u,
		NoWriteToCache = 8u,
		AllowAutosearch = 0x10u,
		BrowserBar = 0x20u,
		Hyperlink = 0x40u,
		EnforceRestricted = 0x80u,
		NewWindowsManaged = 0x100u,
		UntrustedForDownload = 0x200u,
		TrustedForActiveX = 0x400u,
		OpenInNewTab = 0x800u,
		OpenInBackgroundTab = 0x1000u,
		KeepWordWheelText = 0x2000u
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[ComVisible(false)]
	[Guid("79eac9ee-baf9-11ce-8c82-00aa004ba90b")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IInternetSecurityManager
	{
		void SetSecuritySite(NativeMethods.IInternetSecurityMgrSite pSite);

		unsafe void GetSecuritySite(void** ppSite);

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void MapUrlToZone([In][MarshalAs(UnmanagedType.BStr)] string pwszUrl, out int pdwZone, [In] int dwFlags);

		unsafe void GetSecurityId(string pwszUrl, byte* pbSecurityId, int* pcbSecurityId, int dwReserved);

		unsafe void ProcessUrlAction(string pwszUrl, int dwAction, byte* pPolicy, int cbPolicy, byte* pContext, int cbContext, int dwFlags, int dwReserved);

		unsafe void QueryCustomPolicy(string pwszUrl, void* guidKey, byte** ppPolicy, int* pcbPolicy, byte* pContext, int cbContext, int dwReserved);

		void SetZoneMapping(int dwZone, string lpszPattern, int dwFlags);

		unsafe void GetZoneMappings(int dwZone, void** ppenumString, int dwFlags);
	}

	internal sealed class SafeFileMappingHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		public override bool IsInvalid
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		[SecurityCritical]
		internal SafeFileMappingHandle(IntPtr handle)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal SafeFileMappingHandle()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override bool ReleaseHandle()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal sealed class SafeViewOfFileHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		internal unsafe void* Memory
		{
			[SecurityCritical]
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal SafeViewOfFileHandle()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override bool ReleaseHandle()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	internal class ICONINFO_IMPL
	{
		public bool fIcon;

		public int xHotspot;

		public int yHotspot;

		public IntPtr hbmMask;

		public IntPtr hbmColor;

		public ICONINFO_IMPL()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	internal struct ULARGE_INTEGER
	{
		[FieldOffset(0)]
		internal uint LowPart;

		[FieldOffset(4)]
		internal uint HighPart;

		[FieldOffset(0)]
		internal ulong QuadPart;
	}

	[StructLayout(LayoutKind.Explicit)]
	internal struct LARGE_INTEGER
	{
		[FieldOffset(0)]
		internal int LowPart;

		[FieldOffset(4)]
		internal int HighPart;

		[FieldOffset(0)]
		internal long QuadPart;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct MOUSEQUERY
	{
		internal uint uMsg;

		internal IntPtr wParam;

		internal IntPtr lParam;

		internal int ptX;

		internal int ptY;

		internal IntPtr hwnd;
	}

	public struct PROFILEHEADER
	{
		public uint phSize;

		public uint phCMMType;

		public uint phVersion;

		public uint phClass;

		public NativeMethods.ColorSpace phDataColorSpace;

		public uint phConnectionSpace;

		public uint phDateTime_0;

		public uint phDateTime_1;

		public uint phDateTime_2;

		public uint phSignature;

		public uint phPlatform;

		public uint phProfileFlags;

		public uint phManufacturer;

		public uint phModel;

		public uint phAttributes_0;

		public uint phAttributes_1;

		public uint phRenderingIntent;

		public uint phIlluminant_0;

		public uint phIlluminant_1;

		public uint phIlluminant_2;

		public uint phCreator;

		public unsafe fixed byte phReserved[44];
	}

	public struct PROFILE
	{
		public NativeMethods.ProfileType dwType;

		[SecurityCritical]
		public unsafe void* pProfileData;

		public uint cbDataSize;
	}

	public enum HookType
	{
		WH_JOURNALRECORD,
		WH_JOURNALPLAYBACK,
		WH_KEYBOARD,
		WH_GETMESSAGE,
		WH_CALLWNDPROC,
		WH_CBT,
		WH_SYSMSGFILTER,
		WH_MOUSE,
		WH_HARDWARE,
		WH_DEBUG,
		WH_SHELL,
		WH_FOREGROUNDIDLE,
		WH_CALLWNDPROCRET,
		WH_KEYBOARD_LL,
		WH_MOUSE_LL
	}

	public struct MOUSEHOOKSTRUCT
	{
		public NativeMethods.POINT pt;

		public IntPtr hwnd;

		public uint wHitTestCode;

		public IntPtr dwExtraInfo;
	}

	public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

	[Flags]
	public enum PopFlags
	{
		TF_POPF_ALL = 1
	}

	[Flags]
	public enum CreateContextFlags
	{

	}

	public enum TsGravity
	{
		TS_GR_BACKWARD,
		TS_GR_FORWARD
	}

	public enum TsShiftDir
	{
		TS_SD_BACKWARD,
		TS_SD_FORWARD
	}

	[Flags]
	public enum SetTextFlags
	{
		TS_ST_CORRECTION = 1
	}

	[Flags]
	public enum InsertEmbeddedFlags
	{
		TS_IE_CORRECTION = 1
	}

	[Flags]
	public enum InsertAtSelectionFlags
	{
		TS_IAS_NOQUERY = 1,
		TS_IAS_QUERYONLY = 2
	}

	[Flags]
	public enum AdviseFlags
	{
		TS_AS_TEXT_CHANGE = 1,
		TS_AS_SEL_CHANGE = 2,
		TS_AS_LAYOUT_CHANGE = 4,
		TS_AS_ATTR_CHANGE = 8,
		TS_AS_STATUS_CHANGE = 0x10
	}

	[Flags]
	public enum LockFlags
	{
		TS_LF_SYNC = 1,
		TS_LF_READ = 2,
		TS_LF_WRITE = 4,
		TS_LF_READWRITE = TS_LF_READ | TS_LF_WRITE
	}

	[Flags]
	public enum DynamicStatusFlags
	{
		TS_SD_READONLY = 1,
		TS_SD_LOADING = 2
	}

	[Flags]
	public enum StaticStatusFlags
	{
		TS_SS_DISJOINTSEL = 1,
		TS_SS_REGIONS = 2,
		TS_SS_TRANSITORY = 4,
		TS_SS_NOHIDDENTEXT = 8
	}

	[Flags]
	public enum AttributeFlags
	{
		TS_ATTR_FIND_BACKWARDS = 1,
		TS_ATTR_FIND_WANT_OFFSET = 2,
		TS_ATTR_FIND_UPDATESTART = 4,
		TS_ATTR_FIND_WANT_VALUE = 8,
		TS_ATTR_FIND_WANT_END = 0x10,
		TS_ATTR_FIND_HIDDEN = 0x20
	}

	[Flags]
	public enum GetPositionFromPointFlags
	{
		GXFPF_ROUND_NEAREST = 1,
		GXFPF_NEAREST = 2
	}

	public enum TsActiveSelEnd
	{
		TS_AE_NONE,
		TS_AE_START,
		TS_AE_END
	}

	public enum TsRunType
	{
		TS_RT_PLAIN,
		TS_RT_HIDDEN,
		TS_RT_OPAQUE
	}

	[Flags]
	public enum OnTextChangeFlags
	{
		TS_TC_CORRECTION = 1
	}

	public enum TsLayoutCode
	{
		TS_LC_CREATE,
		TS_LC_CHANGE,
		TS_LC_DESTROY
	}

	public enum TfGravity
	{
		TF_GR_BACKWARD,
		TF_GR_FORWARD
	}

	public enum TfShiftDir
	{
		TF_SD_BACKWARD,
		TF_SD_FORWARD
	}

	public enum TfAnchor
	{
		TF_ANCHOR_START,
		TF_ANCHOR_END
	}

	public enum TF_DA_COLORTYPE
	{
		TF_CT_NONE,
		TF_CT_SYSCOLOR,
		TF_CT_COLORREF
	}

	public enum TF_DA_LINESTYLE
	{
		TF_LS_NONE,
		TF_LS_SOLID,
		TF_LS_DOT,
		TF_LS_DASH,
		TF_LS_SQUIGGLE
	}

	public enum TF_DA_ATTR_INFO
	{
		TF_ATTR_INPUT = 0,
		TF_ATTR_TARGET_CONVERTED = 1,
		TF_ATTR_CONVERTED = 2,
		TF_ATTR_TARGET_NOTCONVERTED = 3,
		TF_ATTR_INPUT_ERROR = 4,
		TF_ATTR_FIXEDCONVERTED = 5,
		TF_ATTR_OTHER = -1
	}

	[Flags]
	public enum GetRenderingMarkupFlags
	{
		TF_GRM_INCLUDE_PROPERTY = 1
	}

	[Flags]
	public enum FindRenderingMarkupFlags
	{
		TF_FRM_INCLUDE_PROPERTY = 1,
		TF_FRM_BACKWARD = 2,
		TF_FRM_NO_CONTAINED = 4,
		TF_FRM_NO_RANGE = 8
	}

	[Flags]
	public enum ConversionModeFlags
	{
		TF_CONVERSIONMODE_ALPHANUMERIC = 0,
		TF_CONVERSIONMODE_NATIVE = 1,
		TF_CONVERSIONMODE_KATAKANA = 2,
		TF_CONVERSIONMODE_FULLSHAPE = 8,
		TF_CONVERSIONMODE_ROMAN = 0x10,
		TF_CONVERSIONMODE_CHARCODE = 0x20,
		TF_CONVERSIONMODE_NOCONVERSION = 0x100,
		TF_CONVERSIONMODE_EUDC = 0x200,
		TF_CONVERSIONMODE_SYMBOL = 0x400,
		TF_CONVERSIONMODE_FIXED = 0x800
	}

	[Flags]
	public enum SentenceModeFlags
	{
		TF_SENTENCEMODE_NONE = 0,
		TF_SENTENCEMODE_PLAURALCLAUSE = 1,
		TF_SENTENCEMODE_SINGLECONVERT = 2,
		TF_SENTENCEMODE_AUTOMATIC = 4,
		TF_SENTENCEMODE_PHRASEPREDICT = 8,
		TF_SENTENCEMODE_CONVERSATION = 0x10
	}

	public enum TfCandidateResult
	{
		CAND_FINALIZED,
		CAND_SELECTED,
		CAND_CANCELED
	}

	public struct POINT
	{
		public int x;

		public int y;
	}

	public struct RECT
	{
		public int left;

		public int top;

		public int right;

		public int bottom;
	}

	public struct TS_STATUS
	{
		public DynamicStatusFlags dynamicFlags;

		public StaticStatusFlags staticFlags;
	}

	public struct TS_SELECTIONSTYLE
	{
		public TsActiveSelEnd ase;

		[MarshalAs(UnmanagedType.Bool)]
		public bool interimChar;
	}

	public struct TS_SELECTION_ACP
	{
		public int start;

		public int end;

		public TS_SELECTIONSTYLE style;
	}

	public struct TS_RUNINFO
	{
		public int count;

		public TsRunType type;
	}

	public struct TS_TEXTCHANGE
	{
		public int start;

		public int oldEnd;

		public int newEnd;
	}

	public struct TS_ATTRVAL
	{
		public Guid attributeId;

		public int overlappedId;

		public int reserved;

		[MarshalAs(UnmanagedType.Struct)]
		public NativeMethods.VARIANT val;
	}

	public struct TF_PRESERVEDKEY
	{
		public int vKey;

		public int modifiers;
	}

	public struct TF_DA_COLOR
	{
		public TF_DA_COLORTYPE type;

		public int indexOrColorRef;
	}

	public struct TF_DISPLAYATTRIBUTE
	{
		public TF_DA_COLOR crText;

		public TF_DA_COLOR crBk;

		public TF_DA_LINESTYLE lsStyle;

		[MarshalAs(UnmanagedType.Bool)]
		public bool fBoldLine;

		public TF_DA_COLOR crLine;

		public TF_DA_ATTR_INFO bAttr;
	}

	public struct TF_RENDERINGMARKUP
	{
		[SecurityCritical]
		public ITfRange range;

		public TF_DISPLAYATTRIBUTE tfDisplayAttr;
	}

	internal struct TF_LANGUAGEPROFILE
	{
		internal Guid clsid;

		internal short langid;

		internal Guid catid;

		[MarshalAs(UnmanagedType.Bool)]
		internal bool fActive;

		internal Guid guidProfile;
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8f1b8ad8-0b6b-4874-90c5-bd76011e8f7c")]
	[SuppressUnmanagedCodeSecurity]
	internal interface ITfMessagePump
	{
		[SecurityCritical]
		void PeekMessageA(ref MSG msg, IntPtr hwnd, int msgFilterMin, int msgFilterMax, int removeMsg, out int result);

		[SecurityCritical]
		void GetMessageA(ref MSG msg, IntPtr hwnd, int msgFilterMin, int msgFilterMax, out int result);

		[SecurityCritical]
		void PeekMessageW(ref MSG msg, IntPtr hwnd, int msgFilterMin, int msgFilterMax, int removeMsg, out int result);

		[SecurityCritical]
		void GetMessageW(ref MSG msg, IntPtr hwnd, int msgFilterMin, int msgFilterMax, out int result);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("e2449660-9542-11d2-bf46-00105a2799b5")]
	public interface ITfProperty
	{
		void GetType(out Guid type);

		[PreserveSig]
		int EnumRanges(int editcookie, out IEnumTfRanges ranges, ITfRange targetRange);

		void GetValue(int editCookie, ITfRange range, out object value);

		void GetContext(out ITfContext context);

		void FindRange(int editCookie, ITfRange inRange, out ITfRange outRange, TfAnchor position);

		void stub_SetValueStore();

		void SetValue(int editCookie, ITfRange range, object value);

		void Clear(int editCookie, ITfRange range);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("aa80e7fd-2021-11d2-93e0-0060b067b86e")]
	public interface ITfContext
	{
		int stub_RequestEditSession();

		void InWriteSession(int clientId, [MarshalAs(UnmanagedType.Bool)] out bool inWriteSession);

		void stub_GetSelection();

		void stub_SetSelection();

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void GetStart(int ec, out ITfRange range);

		void stub_GetEnd();

		void stub_GetActiveView();

		void stub_EnumViews();

		void stub_GetStatus();

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void GetProperty(ref Guid guid, out ITfProperty property);

		void stub_GetAppProperty();

		void stub_TrackProperties();

		void stub_EnumProperties();

		void stub_GetDocumentMgr();

		void stub_CreateRangeBackup();
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("aa80e7f4-2021-11d2-93e0-0060b067b86e")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfDocumentMgr
	{
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void CreateContext(int clientId, CreateContextFlags flags, [MarshalAs(UnmanagedType.Interface)] object obj, out ITfContext context, out int editCookie);

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void Push(ITfContext context);

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void Pop(PopFlags flags);

		void GetTop(out ITfContext context);

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void GetBase(out ITfContext context);

		void EnumContexts([MarshalAs(UnmanagedType.Interface)] out object enumContexts);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("aa80e808-2021-11d2-93e0-0060b067b86e")]
	[SuppressUnmanagedCodeSecurity]
	public interface IEnumTfDocumentMgrs
	{
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("101d6610-0990-11d3-8df0-00105a2799b5")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfFunctionProvider
	{
		void GetType(out Guid guid);

		void GetDescription([MarshalAs(UnmanagedType.BStr)] out string desc);

		[PreserveSig]
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		int GetFunction(ref Guid guid, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object obj);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("db593490-098f-11d3-8df0-00105a2799b5")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfFunction
	{
		void GetDisplayName([MarshalAs(UnmanagedType.BStr)] out string funcName);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("581f317e-fd9d-443f-b972-ed00467c5d40")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfCandidateString
	{
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void GetString([MarshalAs(UnmanagedType.BStr)] out string funcName);

		void GetIndex(out int nIndex);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("a3ad50fb-9bdb-49e3-a843-6c76520fbf5d")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfCandidateList
	{
		void EnumCandidates(out object enumCand);

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void GetCandidate(int nIndex, out ITfCandidateString candstring);

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void GetCandidateNum(out int nCount);

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void SetResult(int nIndex, TfCandidateResult result);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("4cea93c0-0a58-11d3-8df0-00105a2799b5")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfFnReconversion
	{
		void GetDisplayName([MarshalAs(UnmanagedType.BStr)] out string funcName);

		[PreserveSig]
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		int QueryRange(ITfRange range, out ITfRange newRange, [MarshalAs(UnmanagedType.Bool)] out bool isConvertable);

		[PreserveSig]
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		int GetReconversion(ITfRange range, out ITfCandidateList candList);

		[PreserveSig]
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		int Reconvert(ITfRange range);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("88f567c6-1757-49f8-a1b2-89234c1eeff9")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfFnConfigure
	{
		void GetDisplayName([MarshalAs(UnmanagedType.BStr)] out string funcName);

		[PreserveSig]
		int Show(IntPtr hwndParent, short langid, ref Guid guidProfile);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("bb95808a-6d8f-4bca-8400-5390b586aedf")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfFnConfigureRegisterWord
	{
		void GetDisplayName([MarshalAs(UnmanagedType.BStr)] out string funcName);

		[PreserveSig]
		int Show(IntPtr hwndParent, short langid, ref Guid guidProfile, [MarshalAs(UnmanagedType.BStr)] string bstrRegistered);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("e4b24db0-0990-11d3-8df0-00105a2799b5")]
	[SuppressUnmanagedCodeSecurity]
	public interface IEnumTfFunctionProviders
	{
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("bb08f7a9-607a-4384-8623-056892b64371")]
	public interface ITfCompartment
	{
		[PreserveSig]
		[SecurityCritical]
		int SetValue(int tid, ref object varValue);

		[SecurityCritical]
		void GetValue(out object varValue);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("743abd5f-f26d-48df-8cc5-238492419b64")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfCompartmentEventSink
	{
		void OnChange(ref Guid rguid);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7dcf57ac-18ad-438b-824d-979bffb74b7c")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfCompartmentMgr
	{
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void GetCompartment(ref Guid guid, out ITfCompartment comp);

		void ClearCompartment(int tid, Guid guid);

		void EnumCompartments(out object enumGuid);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("aa80e801-2021-11d2-93e0-0060b067b86e")]
	[SuppressUnmanagedCodeSecurity]
	internal interface ITfThreadMgr
	{
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void Activate(out int clientId);

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void Deactivate();

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void CreateDocumentMgr(out ITfDocumentMgr docMgr);

		void EnumDocumentMgrs(out IEnumTfDocumentMgrs enumDocMgrs);

		void GetFocus(out ITfDocumentMgr docMgr);

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void SetFocus(ITfDocumentMgr docMgr);

		void AssociateFocus(IntPtr hwnd, ITfDocumentMgr newDocMgr, out ITfDocumentMgr prevDocMgr);

		void IsThreadFocus([MarshalAs(UnmanagedType.Bool)] out bool isFocus);

		[PreserveSig]
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		int GetFunctionProvider(ref Guid classId, out ITfFunctionProvider funcProvider);

		void EnumFunctionProviders(out IEnumTfFunctionProviders enumProviders);

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void GetGlobalCompartment(out ITfCompartmentMgr compartmentMgr);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("28888fe3-c2a0-483a-a3ea-8cb1ce51ff3d")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITextStoreACP
	{
		void AdviseSink(ref Guid riid, [MarshalAs(UnmanagedType.Interface)] object obj, AdviseFlags flags);

		void UnadviseSink([MarshalAs(UnmanagedType.Interface)] object obj);

		void RequestLock(LockFlags flags, out int hrSession);

		void GetStatus(out TS_STATUS status);

		void QueryInsert(int start, int end, int cch, out int startResult, out int endResult);

		void GetSelection(int index, int count, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] TS_SELECTION_ACP[] selection, out int fetched);

		void SetSelection(int count, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] TS_SELECTION_ACP[] selection);

		void GetText(int start, int end, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] char[] text, int cchReq, out int charsCopied, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 6)] TS_RUNINFO[] runInfo, int cRunInfoReq, out int cRunInfoRcv, out int nextCp);

		void SetText(SetTextFlags flags, int start, int end, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] char[] text, int cch, out TS_TEXTCHANGE change);

		void GetFormattedText(int start, int end, [MarshalAs(UnmanagedType.Interface)] out object obj);

		void GetEmbedded(int position, ref Guid guidService, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object obj);

		void QueryInsertEmbedded(ref Guid guidService, int formatEtc, [MarshalAs(UnmanagedType.Bool)] out bool insertable);

		void InsertEmbedded(InsertEmbeddedFlags flags, int start, int end, [MarshalAs(UnmanagedType.Interface)] object obj, out TS_TEXTCHANGE change);

		void InsertTextAtSelection(InsertAtSelectionFlags flags, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] char[] text, int cch, out int start, out int end, out TS_TEXTCHANGE change);

		void InsertEmbeddedAtSelection(InsertAtSelectionFlags flags, [MarshalAs(UnmanagedType.Interface)] object obj, out int start, out int end, out TS_TEXTCHANGE change);

		[PreserveSig]
		int RequestSupportedAttrs(AttributeFlags flags, int count, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] Guid[] filterAttributes);

		[PreserveSig]
		int RequestAttrsAtPosition(int position, int count, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] Guid[] filterAttributes, AttributeFlags flags);

		void RequestAttrsTransitioningAtPosition(int position, int count, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] Guid[] filterAttributes, AttributeFlags flags);

		void FindNextAttrTransition(int start, int halt, int count, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] Guid[] filterAttributes, AttributeFlags flags, out int acpNext, [MarshalAs(UnmanagedType.Bool)] out bool found, out int foundOffset);

		void RetrieveRequestedAttrs(int count, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] TS_ATTRVAL[] attributeVals, out int countFetched);

		void GetEnd(out int end);

		void GetActiveView(out int viewCookie);

		void GetACPFromPoint(int viewCookie, ref POINT point, GetPositionFromPointFlags flags, out int position);

		void GetTextExt(int viewCookie, int start, int end, out RECT rect, [MarshalAs(UnmanagedType.Bool)] out bool clipped);

		void GetScreenExt(int viewCookie, out RECT rect);

		void GetWnd(int viewCookie, out IntPtr hwnd);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("22d44c94-a419-4542-a272-ae26093ececf")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITextStoreACPSink
	{
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		void OnTextChange(OnTextChangeFlags flags, ref TS_TEXTCHANGE change);

		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		void OnSelectionChange();

		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		void OnLayoutChange(TsLayoutCode lcode, int viewCookie);

		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		void OnStatusChange(DynamicStatusFlags flags);

		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		void OnAttrsChange(int start, int end, int count, Guid[] attributes);

		[PreserveSig]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		int OnLockGranted(LockFlags flags);

		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		void OnStartEditTransaction();

		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		void OnEndEditTransaction();
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("c0f1db0c-3a20-405c-a303-96b6010a885f")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfThreadFocusSink
	{
		void OnSetThreadFocus();

		void OnKillThreadFocus();
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("4ea48a35-60ae-446f-8fd6-e6a8d82459f7")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfSource
	{
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void AdviseSink(ref Guid riid, [MarshalAs(UnmanagedType.Interface)] object obj, out int cookie);

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void UnadviseSink(int cookie);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("aa80e7f0-2021-11d2-93e0-0060b067b86e")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfKeystrokeMgr
	{
		void AdviseKeyEventSink(int clientId, [MarshalAs(UnmanagedType.Interface)] object obj, [MarshalAs(UnmanagedType.Bool)] bool fForeground);

		void UnadviseKeyEventSink(int clientId);

		void GetForeground(out Guid clsid);

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void TestKeyDown(int wParam, int lParam, [MarshalAs(UnmanagedType.Bool)] out bool eaten);

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void TestKeyUp(int wParam, int lParam, [MarshalAs(UnmanagedType.Bool)] out bool eaten);

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void KeyDown(int wParam, int lParam, [MarshalAs(UnmanagedType.Bool)] out bool eaten);

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void KeyUp(int wParam, int lParam, [MarshalAs(UnmanagedType.Bool)] out bool eaten);

		void GetPreservedKey(ITfContext context, ref TF_PRESERVEDKEY key, out Guid guid);

		void IsPreservedKey(ref Guid guid, ref TF_PRESERVEDKEY key, [MarshalAs(UnmanagedType.Bool)] out bool registered);

		void PreserveKey(int clientId, ref Guid guid, ref TF_PRESERVEDKEY key, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] char[] desc, int descCount);

		void UnpreserveKey(ref Guid guid, ref TF_PRESERVEDKEY key);

		void SetPreservedKeyDescription(ref Guid guid, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] char[] desc, int descCount);

		void GetPreservedKeyDescription(ref Guid guid, [MarshalAs(UnmanagedType.BStr)] out string desc);

		void SimulatePreservedKey(ITfContext context, ref Guid guid, [MarshalAs(UnmanagedType.Bool)] out bool eaten);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("aa80e7ff-2021-11d2-93e0-0060b067b86e")]
	public interface ITfRange
	{
		[SecurityCritical]
		void GetText(int ec, int flags, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] char[] text, int countMax, out int count);

		[SecurityCritical]
		void SetText(int ec, int flags, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] char[] text, int count);

		[SecurityCritical]
		void GetFormattedText(int ec, [MarshalAs(UnmanagedType.Interface)] out object data);

		[SecurityCritical]
		void GetEmbedded(int ec, ref Guid guidService, ref Guid iid, [MarshalAs(UnmanagedType.Interface)] out object obj);

		[SecurityCritical]
		void InsertEmbedded(int ec, int flags, [MarshalAs(UnmanagedType.Interface)] object data);

		[SecurityCritical]
		void ShiftStart(int ec, int count, out int result, int ZeroForNow);

		[SecurityCritical]
		void ShiftEnd(int ec, int count, out int result, int ZeroForNow);

		[SecurityCritical]
		void ShiftStartToRange(int ec, ITfRange range, TfAnchor position);

		[SecurityCritical]
		void ShiftEndToRange(int ec, ITfRange range, TfAnchor position);

		[SecurityCritical]
		void ShiftStartRegion(int ec, TfShiftDir dir, [MarshalAs(UnmanagedType.Bool)] out bool noRegion);

		[SecurityCritical]
		void ShiftEndRegion(int ec, TfShiftDir dir, [MarshalAs(UnmanagedType.Bool)] out bool noRegion);

		[SecurityCritical]
		void IsEmpty(int ec, [MarshalAs(UnmanagedType.Bool)] out bool empty);

		[SecurityCritical]
		void Collapse(int ec, TfAnchor position);

		[SecurityCritical]
		void IsEqualStart(int ec, ITfRange with, TfAnchor position, [MarshalAs(UnmanagedType.Bool)] out bool equal);

		[SecurityCritical]
		void IsEqualEnd(int ec, ITfRange with, TfAnchor position, [MarshalAs(UnmanagedType.Bool)] out bool equal);

		[SecurityCritical]
		void CompareStart(int ec, ITfRange with, TfAnchor position, out int result);

		[SecurityCritical]
		void CompareEnd(int ec, ITfRange with, TfAnchor position, out int result);

		[SecurityCritical]
		void AdjustForInsert(int ec, int count, [MarshalAs(UnmanagedType.Bool)] out bool insertOk);

		[SecurityCritical]
		void GetGravity(out TfGravity start, out TfGravity end);

		[SecurityCritical]
		void SetGravity(int ec, TfGravity start, TfGravity end);

		[SecurityCritical]
		void Clone(out ITfRange clone);

		[SecurityCritical]
		void GetContext(out ITfContext context);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("057a6296-029b-4154-b79a-0d461d4ea94c")]
	public interface ITfRangeACP
	{
		[SecurityCritical]
		void GetText(int ec, int flags, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] char[] text, int countMax, out int count);

		[SecurityCritical]
		void SetText(int ec, int flags, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] char[] text, int count);

		[SecurityCritical]
		void GetFormattedText(int ec, [MarshalAs(UnmanagedType.Interface)] out object data);

		[SecurityCritical]
		void GetEmbedded(int ec, ref Guid guidService, ref Guid iid, [MarshalAs(UnmanagedType.Interface)] out object obj);

		[SecurityCritical]
		void InsertEmbedded(int ec, int flags, [MarshalAs(UnmanagedType.Interface)] object data);

		[SecurityCritical]
		void ShiftStart(int ec, int count, out int result, int ZeroForNow);

		[SecurityCritical]
		void ShiftEnd(int ec, int count, out int result, int ZeroForNow);

		[SecurityCritical]
		void ShiftStartToRange(int ec, ITfRange range, TfAnchor position);

		[SecurityCritical]
		void ShiftEndToRange(int ec, ITfRange range, TfAnchor position);

		[SecurityCritical]
		void ShiftStartRegion(int ec, TfShiftDir dir, [MarshalAs(UnmanagedType.Bool)] out bool noRegion);

		[SecurityCritical]
		void ShiftEndRegion(int ec, TfShiftDir dir, [MarshalAs(UnmanagedType.Bool)] out bool noRegion);

		[SecurityCritical]
		void IsEmpty(int ec, [MarshalAs(UnmanagedType.Bool)] out bool empty);

		[SecurityCritical]
		void Collapse(int ec, TfAnchor position);

		[SecurityCritical]
		void IsEqualStart(int ec, ITfRange with, TfAnchor position, [MarshalAs(UnmanagedType.Bool)] out bool equal);

		[SecurityCritical]
		void IsEqualEnd(int ec, ITfRange with, TfAnchor position, [MarshalAs(UnmanagedType.Bool)] out bool equal);

		[SecurityCritical]
		void CompareStart(int ec, ITfRange with, TfAnchor position, out int result);

		[SecurityCritical]
		void CompareEnd(int ec, ITfRange with, TfAnchor position, out int result);

		[SecurityCritical]
		void AdjustForInsert(int ec, int count, [MarshalAs(UnmanagedType.Bool)] out bool insertOk);

		[SecurityCritical]
		void GetGravity(out TfGravity start, out TfGravity end);

		[SecurityCritical]
		void SetGravity(int ec, TfGravity start, TfGravity end);

		[SecurityCritical]
		void Clone(out ITfRange clone);

		[SecurityCritical]
		void GetContext(out ITfContext context);

		[SecurityCritical]
		void GetExtent(out int start, out int count);

		[SecurityCritical]
		void SetExtent(int start, int count);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("D7540241-F9A1-4364-BEFC-DBCD2C4395B7")]
	public interface ITfCompositionView
	{
		[SecurityCritical]
		void GetOwnerClsid(out Guid clsid);

		[SecurityCritical]
		void GetRange(out ITfRange range);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("5F20AA40-B57A-4F34-96AB-3576F377CC79")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfContextOwnerCompositionSink
	{
		void OnStartComposition(ITfCompositionView view, [MarshalAs(UnmanagedType.Bool)] out bool ok);

		void OnUpdateComposition(ITfCompositionView view, ITfRange rangeNew);

		void OnEndComposition(ITfCompositionView view);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("D40C8AAE-AC92-4FC7-9A11-0EE0E23AA39B")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfContextComposition
	{
		void StartComposition(int ecWrite, ITfRange range, [MarshalAs(UnmanagedType.Interface)] object sink, [MarshalAs(UnmanagedType.Interface)] out object composition);

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void EnumCompositions([MarshalAs(UnmanagedType.Interface)] out IEnumITfCompositionView enumView);

		void FindComposition(int ecRead, ITfRange testRange, [MarshalAs(UnmanagedType.Interface)] out object enumView);

		void TakeOwnership(int ecWrite, ITfCompositionView view, [MarshalAs(UnmanagedType.Interface)] object sink, [MarshalAs(UnmanagedType.Interface)] out object composition);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("86462810-593B-4916-9764-19C08E9CE110")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfContextOwnerCompositionServices
	{
		void StartComposition(int ecWrite, ITfRange range, [MarshalAs(UnmanagedType.Interface)] object sink, [MarshalAs(UnmanagedType.Interface)] out object composition);

		void EnumCompositions([MarshalAs(UnmanagedType.Interface)] out object enumView);

		void FindComposition(int ecRead, ITfRange testRange, [MarshalAs(UnmanagedType.Interface)] out object enumView);

		void TakeOwnership(int ecWrite, ITfCompositionView view, [MarshalAs(UnmanagedType.Interface)] object sink, [MarshalAs(UnmanagedType.Interface)] out object composition);

		[PreserveSig]
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		int TerminateComposition(ITfCompositionView view);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("5EFD22BA-7838-46CB-88E2-CADB14124F8F")]
	[SuppressUnmanagedCodeSecurity]
	internal interface IEnumITfCompositionView
	{
		void Clone(out IEnumTfRanges ranges);

		[PreserveSig]
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		int Next(int count, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] ITfCompositionView[] compositionview, out int fetched);

		void Reset();

		[PreserveSig]
		int Skip(int count);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("f99d3f40-8e32-11d2-bf46-00105a2799b5")]
	public interface IEnumTfRanges
	{
		[SecurityCritical]
		void Clone(out IEnumTfRanges ranges);

		[PreserveSig]
		[SecurityCritical]
		int Next(int count, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] ITfRange[] ranges, out int fetched);

		[SecurityCritical]
		void Reset();

		[PreserveSig]
		[SecurityCritical]
		int Skip(int count);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("42d4d099-7c1a-4a89-b836-6c6f22160df0")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfEditRecord
	{
		void GetSelectionStatus([MarshalAs(UnmanagedType.Bool)] out bool selectionChanged);

		void GetTextAndPropertyUpdates(int flags, ref IntPtr properties, int count, out IEnumTfRanges ranges);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8127d409-ccd3-4683-967a-b43d5b482bf7")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfTextEditSink
	{
		void OnEndEdit(ITfContext context, int ecReadOnly, ITfEditRecord editRecord);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8c03d21b-95a7-4ba0-ae1b-7fce12a72930")]
	[SuppressUnmanagedCodeSecurity]
	public interface IEnumTfRenderingMarkup
	{
		void Clone(out IEnumTfRenderingMarkup clone);

		[PreserveSig]
		int Next(int count, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] TF_RENDERINGMARKUP[] markup, out int fetched);

		void Reset();

		[PreserveSig]
		int Skip(int count);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("a305b1c0-c776-4523-bda0-7c5a2e0fef10")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfContextRenderingMarkup
	{
		void GetRenderingMarkup(int editCookie, GetRenderingMarkupFlags flags, ITfRange range, out IEnumTfRenderingMarkup enumMarkup);

		void FindNextRenderingMarkup(int editCookie, FindRenderingMarkupFlags flags, ITfRange queryRange, TfAnchor queryAnchor, out ITfRange foundRange, out TF_RENDERINGMARKUP foundMarkup);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("1F02B6C5-7842-4EE6-8A0B-9A24183A95CA")]
	public interface ITfInputProcessorProfiles
	{
		[SecurityCritical]
		void stub_Register();

		[SecurityCritical]
		void stub_Unregister();

		[SecurityCritical]
		void stub_AddLanguageProfile();

		[SecurityCritical]
		void stub_RemoveLanguageProfile();

		[SecurityCritical]
		void stub_EnumInputProcessorInfo();

		[SecurityCritical]
		void stub_GetDefaultLanguageProfile();

		[SecurityCritical]
		void stub_SetDefaultLanguageProfile();

		[SecurityCritical]
		void ActivateLanguageProfile(ref Guid clsid, short langid, ref Guid guidProfile);

		[PreserveSig]
		[SecurityCritical]
		int GetActiveLanguageProfile(ref Guid clsid, out short langid, out Guid profile);

		[SecurityCritical]
		void stub_GetLanguageProfileDescription();

		[SecurityCritical]
		void GetCurrentLanguage(out short langid);

		[PreserveSig]
		[SecurityCritical]
		int ChangeCurrentLanguage(short langid);

		[PreserveSig]
		[SecurityCritical]
		int GetLanguageList(out IntPtr langids, out int count);

		[SecurityCritical]
		void EnumLanguageProfiles(short langid, out IEnumTfLanguageProfiles enumIPP);

		[SecurityCritical]
		void stub_EnableLanguageProfile();

		[SecurityCritical]
		void stub_IsEnabledLanguageProfile();

		[SecurityCritical]
		void stub_EnableLanguageProfileByDefault();

		[SecurityCritical]
		void stub_SubstituteKeyboardLayout();
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("3d61bf11-ac5f-42c8-a4cb-931bcc28c744")]
	[SuppressUnmanagedCodeSecurity]
	internal interface IEnumTfLanguageProfiles
	{
		void Clone(out IEnumTfLanguageProfiles enumIPP);

		[PreserveSig]
		int Next(int count, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] TF_LANGUAGEPROFILE[] profiles, out int fetched);

		void Reset();

		void Skip(int count);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("43c9fe15-f494-4c17-9de2-b8a4ac350aa8")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfLanguageProfileNotifySink
	{
		void OnLanguageChange(short langid, [MarshalAs(UnmanagedType.Bool)] out bool bAccept);

		void OnLanguageChanged();
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8ded7393-5db1-475c-9e71-a39111b0ff67")]
	public interface ITfDisplayAttributeMgr
	{
		[SecurityCritical]
		void OnUpdateInfo();

		[SecurityCritical]
		void stub_EnumDisplayAttributeInfo();

		[SecurityCritical]
		void GetDisplayAttributeInfo(ref Guid guid, out ITfDisplayAttributeInfo info, out Guid clsid);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("70528852-2f26-4aea-8c96-215150578932")]
	public interface ITfDisplayAttributeInfo
	{
		[SecurityCritical]
		void stub_GetGUID();

		[SecurityCritical]
		void stub_GetDescription();

		[SecurityCritical]
		void GetAttributeInfo(out TF_DISPLAYATTRIBUTE attr);

		[SecurityCritical]
		void stub_SetAttributeInfo();

		[SecurityCritical]
		void stub_Reset();
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("c3acefb5-f69d-4905-938f-fcadcf4be830")]
	public interface ITfCategoryMgr
	{
		[SecurityCritical]
		void stub_RegisterCategory();

		[SecurityCritical]
		void stub_UnregisterCategory();

		[SecurityCritical]
		void stub_EnumCategoriesInItem();

		[SecurityCritical]
		void stub_EnumItemsInCategory();

		[SecurityCritical]
		void stub_FindClosestCategory();

		[SecurityCritical]
		void stub_RegisterGUIDDescription();

		[SecurityCritical]
		void stub_UnregisterGUIDDescription();

		[SecurityCritical]
		void stub_GetGUIDDescription();

		[SecurityCritical]
		void stub_RegisterGUIDDWORD();

		[SecurityCritical]
		void stub_UnregisterGUIDDWORD();

		[SecurityCritical]
		void stub_GetGUIDDWORD();

		[SecurityCritical]
		void stub_RegisterGUID();

		[PreserveSig]
		[SecurityCritical]
		int GetGUID(int guidatom, out Guid guid);

		[SecurityCritical]
		void stub_IsEqualTfGuidAtom();
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("aa80e80c-2021-11d2-93e0-0060b067b86e")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfContextOwner
	{
		void GetACPFromPoint(ref POINT point, GetPositionFromPointFlags flags, out int position);

		void GetTextExt(int start, int end, out RECT rect, [MarshalAs(UnmanagedType.Bool)] out bool clipped);

		void GetScreenExt(out RECT rect);

		void GetStatus(out TS_STATUS status);

		void GetWnd(out IntPtr hwnd);

		void GetValue(ref Guid guidAttribute, out object varValue);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("b23eb630-3e1c-11d3-a745-0050040ab407")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfContextOwnerServices
	{
		void stub_OnLayoutChange();

		void stub_OnStatusChange();

		void stub_OnAttributeChange();

		void stub_Serialize();

		void stub_Unserialize();

		void stub_ForceLoadProperty();

		void CreateRange(int acpStart, int acpEnd, out ITfRangeACP range);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("a615096f-1c57-4813-8a15-55ee6e5a839c")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfTransitoryExtensionSink
	{
		void OnTransitoryExtensionUpdated(ITfContext context, int ecReadOnly, ITfRange rangeResult, ITfRange rangeComposition, [MarshalAs(UnmanagedType.Bool)] out bool fDeleteResultRange);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("fde1eaee-6924-4cdf-91e7-da38cff5559d")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfInputScope
	{
		void GetInputScopes(out IntPtr ppinputscopes, out int count);

		[PreserveSig]
		int GetPhrase(out IntPtr ppbstrPhrases, out int count);

		[PreserveSig]
		int GetRegularExpression([MarshalAs(UnmanagedType.BStr)] out string desc);

		[PreserveSig]
		int GetSRGC([MarshalAs(UnmanagedType.BStr)] out string desc);

		[PreserveSig]
		int GetXML([MarshalAs(UnmanagedType.BStr)] out string desc);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("3bdd78e2-c16e-47fd-b883-ce6facc1a208")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfMouseTrackerACP
	{
		[PreserveSig]
		int AdviceMouseSink(ITfRangeACP range, ITfMouseSink sink, out int dwCookie);

		[PreserveSig]
		int UnadviceMouseSink(int dwCookie);
	}

	[ComImport]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("a1adaaa2-3a24-449d-ac96-5183e7f5c217")]
	[SuppressUnmanagedCodeSecurity]
	public interface ITfMouseSink
	{
		[PreserveSig]
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		int OnMouseEvent(int edge, int quadrant, int btnStatus, [MarshalAs(UnmanagedType.Bool)] out bool eaten);
	}

	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	internal class WIC
	{
		internal enum WICBitmapTransformOptions
		{
			WICBitmapTransformRotate0 = 0,
			WICBitmapTransformRotate90 = 1,
			WICBitmapTransformRotate180 = 2,
			WICBitmapTransformRotate270 = 3,
			WICBitmapTransformFlipHorizontal = 8,
			WICBitmapTransformFlipVertical = 16
		}

		internal enum WICPaletteType
		{
			WICPaletteTypeCustom = 0,
			WICPaletteTypeOptimal = 1,
			WICPaletteTypeFixedBW = 2,
			WICPaletteTypeFixedHalftone8 = 3,
			WICPaletteTypeFixedHalftone27 = 4,
			WICPaletteTypeFixedHalftone64 = 5,
			WICPaletteTypeFixedHalftone125 = 6,
			WICPaletteTypeFixedHalftone216 = 7,
			WICPaletteTypeFixedWebPalette = WICPaletteTypeFixedHalftone216,
			WICPaletteTypeFixedHalftone252 = 8,
			WICPaletteTypeFixedHalftone256 = 9,
			WICPaletteTypeFixedGray4 = 10,
			WICPaletteTypeFixedGray16 = 11,
			WICPaletteTypeFixedGray256 = 12
		}

		internal const int WINCODEC_SDK_VERSION = 566;

		internal static readonly Guid WICPixelFormat32bppPBGRA;

		[DllImport("WindowsCodecs.dll", EntryPoint = "WICCreateImagingFactory_Proxy")]
		internal static extern int CreateImagingFactory(uint SDKVersion, out IntPtr ppICodecFactory);

		[DllImport("WindowsCodecs.dll", EntryPoint = "IWICImagingFactory_CreateStream_Proxy")]
		internal static extern int CreateStream(IntPtr pICodecFactory, out IntPtr ppIStream);

		[DllImport("WindowsCodecs.dll", EntryPoint = "IWICStream_InitializeFromMemory_Proxy")]
		internal static extern int InitializeStreamFromMemory(IntPtr pIWICStream, IntPtr pbBuffer, uint cbSize);

		[DllImport("WindowsCodecs.dll", EntryPoint = "IWICImagingFactory_CreateDecoderFromStream_Proxy")]
		internal static extern int CreateDecoderFromStream(IntPtr pICodecFactory, IntPtr pIStream, ref Guid guidVendor, uint metadataFlags, out IntPtr ppIDecode);

		[DllImport("WindowsCodecs.dll", EntryPoint = "IWICBitmapDecoder_GetFrame_Proxy")]
		internal static extern int GetFrame(IntPtr THIS_PTR, uint index, out IntPtr ppIFrameDecode);

		[DllImport("WindowsCodecs.dll", EntryPoint = "IWICImagingFactory_CreateFormatConverter_Proxy")]
		internal static extern int CreateFormatConverter(IntPtr pICodecFactory, out IntPtr ppFormatConverter);

		[DllImport("WindowsCodecs.dll", EntryPoint = "IWICFormatConverter_Initialize_Proxy")]
		internal static extern int InitializeFormatConverter(IntPtr THIS_PTR, IntPtr source, ref Guid dstFormat, int dither, IntPtr bitmapPalette, double alphaThreshold, WICPaletteType paletteTranslate);

		[DllImport("WindowsCodecs.dll", EntryPoint = "IWICImagingFactory_CreateBitmapFlipRotator_Proxy")]
		internal static extern int CreateBitmapFlipRotator(IntPtr pICodecFactory, out IntPtr ppBitmapFlipRotator);

		[DllImport("WindowsCodecs.dll", EntryPoint = "IWICBitmapFlipRotator_Initialize_Proxy")]
		internal static extern int InitializeBitmapFlipRotator(IntPtr THIS_PTR, IntPtr source, WICBitmapTransformOptions options);

		[DllImport("WindowsCodecs.dll", EntryPoint = "IWICBitmapSource_GetSize_Proxy")]
		internal static extern int GetBitmapSize(IntPtr THIS_PTR, out int puiWidth, out int puiHeight);

		[DllImport("WindowsCodecs.dll", EntryPoint = "IWICBitmapSource_CopyPixels_Proxy")]
		internal static extern int CopyPixels(IntPtr THIS_PTR, ref Int32Rect prc, int cbStride, int cbBufferSize, IntPtr pvPixels);

		public WIC()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		static WIC()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal class HRESULT
	{
		[SecuritySafeCritical]
		public static void Check(int hr)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		public HRESULT()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public const int MB_PRECOMPOSED = 1;

	public const int MB_COMPOSITE = 2;

	public const int MB_USEGLYPHCHARS = 4;

	public const int MB_ERR_INVALID_CHARS = 8;

	public const int WAIT_FAILED = -1;

	internal const uint INTERNET_COOKIE_THIRD_PARTY = 16u;

	internal const uint INTERNET_COOKIE_EVALUATE_P3P = 64u;

	internal const uint INTERNET_COOKIE_IS_RESTRICTED = 512u;

	internal const uint COOKIE_STATE_REJECT = 5u;

	public const int PROCESS_VM_READ = 16;

	public const int PROCESS_QUERY_INFORMATION = 1024;

	internal static readonly IntPtr INVALID_HANDLE_VALUE;

	internal const int PAGE_NOACCESS = 1;

	internal const int PAGE_READONLY = 2;

	internal const int PAGE_READWRITE = 4;

	internal const int PAGE_WRITECOPY = 8;

	internal const int PAGE_EXECUTE = 16;

	internal const int PAGE_EXECUTE_READ = 32;

	internal const int PAGE_EXECUTE_READWRITE = 64;

	internal const int PAGE_EXECUTE_WRITECOPY = 128;

	internal const int PAGE_GUARD = 256;

	internal const int PAGE_NOCACHE = 512;

	internal const int PAGE_WRITECOMBINE = 1024;

	internal const int MEM_COMMIT = 4096;

	internal const int MEM_RESERVE = 8192;

	internal const int MEM_DECOMMIT = 16384;

	internal const int MEM_RELEASE = 32768;

	internal const int MEM_FREE = 65536;

	internal const int MEM_PRIVATE = 131072;

	internal const int MEM_MAPPED = 262144;

	internal const int MEM_RESET = 524288;

	internal const int MEM_TOP_DOWN = 1048576;

	internal const int MEM_WRITE_WATCH = 2097152;

	internal const int MEM_PHYSICAL = 4194304;

	internal const int MEM_4MB_PAGES = int.MinValue;

	internal const int SEC_FILE = 8388608;

	internal const int SEC_IMAGE = 16777216;

	internal const int SEC_RESERVE = 67108864;

	internal const int SEC_COMMIT = 134217728;

	internal const int SEC_NOCACHE = 268435456;

	internal const int MEM_IMAGE = 16777216;

	internal const int WRITE_WATCH_FLAG_RESET = 1;

	internal const int SECTION_ALL_ACCESS = 983071;

	internal const int STANDARD_RIGHTS_REQUIRED = 983040;

	internal const int SECTION_QUERY = 1;

	internal const int SECTION_MAP_WRITE = 2;

	internal const int SECTION_MAP_READ = 4;

	internal const int SECTION_MAP_EXECUTE = 8;

	internal const int SECTION_EXTEND_SIZE = 16;

	internal const int FILE_MAP_COPY = 1;

	internal const int FILE_MAP_WRITE = 2;

	internal const int FILE_MAP_READ = 4;

	internal const int FILE_MAP_ALL_ACCESS = 983071;

	internal const int SDDL_REVISION_1 = 1;

	internal const int SDDL_REVISION = 1;

	public const int EventObjectUIFragmentCreate = 1879048191;

	internal const int STATUS_SUCCESS = 0;

	internal const int STATUS_TIMEOUT = 258;

	internal const int STATUS_BUFFER_TOO_SMALL = -1073741789;

	public const int DUPLICATE_CLOSE_SOURCE = 1;

	public const int DUPLICATE_SAME_ACCESS = 2;

	public const int TF_CLIENTID_NULL = 0;

	public const char TS_CHAR_EMBEDDED = '￼';

	public const char TS_CHAR_REGION = '\0';

	public const char TS_CHAR_REPLACEMENT = '\ufffd';

	public const int TS_DEFAULT_SELECTION = -1;

	public const int TS_S_ASYNC = 262912;

	public const int TS_E_NOSELECTION = -2147220987;

	public const int TS_E_NOLAYOUT = -2147220986;

	public const int TS_E_INVALIDPOINT = -2147220985;

	public const int TS_E_SYNCHRONOUS = -2147220984;

	public const int TS_E_READONLY = -2147220983;

	public const int TS_E_FORMAT = -2147220982;

	public const int TF_INVALID_COOKIE = -1;

	public const int TF_DICTATION_ON = 1;

	public const int TF_COMMANDING_ON = 8;

	public static readonly Guid IID_ITextStoreACPSink;

	public static readonly Guid IID_ITfThreadFocusSink;

	public static readonly Guid IID_ITfTextEditSink;

	public static readonly Guid IID_ITfLanguageProfileNotifySink;

	public static readonly Guid IID_ITfCompartmentEventSink;

	public static readonly Guid IID_ITfTransitoryExtensionSink;

	public static readonly Guid GUID_TFCAT_TIP_KEYBOARD;

	public static readonly Guid GUID_PROP_ATTRIBUTE;

	public static readonly Guid GUID_PROP_LANGID;

	public static readonly Guid GUID_PROP_READING;

	public static readonly Guid GUID_PROP_INPUTSCOPE;

	public static readonly Guid GUID_COMPARTMENT_KEYBOARD_DISABLED;

	public static Guid GUID_COMPARTMENT_KEYBOARD_OPENCLOSE;

	public static readonly Guid GUID_COMPARTMENT_HANDWRITING_OPENCLOSE;

	public static readonly Guid GUID_COMPARTMENT_SPEECH_DISABLED;

	public static readonly Guid GUID_COMPARTMENT_SPEECH_OPENCLOSE;

	public static readonly Guid GUID_COMPARTMENT_SPEECH_GLOBALSTATE;

	public static readonly Guid GUID_COMPARTMENT_KEYBOARD_INPUTMODE_CONVERSION;

	public static readonly Guid GUID_COMPARTMENT_KEYBOARD_INPUTMODE_SENTENCE;

	public static readonly Guid GUID_COMPARTMENT_TRANSITORYEXTENSION;

	public static readonly Guid GUID_COMPARTMENT_TRANSITORYEXTENSION_DOCUMENTMANAGER;

	public static readonly Guid GUID_COMPARTMENT_TRANSITORYEXTENSION_PARENT;

	public static readonly Guid Clsid_SpeechTip;

	public static readonly Guid Guid_Null;

	public static readonly Guid IID_ITfFnCustomSpeechCommand;

	public static readonly Guid IID_ITfFnReconversion;

	public static readonly Guid IID_ITfFnConfigure;

	public static readonly Guid IID_ITfFnConfigureRegisterWord;

	public static readonly Guid TSATTRID_Font_FaceName;

	public static readonly Guid TSATTRID_Font_SizePts;

	public static readonly Guid TSATTRID_Font_Style_Height;

	public static readonly Guid TSATTRID_Text_VerticalWriting;

	public static readonly Guid TSATTRID_Text_Orientation;

	public static readonly Guid TSATTRID_Text_ReadOnly;

	public static readonly Guid GUID_SYSTEM_FUNCTIONPROVIDER;

	[SecurityCritical]
	public static object PtrToStructure(IntPtr lparam, Type cls)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public static void StructureToPtr(object structure, IntPtr ptr, bool fDeleteOld)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int OleGetClipboard(ref IDataObject data);

	[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int OleSetClipboard(IDataObject pDataObj);

	[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int OleFlushClipboard();

	[DllImport("uxtheme.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static extern int GetCurrentThemeName(StringBuilder pszThemeFileName, int dwMaxNameChars, StringBuilder pszColorBuff, int dwMaxColorChars, StringBuilder pszSizeBuff, int cchMaxSizeChars);

	[DllImport("dwmapi.dll", BestFitMapping = false)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static extern int DwmIsCompositionEnabled(out int enabled);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static extern IntPtr GetCurrentThread();

	[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static extern WindowMessage RegisterWindowMessage(string msg);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static extern bool SetWindowPos(HandleRef hWnd, HandleRef hWndInsertAfter, int x, int y, int cx, int cy, int flags);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static extern IntPtr GetWindow(HandleRef hWnd, int uCmd);

	[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static extern int GetClassName(HandleRef hwnd, StringBuilder lpClassName, int nMaxCount);

	[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static extern int MessageBox(HandleRef hWnd, string text, string caption, int type);

	[DllImport("uxtheme.dll", BestFitMapping = false, CharSet = CharSet.Auto, EntryPoint = "SetWindowTheme")]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static extern int CriticalSetWindowTheme(HandleRef hWnd, string subAppName, string subIdList);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	public static extern IntPtr CreateCompatibleBitmap(HandleRef hDC, int width, int height);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateCompatibleBitmap", ExactSpelling = true, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr CriticalCreateCompatibleBitmap(HandleRef hDC, int width, int height);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetStockObject", SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr CriticalGetStockObject(int stockObject);

	[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "FillRect", SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int CriticalFillRect(IntPtr hdc, ref NativeMethods.RECT rcFill, IntPtr brush);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static extern int GetBitmapBits(HandleRef hbmp, int cbBuffer, byte[] lpvBits);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool ShowWindow(HandleRef hWnd, int nCmdShow);

	[SecurityCritical]
	public static void DeleteObject(HandleRef hObject)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public static bool DeleteObjectNoThrow(HandleRef hObject)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteObject", ExactSpelling = true, SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static extern bool IntDeleteObject(HandleRef hObject);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	public static extern IntPtr SelectObject(HandleRef hdc, IntPtr obj);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static extern IntPtr SelectObject(HandleRef hdc, NativeMethods.BitmapHandle obj);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SelectObject", ExactSpelling = true, SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static extern IntPtr CriticalSelectObject(HandleRef hdc, IntPtr obj);

	[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
	public static extern int GetClipboardFormatName(int format, StringBuilder lpString, int cchMax);

	[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int RegisterClipboardFormat(string format);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	public static extern bool BitBlt(HandleRef hDC, int x, int y, int nWidth, int nHeight, HandleRef hSrcDC, int xSrc, int ySrc, int dwRop);

	[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "PrintWindow", ExactSpelling = true, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool CriticalPrintWindow(HandleRef hWnd, HandleRef hDC, int flags);

	[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "RedrawWindow", ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool CriticalRedrawWindow(HandleRef hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, int flags);

	[DllImport("shell32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
	public static extern int DragQueryFile(HandleRef hDrop, int iFile, StringBuilder lpszFile, int cch);

	[DllImport("shell32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static extern IntPtr ShellExecute(HandleRef hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

	[DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	internal static extern bool ShellExecuteEx([In][Out] ShellExecuteInfo lpExecInfo);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static extern int MultiByteToWideChar(int CodePage, int dwFlags, byte[] lpMultiByteStr, int cchMultiByte, [Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpWideCharStr, int cchWideChar);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static extern int WideCharToMultiByte(int codePage, int flags, [MarshalAs(UnmanagedType.LPWStr)] string wideStr, int chars, [In][Out] byte[] pOutBytes, int bufferBytes, IntPtr defaultChar, IntPtr pDefaultUsed);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "RtlMoveMemory", ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern void CopyMemoryW(IntPtr pdst, string psrc, int cb);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "RtlMoveMemory", ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern void CopyMemoryW(IntPtr pdst, char[] psrc, int cb);

	[DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern void CopyMemory(IntPtr pdst, byte[] psrc, int cb);

	[SecurityCritical]
	public static void GetKeyboardState(byte[] keystate)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static string GetModuleFileName(HandleRef hModule)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool TranslateMessage([In][Out] ref MSG msg);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr DispatchMessage([In] ref MSG msg);

	[SecurityCritical]
	public static void PostThreadMessage(int id, int msg, IntPtr wparam, IntPtr lparam)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("oleacc.dll")]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	internal static extern int ObjectFromLresult(IntPtr lResult, ref Guid iid, IntPtr wParam, [In][Out] ref IAccessible ppvObject);

	[DllImport("user32.dll")]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	internal static extern bool IsWinEventHookInstalled(int winevent);

	[SecurityCritical]
	public static int OleInitialize()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("ole32.dll")]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int CoRegisterPSClsid(ref Guid riid, ref Guid rclsid);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	public static extern bool EnumThreadWindows(int dwThreadId, NativeMethods.EnumThreadWindowsCallback lpfn, HandleRef lParam);

	[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int OleUninitialize();

	[SecurityCritical]
	public static bool CloseHandleNoThrow(HandleRef handle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	public static extern int CreateStreamOnHGlobal(IntPtr hGlobal, bool fDeleteOnRelease, ref System.Runtime.InteropServices.ComTypes.IStream istream);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateCompatibleDC", SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr CriticalCreateCompatibleDC(HandleRef hDC);

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static IntPtr CreateCompatibleDC(HandleRef hDC)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public static bool UnmapViewOfFileNoThrow(HandleRef pvBaseAddress)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public static bool EnableWindow(HandleRef hWnd, bool enable)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public static bool EnableWindowNoThrow(HandleRef hWnd, bool enable)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern int GetObject(HandleRef hObject, int nSize, [In][Out] NativeMethods.BITMAP bm);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr GetFocus();

	[SecurityCritical]
	internal static bool GetCursorPos([In][Out] NativeMethods.POINT pt)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static bool TryGetCursorPos([In][Out] NativeMethods.POINT pt)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int GetWindowThreadProcessId(HandleRef hWnd, out int lpdwProcessId);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern short GetKeyState(int keyCode);

	[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true, PreserveSig = false)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern void DoDragDrop(IDataObject dataObject, IOleDropSource dropSource, int allowedEffects, int[] finalEffect);

	[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern void ReleaseStgMedium(ref STGMEDIUM medium);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool InvalidateRect(HandleRef hWnd, IntPtr rect, bool erase);

	[SecurityCritical]
	internal static int GetWindowText(HandleRef hWnd, [Out] StringBuilder lpString, int nMaxCount)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static int GetWindowTextLength(HandleRef hWnd)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr GlobalAlloc(int uFlags, IntPtr dwBytes);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr GlobalReAlloc(HandleRef handle, IntPtr bytes, int flags);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr GlobalLock(HandleRef handle);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool GlobalUnlock(HandleRef handle);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr GlobalFree(HandleRef handle);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr GlobalSize(HandleRef handle);

	[DllImport("imm32.dll", CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool ImmSetConversionStatus(HandleRef hIMC, int conversion, int sentence);

	[DllImport("imm32.dll", CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool ImmGetConversionStatus(HandleRef hIMC, ref int conversion, ref int sentence);

	[DllImport("imm32.dll", CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr ImmGetContext(HandleRef hWnd);

	[DllImport("imm32.dll", CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool ImmReleaseContext(HandleRef hWnd, HandleRef hIMC);

	[DllImport("imm32.dll", CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr ImmAssociateContext(HandleRef hWnd, HandleRef hIMC);

	[DllImport("imm32.dll", CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool ImmSetOpenStatus(HandleRef hIMC, bool open);

	[DllImport("imm32.dll", CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool ImmGetOpenStatus(HandleRef hIMC);

	[DllImport("imm32.dll", CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool ImmNotifyIME(HandleRef hIMC, int dwAction, int dwIndex, int dwValue);

	[DllImport("imm32.dll", CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int ImmGetProperty(HandleRef hkl, int flags);

	[DllImport("imm32.dll", CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int ImmGetCompositionString(HandleRef hIMC, int dwIndex, char[] lpBuf, int dwBufLen);

	[DllImport("imm32.dll", CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int ImmGetCompositionString(HandleRef hIMC, int dwIndex, byte[] lpBuf, int dwBufLen);

	[DllImport("imm32.dll", CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int ImmGetCompositionString(HandleRef hIMC, int dwIndex, int[] lpBuf, int dwBufLen);

	[DllImport("imm32.dll", CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int ImmGetCompositionString(HandleRef hIMC, int dwIndex, IntPtr lpBuf, int dwBufLen);

	[DllImport("imm32.dll", CharSet = CharSet.Auto)]
	public static extern int ImmConfigureIME(HandleRef hkl, HandleRef hwnd, int dwData, IntPtr pvoid);

	[DllImport("imm32.dll", CharSet = CharSet.Auto)]
	public static extern int ImmConfigureIME(HandleRef hkl, HandleRef hwnd, int dwData, [In] ref NativeMethods.REGISTERWORD registerWord);

	[DllImport("imm32.dll", CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int ImmSetCompositionWindow(HandleRef hIMC, [In][Out] ref NativeMethods.COMPOSITIONFORM compform);

	[DllImport("imm32.dll", CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int ImmSetCandidateWindow(HandleRef hIMC, [In][Out] ref NativeMethods.CANDIDATEFORM candform);

	[DllImport("imm32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr ImmGetDefaultIMEWnd(HandleRef hwnd);

	[SecurityCritical]
	internal static IntPtr SetFocus(HandleRef hWnd)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static bool TrySetFocus(HandleRef hWnd)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static bool TrySetFocus(HandleRef hWnd, ref IntPtr result)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static IntPtr GetParent(HandleRef hWnd)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr GetAncestor(HandleRef hWnd, int flags);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static extern bool IsChild(HandleRef hWndParent, HandleRef hwnd);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr SetParent(HandleRef hWnd, HandleRef hWndParent);

	[SecurityCritical]
	internal static IntPtr GetModuleHandle(string modName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr CallWindowProc(IntPtr wndProc, IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "DefWindowProcW")]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr DefWindowProc(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

	[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr IntGetProcAddress(HandleRef hModule, string lpProcName);

	[SecurityCritical]
	public static IntPtr GetProcAddress(HandleRef hModule, string lpProcName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress")]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr GetProcAddressNoThrow(HandleRef hModule, string lpProcName);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr LoadLibrary(string lpFileName);

	[DllImport("user32.dll")]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int GetSystemMetrics(SM nIndex);

	[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool SystemParametersInfo(int nAction, int nParam, ref NativeMethods.RECT rc, int nUpdate);

	[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool SystemParametersInfo(int nAction, int nParam, ref int value, int ignore);

	[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool SystemParametersInfo(int nAction, int nParam, ref bool value, int ignore);

	[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool SystemParametersInfo(int nAction, int nParam, ref NativeMethods.HIGHCONTRAST_I rc, int nUpdate);

	[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool SystemParametersInfo(int nAction, int nParam, [In][Out] NativeMethods.NONCLIENTMETRICS metrics, int nUpdate);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool GetSystemPowerStatus(ref NativeMethods.SYSTEM_POWER_STATUS systemPowerStatus);

	[SecurityCritical]
	public static void ClientToScreen(HandleRef hWnd, [In][Out] NativeMethods.POINT pt)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr GetDesktopWindow();

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr GetForegroundWindow();

	[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int RegisterDragDrop(HandleRef hwnd, IOleDropTarget target);

	[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int RevokeDragDrop(HandleRef hwnd);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool PeekMessage([In][Out] ref MSG msg, HandleRef hwnd, WindowMessage msgMin, WindowMessage msgMax, int remove);

	[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool SetProp(HandleRef hWnd, string propName, HandleRef data);

	[SecurityCritical]
	internal static void PostMessage(HandleRef hwnd, WindowMessage msg, IntPtr wparam, IntPtr lparam)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "PostMessage")]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern bool TryPostMessage(HandleRef hwnd, WindowMessage msg, IntPtr wparam, IntPtr lparam);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern void NotifyWinEvent(int winEvent, HandleRef hwnd, int objType, int objID);

	[SecurityCritical]
	public static IntPtr BeginPaint(HandleRef hWnd, [In][Out][MarshalAs(UnmanagedType.LPStruct)] ref NativeMethods.PAINTSTRUCT lpPaint)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static bool EndPaint(HandleRef hWnd, [In][MarshalAs(UnmanagedType.LPStruct)] ref NativeMethods.PAINTSTRUCT lpPaint)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public static IntPtr GetDC(HandleRef hWnd)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int GetDeviceCaps(HandleRef hDC, int nIndex);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr GetActiveWindow();

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool SetForegroundWindow(HandleRef hWnd);

	[DllImport("comdlg32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern int CommDlgExtendedError();

	[DllImport("comdlg32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern bool GetOpenFileName([In][Out] NativeMethods.OPENFILENAME_I ofn);

	[DllImport("comdlg32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern bool GetSaveFileName([In][Out] NativeMethods.OPENFILENAME_I ofn);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool SetLayeredWindowAttributes(HandleRef hwnd, int crKey, byte bAlpha, int dwFlags);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, NativeMethods.POINT pptDst, NativeMethods.POINT pSizeDst, IntPtr hdcSrc, NativeMethods.POINT pptSrc, int crKey, ref NativeMethods.BLENDFUNCTION pBlend, int dwFlags);

	[DllImport("user32.dll", SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr SetActiveWindow(HandleRef hWnd);

	[SecurityCritical]
	public static bool DestroyCursor(IntPtr hCurs)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public static bool DestroyIcon(IntPtr hIcon)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public static bool DeleteObject(IntPtr hObject)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static NativeMethods.BitmapHandle CreateDIBSection(HandleRef hdc, ref NativeMethods.BITMAPINFO bitmapInfo, int iUsage, ref IntPtr ppvBits, SafeFileMappingHandle hSection, int dwOffset)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static NativeMethods.BitmapHandle CreateBitmap(int width, int height, int planes, int bitsPerPixel, byte[] lpvBits)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static bool DestroyIcon(HandleRef handle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static NativeMethods.IconHandle CreateIconIndirect([In][MarshalAs(UnmanagedType.LPStruct)] NativeMethods.ICONINFO iconInfo)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool IsWindow(HandleRef hWnd);

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static void DeleteDC(HandleRef hDC)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public static void CriticalDeleteDC(HandleRef hDC)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public static bool GetMessageW([In][Out] ref MSG msg, HandleRef hWnd, int uMsgFilterMin, int uMsgFilterMax)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public static IntPtr WindowFromPoint(int x, int y)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, EntryPoint = "CreateWindowEx", SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr IntCreateWindowEx(int dwExStyle, string lpszClassName, string lpszWindowName, int style, int x, int y, int width, int height, HandleRef hWndParent, HandleRef hMenu, HandleRef hInst, [MarshalAs(UnmanagedType.AsAny)] object pvParam);

	[SecurityCritical]
	public static IntPtr CreateWindowEx(int dwExStyle, string lpszClassName, string lpszWindowName, int style, int x, int y, int width, int height, HandleRef hWndParent, HandleRef hMenu, HandleRef hInst, [MarshalAs(UnmanagedType.AsAny)] object pvParam)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "DestroyWindow", SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool IntDestroyWindow(HandleRef hWnd);

	[SecurityCritical]
	public static void DestroyWindow(HandleRef hWnd)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("user32.dll")]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	internal static extern IntPtr SetWinEventHook(int eventMin, int eventMax, IntPtr hmodWinEventProc, NativeMethods.WinEventProcDef WinEventReentrancyFilter, uint idProcess, uint idThread, int dwFlags);

	[DllImport("user32.dll")]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	internal static extern bool UnhookWinEvent(IntPtr winEventHook);

	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static void EnumChildWindows(HandleRef hwndParent, EnumChildrenCallback lpEnumFunc, HandleRef lParam)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int GetWindowRgn(HandleRef hWnd, HandleRef hRgn);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool PtInRegion(HandleRef hRgn, int X, int Y);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr CreateRectRgn(int x1, int y1, int x2, int y2);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern uint GetRawInputDeviceList([In][Out] NativeMethods.RAWINPUTDEVICELIST[] ridl, [In][Out] ref uint numDevices, uint sizeInBytes);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern uint GetRawInputDeviceInfo(IntPtr hDevice, uint command, [In] ref NativeMethods.RID_DEVICE_INFO ridInfo, ref uint sizeInBytes);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetTempFileName", SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern uint _GetTempFileName(string tmpPath, string prefix, uint uniqueIdOrZero, StringBuilder tmpFileName);

	[SecurityCritical]
	internal static uint GetTempFileName(string tmpPath, string prefix, uint uniqueIdOrZero, StringBuilder tmpFileName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("shell32.dll", BestFitMapping = false, CharSet = CharSet.Auto, ThrowOnUnmappableChar = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern int ExtractIconEx(string szExeFileName, int nIconIndex, out NativeMethods.IconHandle phiconLarge, out NativeMethods.IconHandle phiconSmall, int nIcons);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern NativeMethods.IconHandle CreateIcon(IntPtr hInstance, int nWidth, int nHeight, byte cPlanes, byte cBitsPixel, byte[] lpbANDbits, byte[] lpbXORbits);

	[DllImport("user32.dll", SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool CreateCaret(HandleRef hwnd, NativeMethods.BitmapHandle hbitmap, int width, int height);

	[DllImport("user32.dll", SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool ShowCaret(HandleRef hwnd);

	[DllImport("user32.dll", SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool HideCaret(HandleRef hwnd);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool ShowWindowAsync(HandleRef hWnd, int nCmdShow);

	[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, EntryPoint = "LoadImage", SetLastError = true, ThrowOnUnmappableChar = true)]
	internal static extern NativeMethods.IconHandle LoadImageIcon(IntPtr hinst, string stName, int nType, int cxDesired, int cyDesired, int nFlags);

	[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, EntryPoint = "LoadImage", SetLastError = true, ThrowOnUnmappableChar = true)]
	internal static extern NativeMethods.CursorHandle LoadImageCursor(IntPtr hinst, string stName, int nType, int cxDesired, int cyDesired, int nFlags);

	[DllImport("urlmon.dll", ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern int CoInternetIsFeatureEnabled(int featureEntry, int dwFlags);

	[DllImport("urlmon.dll", ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern int CoInternetSetFeatureEnabled(int featureEntry, int dwFlags, bool fEnable);

	[DllImport("urlmon.dll", ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern int CoInternetIsFeatureZoneElevationEnabled([MarshalAs(UnmanagedType.LPWStr)] string szFromURL, [MarshalAs(UnmanagedType.LPWStr)] string szToURL, IInternetSecurityManager secMgr, int dwFlags);

	[DllImport("PresentationHost_v0400.dll", EntryPoint = "ProcessUnhandledException")]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern void ProcessUnhandledException_DLL([MarshalAs(UnmanagedType.BStr)] string errMsg);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern bool GetVersionEx([In][Out] NativeMethods.OSVERSIONINFOEX ver);

	[DllImport("urlmon.dll", ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern int CoInternetCreateSecurityManager([MarshalAs(UnmanagedType.Interface)] object pIServiceProvider, [MarshalAs(UnmanagedType.Interface)] out object ppISecurityManager, int dwReserved);

	[DllImport("kernel32.dll", SetLastError = true)]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static extern IntPtr LocalFree(IntPtr hMem);

	[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern SafeFileHandle CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, [In] NativeMethods.SECURITY_ATTRIBUTES lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern IntPtr GetMessageExtraInfo();

	[SecurityCritical]
	internal static int WaitForMultipleObjectsEx(int nCount, IntPtr[] pHandles, bool bWaitAll, int dwMilliseconds, bool bAlertable)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static int MsgWaitForMultipleObjectsEx(int nCount, IntPtr[] pHandles, int dwMilliseconds, int dwWakeMask, int dwFlags)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Unicode, EntryPoint = "RegisterClassEx", SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern ushort IntRegisterClassEx(NativeMethods.WNDCLASSEX_D wc_d);

	[SecurityCritical]
	internal static ushort RegisterClassEx(NativeMethods.WNDCLASSEX_D wc_d)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, EntryPoint = "UnregisterClass", SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern int IntUnregisterClass(IntPtr atomString, IntPtr hInstance);

	[SecurityCritical]
	internal static void UnregisterClass(IntPtr atomString, IntPtr hInstance)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static MS.Internal.Interop.HRESULT ChangeWindowMessageFilterEx(IntPtr hwnd, WindowMessage message, MSGFLT action, out MSGFLTINFO extStatus)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static string ObtainUserAgentString()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	internal static extern IntPtr SendMessage(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);

	[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern IntPtr UnsafeSendMessage(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);

	[DllImport("user32.dll")]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal unsafe static extern IntPtr RegisterPowerSettingNotification(IntPtr hRecipient, Guid* pGuid, int Flags);

	[DllImport("user32.dll")]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern IntPtr UnregisterPowerSettingNotification(IntPtr hPowerNotify);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern IntPtr SendMessage(HandleRef hWnd, WindowMessage msg, IntPtr wParam, NativeMethods.IconHandle iconHandle);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern void SetLastError(int dwErrorCode);

	[DllImport("user32.dll")]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool GetLayeredWindowAttributes(HandleRef hwnd, IntPtr pcrKey, IntPtr pbAlpha, IntPtr pdwFlags);

	[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern SafeFileMappingHandle CreateFileMapping(SafeFileHandle hFile, NativeMethods.SECURITY_ATTRIBUTES lpFileMappingAttributes, int flProtect, uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName);

	[DllImport("kernel32.dll", SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern SafeViewOfFileHandle MapViewOfFileEx(SafeFileMappingHandle hFileMappingObject, int dwDesiredAccess, int dwFileOffsetHigh, int dwFileOffsetLow, IntPtr dwNumberOfBytesToMap, IntPtr lpBaseAddress);

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal static IntPtr SetWindowLong(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static IntPtr CriticalSetWindowLong(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static IntPtr CriticalSetWindowLong(HandleRef hWnd, int nIndex, NativeMethods.WndProc dwNewLong)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static IntPtr GetWindowLongPtr(HandleRef hWnd, int nIndex)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static int GetWindowLong(HandleRef hWnd, int nIndex)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static NativeMethods.WndProc GetWindowLongWndProc(HandleRef hWnd)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("winmm.dll", CharSet = CharSet.Unicode)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	internal static extern bool PlaySound([In] string soundName, IntPtr hmod, SafeNativeMethods.PlaySoundFlags soundFlags);

	[DllImport("wininet.dll", CharSet = CharSet.Unicode, EntryPoint = "InternetGetCookieExW", ExactSpelling = true, SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	internal static extern bool InternetGetCookieEx([In] string Url, [In] string cookieName, [Out] StringBuilder cookieData, [In][Out] ref uint pchCookieData, uint flags, IntPtr reserved);

	[DllImport("wininet.dll", CharSet = CharSet.Unicode, EntryPoint = "InternetSetCookieExW", ExactSpelling = true, SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	internal static extern uint InternetSetCookieEx([In] string Url, [In] string CookieName, [In] string cookieData, uint flags, [In] string p3pHeader);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	internal static extern int GetLocaleInfoW(int locale, int type, string data, int dataSize);

	[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	internal static extern int FindNLSString(int locale, uint flags, [MarshalAs(UnmanagedType.LPWStr)] string sourceString, int sourceCount, [MarshalAs(UnmanagedType.LPWStr)] string findString, int findCount, out int found);

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal static void SetWindowText(HandleRef hWnd, string text)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static void GetIconInfo(HandleRef hIcon, out NativeMethods.ICONINFO piconinfo)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal static void GetWindowPlacement(HandleRef hWnd, ref NativeMethods.WINDOWPLACEMENT placement)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal static void SetWindowPlacement(HandleRef hWnd, [In] ref NativeMethods.WINDOWPLACEMENT placement)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern bool SystemParametersInfo(int nAction, int nParam, [In][Out] NativeMethods.ANIMATIONINFO anim, int nUpdate);

	[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, ThrowOnUnmappableChar = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern bool SystemParametersInfo(int nAction, int nParam, [In][Out] NativeMethods.ICONMETRICS metrics, int nUpdate);

	[DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool BeginPanningFeedback(HandleRef hwnd);

	[DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool UpdatePanningFeedback(HandleRef hwnd, int lTotalOverpanOffsetX, int lTotalOverpanOffsetY, bool fInInertia);

	[DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool EndPanningFeedback(HandleRef hwnd, bool fAnimateBack);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern bool SetEvent(IntPtr hEvent);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int SetEvent([In] SafeWaitHandle hHandle);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int WaitForSingleObject([In] SafeWaitHandle hHandle, [In] int dwMilliseconds);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern int GetMouseMovePointsEx(uint cbSize, [In] ref NativeMethods.MOUSEMOVEPOINT pointsIn, [Out] NativeMethods.MOUSEMOVEPOINT[] pointsBufferOut, int nBufPoints, uint resolution);

	[DllImport("kernel32.dll", SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern bool GetFileSizeEx(SafeFileHandle hFile, ref LARGE_INTEGER lpFileSize);

	[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
	internal static extern bool ConvertStringSecurityDescriptorToSecurityDescriptor(string stringSecurityDescriptor, int stringSDRevision, ref IntPtr securityDescriptor, IntPtr securityDescriptorSize);

	[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern SafeFileMappingHandle OpenFileMapping(int dwDesiredAccess, bool bInheritHandle, string lpName);

	[DllImport("kernel32.dll", SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern IntPtr VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, int flAllocationType, int flProtect);

	[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int OleIsCurrentClipboard(IDataObject pDataObj);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	internal static extern int GetOEMCP();

	[DllImport("ntdll.dll")]
	internal static extern int RtlNtStatusToDosError(int Status);

	internal static bool NtSuccess(int err)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal static void NtCheck(int err)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static int SafeReleaseComObject(object o)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("wininet.dll", EntryPoint = "GetUrlCacheConfigInfoW", SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern bool GetUrlCacheConfigInfo(ref NativeMethods.InternetCacheConfigInfo pInternetCacheConfigInfo, ref uint cbCacheConfigInfo, uint fieldControl);

	[DllImport("WtsApi32.dll")]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool WTSRegisterSessionNotification(IntPtr hwnd, uint dwFlags);

	[DllImport("WtsApi32.dll")]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool WTSUnRegisterSessionNotification(IntPtr hwnd);

	[DllImport("kernel32.dll", SetLastError = true)]
	[SecurityCritical]
	public static extern IntPtr GetCurrentProcess();

	[DllImport("kernel32.dll", SetLastError = true)]
	[SecurityCritical]
	public static extern bool DuplicateHandle(IntPtr hSourceProcess, SafeWaitHandle hSourceHandle, IntPtr hTargetProcessHandle, out IntPtr hTargetHandle, uint dwDesiredAccess, bool fInheritHandle, uint dwOptions);

	[DllImport("user32.dll")]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool IsIconic(IntPtr hWnd);

	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static HandleRef SetWindowsHookEx(HookType idHook, HookProc lpfn, IntPtr hMod, int dwThreadId)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("user32.dll", SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool UnhookWindowsHookEx(HandleRef hhk);

	[DllImport("user32.dll", SetLastError = true)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr CallNextHookEx(HandleRef hhk, int nCode, IntPtr wParam, IntPtr lParam);

	[DllImport("msctf.dll")]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	internal static extern int TF_CreateThreadMgr(out ITfThreadMgr threadManager);

	[DllImport("msctf.dll")]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	public static extern int TF_CreateInputProcessorProfiles(out ITfInputProcessorProfiles profiles);

	[DllImport("msctf.dll")]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static extern int TF_CreateDisplayAttributeMgr(out ITfDisplayAttributeMgr dam);

	[DllImport("msctf.dll")]
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	public static extern int TF_CreateCategoryMgr(out ITfCategoryMgr catmgr);

	public UnsafeNativeMethods()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static UnsafeNativeMethods()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
