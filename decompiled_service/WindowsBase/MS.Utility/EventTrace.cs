using System;
using System.Security;

namespace MS.Utility;

internal static class EventTrace
{
	public enum LayoutSource : byte
	{
		LayoutManager,
		HwndSource_SetLayoutSize,
		HwndSource_WMSIZE
	}

	internal enum Level : byte
	{
		LogAlways = 0,
		Critical = 1,
		Error = 2,
		Warning = 3,
		Info = 4,
		Verbose = 5,
		PERF = 16,
		PERF_LOW = 17,
		PERF_MED = 18,
		PERF_HIGH = 19
	}

	[Flags]
	internal enum Keyword
	{
		KeywordGeneral = 1,
		KeywordPerf = 2,
		KeywordText = 4,
		KeywordInput = 8,
		KeywordAnnotation = 0x10,
		KeywordXamlBaml = 0x20,
		KeywordXPS = 0x40,
		KeywordAnimation = 0x80,
		KeywordLayout = 0x100,
		KeywordHosting = 0x400,
		KeywordHeapMeter = 0x800,
		KeywordGraphics = 0x1000,
		KeywordDispatcher = 0x2000
	}

	internal enum Event : ushort
	{
		WClientCreateVisual = 1,
		WClientAppCtor = 2,
		WClientAppRun = 3,
		WClientString = 4,
		WClientStringBegin = 5,
		WClientStringEnd = 6,
		WClientPropParentCheck = 7,
		UpdateVisualStateStart = 8,
		UpdateVisualStateEnd = 9,
		PerfElementIDName = 10,
		PerfElementIDAssignment = 11,
		WClientFontCache = 1001,
		WClientInputMessage = 2001,
		StylusEventQueued = 2002,
		TouchDownReported = 2003,
		TouchMoveReported = 2004,
		TouchUpReported = 2005,
		ManipulationReportFrame = 2006,
		ManipulationEventRaised = 2007,
		CreateStickyNoteBegin = 3001,
		CreateStickyNoteEnd = 3002,
		DeleteTextNoteBegin = 3003,
		DeleteTextNoteEnd = 3004,
		DeleteInkNoteBegin = 3005,
		DeleteInkNoteEnd = 3006,
		CreateHighlightBegin = 3007,
		CreateHighlightEnd = 3008,
		ClearHighlightBegin = 3009,
		ClearHighlightEnd = 3010,
		LoadAnnotationsBegin = 3011,
		LoadAnnotationsEnd = 3012,
		AddAnnotationBegin = 3013,
		AddAnnotationEnd = 3014,
		DeleteAnnotationBegin = 3015,
		DeleteAnnotationEnd = 3016,
		GetAnnotationByIdBegin = 3017,
		GetAnnotationByIdEnd = 3018,
		GetAnnotationByLocBegin = 3019,
		GetAnnotationByLocEnd = 3020,
		GetAnnotationsBegin = 3021,
		GetAnnotationsEnd = 3022,
		SerializeAnnotationBegin = 3023,
		SerializeAnnotationEnd = 3024,
		DeserializeAnnotationBegin = 3025,
		DeserializeAnnotationEnd = 3026,
		UpdateAnnotationWithSNCBegin = 3027,
		UpdateAnnotationWithSNCEnd = 3028,
		UpdateSNCWithAnnotationBegin = 3029,
		UpdateSNCWithAnnotationEnd = 3030,
		AnnotationTextChangedBegin = 3031,
		AnnotationTextChangedEnd = 3032,
		AnnotationInkChangedBegin = 3033,
		AnnotationInkChangedEnd = 3034,
		AddAttachedSNBegin = 3035,
		AddAttachedSNEnd = 3036,
		RemoveAttachedSNBegin = 3037,
		RemoveAttachedSNEnd = 3038,
		AddAttachedHighlightBegin = 3039,
		AddAttachedHighlightEnd = 3040,
		RemoveAttachedHighlightBegin = 3041,
		RemoveAttachedHighlightEnd = 3042,
		AddAttachedMHBegin = 3043,
		AddAttachedMHEnd = 3044,
		RemoveAttachedMHBegin = 3045,
		RemoveAttachedMHEnd = 3046,
		WClientParseBamlBegin = 4001,
		WClientParseBamlEnd = 4002,
		WClientParseXmlBegin = 4003,
		WClientParseXmlEnd = 4004,
		WClientParseFefCrInstBegin = 4005,
		WClientParseFefCrInstEnd = 4006,
		WClientParseInstVisTreeBegin = 4007,
		WClientParseInstVisTreeEnd = 4008,
		WClientParseRdrCrInstBegin = 4009,
		WClientParseRdrCrInstEnd = 4010,
		WClientParseRdrCrInFTypBegin = 4011,
		WClientParseRdrCrInFTypEnd = 4012,
		WClientResourceFindBegin = 4013,
		WClientResourceFindEnd = 4014,
		WClientResourceCacheValue = 4015,
		WClientResourceCacheNull = 4016,
		WClientResourceCacheMiss = 4017,
		WClientResourceStock = 4018,
		WClientResourceBamlAssembly = 4019,
		WClientParseXamlBegin = 4020,
		WClientParseXamlBamlInfo = 4021,
		WClientParseXamlEnd = 4022,
		WClientDRXFlushPageStart = 5001,
		WClientDRXFlushPageStop = 5002,
		WClientDRXSerializeTreeStart = 5003,
		WClientDRXSerializeTreeEnd = 5004,
		WClientDRXGetVisualStart = 5005,
		WClientDRXGetVisualEnd = 5006,
		WClientDRXReleaseWriterStart = 5007,
		WClientDRXReleaseWriterEnd = 5008,
		WClientDRXGetPrintCapStart = 5009,
		WClientDRXGetPrintCapEnd = 5010,
		WClientDRXPTProviderStart = 5011,
		WClientDRXPTProviderEnd = 5012,
		WClientDRXRasterStart = 5013,
		WClientDRXRasterEnd = 5014,
		WClientDRXOpenPackageBegin = 5015,
		WClientDRXOpenPackageEnd = 5016,
		WClientDRXGetStreamBegin = 5017,
		WClientDRXGetStreamEnd = 5018,
		WClientDRXPageVisible = 5019,
		WClientDRXPageLoaded = 5020,
		WClientDRXInvalidateView = 5021,
		WClientDRXStyleCreated = 5022,
		WClientDRXFindBegin = 5023,
		WClientDRXFindEnd = 5024,
		WClientDRXZoom = 5025,
		WClientDRXEnsureOMBegin = 5026,
		WClientDRXEnsureOMEnd = 5027,
		WClientDRXTreeFlattenBegin = 5028,
		WClientDRXTreeFlattenEnd = 5029,
		WClientDRXAlphaFlattenBegin = 5030,
		WClientDRXAlphaFlattenEnd = 5031,
		WClientDRXGetDevModeBegin = 5032,
		WClientDRXGetDevModeEnd = 5033,
		WClientDRXStartDocBegin = 5034,
		WClientDRXStartDocEnd = 5035,
		WClientDRXEndDocBegin = 5036,
		WClientDRXEndDocEnd = 5037,
		WClientDRXStartPageBegin = 5038,
		WClientDRXStartPageEnd = 5039,
		WClientDRXEndPageBegin = 5040,
		WClientDRXEndPageEnd = 5041,
		WClientDRXCommitPageBegin = 5042,
		WClientDRXCommitPageEnd = 5043,
		WClientDRXConvertFontBegin = 5044,
		WClientDRXConvertFontEnd = 5045,
		WClientDRXConvertImageBegin = 5046,
		WClientDRXConvertImageEnd = 5047,
		WClientDRXSaveXpsBegin = 5048,
		WClientDRXSaveXpsEnd = 5049,
		WClientDRXLoadPrimitiveBegin = 5050,
		WClientDRXLoadPrimitiveEnd = 5051,
		WClientDRXSavePageBegin = 5052,
		WClientDRXSavePageEnd = 5053,
		WClientDRXSerializationBegin = 5054,
		WClientDRXSerializationEnd = 5055,
		WClientDRXReadStreamBegin = 5056,
		WClientDRXReadStreamEnd = 5057,
		WClientDRXGetPageBegin = 5058,
		WClientDRXGetPageEnd = 5059,
		WClientDRXLineDown = 5060,
		WClientDRXPageDown = 5061,
		WClientDRXPageJump = 5062,
		WClientDRXLayoutBegin = 5063,
		WClientDRXLayoutEnd = 5064,
		WClientDRXInstantiated = 5065,
		WClientTimeManagerTickBegin = 6001,
		WClientTimeManagerTickEnd = 6002,
		WClientLayoutBegin = 7001,
		WClientLayoutEnd = 7002,
		WClientMeasureBegin = 7005,
		WClientMeasureAbort = 7006,
		WClientMeasureEnd = 7007,
		WClientMeasureElementBegin = 7008,
		WClientMeasureElementEnd = 7009,
		WClientArrangeBegin = 7010,
		WClientArrangeAbort = 7011,
		WClientArrangeEnd = 7012,
		WClientArrangeElementBegin = 7013,
		WClientArrangeElementEnd = 7014,
		WClientLayoutAbort = 7015,
		WClientLayoutFireSizeChangedBegin = 7016,
		WClientLayoutFireSizeChangedEnd = 7017,
		WClientLayoutFireLayoutUpdatedBegin = 7018,
		WClientLayoutFireLayoutUpdatedEnd = 7019,
		WClientLayoutFireAutomationEventsBegin = 7020,
		WClientLayoutFireAutomationEventsEnd = 7021,
		WClientLayoutException = 7022,
		WClientLayoutInvalidated = 7023,
		WpfHostUm_WinMainStart = 9003,
		WpfHostUm_WinMainEnd = 9004,
		WpfHostUm_InvokingBrowser = 9005,
		WpfHostUm_LaunchingRestrictedProcess = 9006,
		WpfHostUm_EnteringMessageLoop = 9007,
		WpfHostUm_ClassFactoryCreateInstance = 9008,
		WpfHostUm_ReadingDeplManifestStart = 9009,
		WpfHostUm_ReadingDeplManifestEnd = 9010,
		WpfHostUm_ReadingAppManifestStart = 9011,
		WpfHostUm_ReadingAppManifestEnd = 9012,
		WpfHostUm_ParsingMarkupVersionStart = 9013,
		WpfHostUm_ParsingMarkupVersionEnd = 9014,
		WpfHostUm_IPersistFileLoad = 9015,
		WpfHostUm_IPersistMonikerLoadStart = 9016,
		WpfHostUm_IPersistMonikerLoadEnd = 9017,
		WpfHostUm_BindProgress = 9018,
		WpfHostUm_OnStopBinding = 9019,
		WpfHostUm_VersionAttach = 9020,
		WpfHostUm_VersionActivateStart = 9021,
		WpfHostUm_VersionActivateEnd = 9022,
		WpfHostUm_StartingCLRStart = 9023,
		WpfHostUm_StartingCLREnd = 9024,
		WpfHostUm_IHlinkTargetNavigateStart = 9025,
		WpfHostUm_IHlinkTargetNavigateEnd = 9026,
		WpfHostUm_ReadyStateChanged = 9027,
		WpfHostUm_InitDocHostStart = 9028,
		WpfHostUm_InitDocHostEnd = 9029,
		WpfHostUm_MergingMenusStart = 9030,
		WpfHostUm_MergingMenusEnd = 9031,
		WpfHostUm_UIActivationStart = 9032,
		WpfHostUm_UIActivationEnd = 9033,
		WpfHostUm_LoadingResourceDLLStart = 9034,
		WpfHostUm_LoadingResourceDLLEnd = 9035,
		WpfHostUm_OleCmdQueryStatusStart = 9036,
		WpfHostUm_OleCmdQueryStatusEnd = 9037,
		WpfHostUm_OleCmdExecStart = 9038,
		WpfHostUm_OleCmdExecEnd = 9039,
		WpfHostUm_ProgressPageShown = 9040,
		WpfHostUm_AdHocProfile1Start = 9041,
		WpfHostUm_AdHocProfile1End = 9042,
		WpfHostUm_AdHocProfile2Start = 9043,
		WpfHostUm_AdHocProfile2End = 9044,
		WpfHost_DocObjHostCreated = 9045,
		WpfHost_XappLauncherAppStartup = 9046,
		WpfHost_XappLauncherAppExit = 9047,
		WpfHost_DocObjHostRunApplicationStart = 9048,
		WpfHost_DocObjHostRunApplicationEnd = 9049,
		WpfHost_ClickOnceActivationStart = 9050,
		WpfHost_ClickOnceActivationEnd = 9051,
		WpfHost_InitAppProxyStart = 9052,
		WpfHost_InitAppProxyEnd = 9053,
		WpfHost_AppProxyCtor = 9054,
		WpfHost_RootBrowserWindowSetupStart = 9055,
		WpfHost_RootBrowserWindowSetupEnd = 9056,
		WpfHost_AppProxyRunStart = 9057,
		WpfHost_AppProxyRunEnd = 9058,
		WpfHost_AppDomainManagerCctor = 9059,
		WpfHost_ApplicationActivatorCreateInstanceStart = 9060,
		WpfHost_ApplicationActivatorCreateInstanceEnd = 9061,
		WpfHost_DetermineApplicationTrustStart = 9062,
		WpfHost_DetermineApplicationTrustEnd = 9063,
		WpfHost_FirstTimeActivation = 9064,
		WpfHost_GetDownloadPageStart = 9065,
		WpfHost_GetDownloadPageEnd = 9066,
		WpfHost_DownloadDeplManifestStart = 9067,
		WpfHost_DownloadDeplManifestEnd = 9068,
		WpfHost_AssertAppRequirementsStart = 9069,
		WpfHost_AssertAppRequirementsEnd = 9070,
		WpfHost_DownloadApplicationStart = 9071,
		WpfHost_DownloadApplicationEnd = 9072,
		WpfHost_DownloadProgressUpdate = 9073,
		WpfHost_XappLauncherAppNavigated = 9074,
		WpfHost_StartingFontCacheServiceStart = 9075,
		WpfHost_StartingFontCacheServiceEnd = 9076,
		WpfHost_UpdateBrowserCommandsStart = 9077,
		WpfHost_UpdateBrowserCommandsEnd = 9078,
		WpfHost_PostShutdown = 9079,
		WpfHost_AbortingActivation = 9080,
		WpfHost_IBHSRunStart = 9081,
		WpfHost_IBHSRunEnd = 9082,
		Wpf_NavigationAsyncWorkItem = 9083,
		Wpf_NavigationWebResponseReceived = 9084,
		Wpf_NavigationEnd = 9085,
		Wpf_NavigationContentRendered = 9086,
		Wpf_NavigationStart = 9087,
		Wpf_NavigationLaunchBrowser = 9088,
		Wpf_NavigationPageFunctionReturn = 9089,
		DrawBitmapInfo = 11001,
		BitmapCopyInfo = 11002,
		SetClipInfo = 11003,
		DWMDraw_ClearStart = 11004,
		DWMDraw_ClearEnd = 11005,
		DWMDraw_BitmapStart = 11006,
		DWMDraw_BitmapEnd = 11007,
		DWMDraw_RectangleStart = 11008,
		DWMDraw_RectangleEnd = 11009,
		DWMDraw_GeometryStart = 11010,
		DWMDraw_GeometryEnd = 11011,
		DWMDraw_ImageStart = 11012,
		DWMDraw_ImageEnd = 11013,
		DWMDraw_GlyphRunStart = 11014,
		DWMDraw_GlyphRunEnd = 11015,
		DWMDraw_BeginLayerStart = 11016,
		DWMDraw_BeginLayerEnd = 11017,
		DWMDraw_EndLayerStart = 11018,
		DWMDraw_EndLayerEnd = 11019,
		DWMDraw_ClippedBitmapStart = 11020,
		DWMDraw_ClippedBitmapEnd = 11021,
		DWMDraw_Info = 11022,
		LayerEventStart = 11023,
		LayerEventEnd = 11024,
		WClientDesktopRTCreateBegin = 11025,
		WClientDesktopRTCreateEnd = 11026,
		WClientUceProcessQueueBegin = 11027,
		WClientUceProcessQueueEnd = 11028,
		WClientUceProcessQueueInfo = 11029,
		WClientUcePrecomputeBegin = 11030,
		WClientUcePrecomputeEnd = 11031,
		WClientUceRenderBegin = 11032,
		WClientUceRenderEnd = 11033,
		WClientUcePresentBegin = 11034,
		WClientUcePresentEnd = 11035,
		WClientUceResponse = 11036,
		WClientUceCheckDeviceStateInfo = 11037,
		VisualCacheAlloc = 11038,
		VisualCacheUpdate = 11039,
		CreateChannel = 11040,
		CreateOrAddResourceOnChannel = 11041,
		CreateWpfGfxResource = 11042,
		ReleaseOnChannel = 11043,
		UnexpectedSoftwareFallback = 11044,
		WClientInterlockedRenderBegin = 11045,
		WClientInterlockedRenderEnd = 11046,
		WClientRenderHandlerBegin = 11047,
		WClientRenderHandlerEnd = 11048,
		WClientAnimRenderHandlerBegin = 11049,
		WClientAnimRenderHandlerEnd = 11050,
		WClientMediaRenderBegin = 11051,
		WClientMediaRenderEnd = 11052,
		WClientPostRender = 11053,
		WClientQPCFrequency = 11054,
		WClientPrecomputeSceneBegin = 11055,
		WClientPrecomputeSceneEnd = 11056,
		WClientCompileSceneBegin = 11057,
		WClientCompileSceneEnd = 11058,
		WClientUIResponse = 11059,
		WClientUICommitChannel = 11060,
		WClientUceNotifyPresent = 11061,
		WClientScheduleRender = 11062,
		WClientOnRenderBegin = 11063,
		WClientOnRenderEnd = 11064,
		WClientCreateIRT = 11065,
		WClientPotentialIRTResource = 11066,
		WClientUIContextDispatchBegin = 12001,
		WClientUIContextDispatchEnd = 12002,
		WClientUIContextPost = 12003,
		WClientUIContextAbort = 12004,
		WClientUIContextPromote = 12005,
		WClientUIContextIdle = 12006
	}

	internal static readonly TraceProvider EventProvider;

	internal static void EasyTraceEvent(Keyword keywords, Event eventID)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void EasyTraceEvent(Keyword keywords, Level level, Event eventID)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void EasyTraceEvent(Keyword keywords, Event eventID, object param1)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void EasyTraceEvent(Keyword keywords, Level level, Event eventID, object param1)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void EasyTraceEvent(Keyword keywords, Event eventID, object param1, object param2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void EasyTraceEvent(Keyword keywords, Level level, Event eventID, object param1, object param2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void EasyTraceEvent(Keyword keywords, Event eventID, object param1, object param2, object param3)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static bool IsEnabled(Keyword flag, Level level)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	static EventTrace()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static Guid GetGuidForEvent(Event arg)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static ushort GetTaskForEvent(Event arg)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static byte GetOpcodeForEvent(Event arg)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static byte GetVersionForEvent(Event arg)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
