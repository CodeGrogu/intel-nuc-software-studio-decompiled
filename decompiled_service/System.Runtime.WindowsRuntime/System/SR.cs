using System.Globalization;
using System.Resources;
using System.Threading;

namespace System;

internal sealed class SR
{
	internal const string Argument_BufferIndexExceedsCapacity = "Argument_BufferIndexExceedsCapacity";

	internal const string Argument_BufferLengthExceedsCapacity = "Argument_BufferLengthExceedsCapacity";

	internal const string Argument_CannotAppendBecauseIStorageFileMustBeAnIStorageItem = "Argument_CannotAppendBecauseIStorageFileMustBeAnIStorageItem";

	internal const string Argument_IndexOutOfArrayBounds = "Argument_IndexOutOfArrayBounds";

	internal const string Argument_InstancesImplementingIRASThatCanReadMustImplementIIS = "Argument_InstancesImplementingIRASThatCanReadMustImplementIIS";

	internal const string Argument_InstancesImplementingIRASThatCanWriteMustImplementIOS = "Argument_InstancesImplementingIRASThatCanWriteMustImplementIOS";

	internal const string Argument_InsufficientArrayElementsAfterOffset = "Argument_InsufficientArrayElementsAfterOffset";

	internal const string Argument_InsufficientBufferCapacity = "Argument_InsufficientBufferCapacity";

	internal const string Argument_InsufficientSpaceToCopyBuffer = "Argument_InsufficientSpaceToCopyBuffer";

	internal const string Argument_InsufficientSpaceInSourceBuffer = "Argument_InsufficientSpaceInSourceBuffer";

	internal const string Argument_InsufficientSpaceInTargetBuffer = "Argument_InsufficientSpaceInTargetBuffer";

	internal const string Argument_NotSufficientCapabilitiesToConvertToWinRtStream = "Argument_NotSufficientCapabilitiesToConvertToWinRtStream";

	internal const string Argument_ObjectMustBeWinRtStreamToConvertToNetFxStream = "Argument_ObjectMustBeWinRtStreamToConvertToNetFxStream";

	internal const string Argument_RelativePathMayNotBeWhitespaceOnly = "Argument_RelativePathMayNotBeWhitespaceOnly";

	internal const string Argument_StreamPositionBeyondEOS = "Argument_StreamPositionBeyondEOS";

	internal const string Argument_UnexpectedAsyncResult = "Argument_UnexpectedAsyncResult";

	internal const string Argument_WinRtStreamCannotReadOrWrite = "Argument_WinRtStreamCannotReadOrWrite";

	internal const string ArgumentOutOfRange_CannotResizeStreamToNegative = "ArgumentOutOfRange_CannotResizeStreamToNegative";

	internal const string ArgumentOutOfRange_IO_CannotSeekToNegativePosition = "ArgumentOutOfRange_IO_CannotSeekToNegativePosition";

	internal const string ArgumentOutOfRange_InvalidInputStreamOptionsEnumValue = "ArgumentOutOfRange_InvalidInputStreamOptionsEnumValue";

	internal const string ArgumentOutOfRange_WinRtAdapterBufferSizeMayNotBeNegative = "ArgumentOutOfRange_WinRtAdapterBufferSizeMayNotBeNegative";

	internal const string InvalidOperation_CannotCallThisMethodInCurrentState = "InvalidOperation_CannotCallThisMethodInCurrentState";

	internal const string InvalidOperation_CannotChangeBufferSizeOfWinRtStreamAdapter = "InvalidOperation_CannotChangeBufferSizeOfWinRtStreamAdapter";

	internal const string InvalidOperation_CannotChangeBufferSizeOfWinRtStreamAdapterToZero = "InvalidOperation_CannotChangeBufferSizeOfWinRtStreamAdapterToZero";

	internal const string InvalidOperation_CannotConvertRunningTaskUseOverloadToSpecifyCompletedHandler = "InvalidOperation_CannotConvertRunningTaskUseOverloadToSpecifyCompletedHandler";

	internal const string InvalidOperation_CannotGetResultsFromIncompleteOperation = "InvalidOperation_CannotGetResultsFromIncompleteOperation";

	internal const string InvalidOperation_CannotSetCompletionHanlderMoreThanOnce = "InvalidOperation_CannotSetCompletionHanlderMoreThanOnce";

	internal const string InvalidOperation_CannotSetStreamSizeCannotWrite = "InvalidOperation_CannotSetStreamSizeCannotWrite";

	internal const string InvalidOperation_CannotStartAsyncMoreThanOnce = "InvalidOperation_CannotStartAsyncMoreThanOnce";

	internal const string InvalidOperation_IllegalStateChange = "InvalidOperation_IllegalStateChange";

	internal const string InvalidOperation_InvalidAsyncCompletion = "InvalidOperation_InvalidAsyncCompletion";

	internal const string InvalidOperation_MultipleIOCompletionCallbackInvocation = "InvalidOperation_MultipleIOCompletionCallbackInvocation";

	internal const string InvalidOperation_SendNotSupportedOnWindowsRTSynchronizationContext = "InvalidOperation_SendNotSupportedOnWindowsRTSynchronizationContext";

	internal const string InvalidOperation_TaskProviderReturnedUnstartedTask = "InvalidOperation_TaskProviderReturnedUnstartedTask";

	internal const string InvalidOperation_UnexpectedAsyncOperationID = "InvalidOperation_UnexpectedAsyncOperationID";

	internal const string InvalidOperation_UnstartedTaskSpecified = "InvalidOperation_UnstartedTaskSpecified";

	internal const string IO_CannotSeekBeyondInt64MaxValue = "IO_CannotSeekBeyondInt64MaxValue";

	internal const string IO_CannotSetSizeBeyondInt64MaxValue = "IO_CannotSetSizeBeyondInt64MaxValue";

	internal const string IO_General = "IO_General";

	internal const string IO_UnderlyingWinRTStreamTooLong_CannotUseLengthOrPosition = "IO_UnderlyingWinRTStreamTooLong_CannotUseLengthOrPosition";

	internal const string NotImplemented_NativeRoutineNotFound = "NotImplemented_NativeRoutineNotFound";

	internal const string NotSupported_CannotConvertNotReadableToInputStream = "NotSupported_CannotConvertNotReadableToInputStream";

	internal const string NotSupported_CannotConvertNotSeekableToRandomAccessStream = "NotSupported_CannotConvertNotSeekableToRandomAccessStream";

	internal const string NotSupported_CannotConvertNotWritableToOutputStream = "NotSupported_CannotConvertNotWritableToOutputStream";

	internal const string NotSupported_CannotReadFromStream = "NotSupported_CannotReadFromStream";

	internal const string NotSupported_CannotSeekInStream = "NotSupported_CannotSeekInStream";

	internal const string NotSupported_CannotUseLength_StreamNotSeekable = "NotSupported_CannotUseLength_StreamNotSeekable";

	internal const string NotSupported_CannotUsePosition_StreamNotSeekable = "NotSupported_CannotUsePosition_StreamNotSeekable";

	internal const string NotSupported_CannotWriteToStream = "NotSupported_CannotWriteToStream";

	internal const string NotSupported_CloningNotSupported = "NotSupported_CloningNotSupported";

	internal const string NullReference_IOCompletionCallbackCannotProcessNullAsyncInfo = "NullReference_IOCompletionCallbackCannotProcessNullAsyncInfo";

	internal const string NullReference_TaskProviderReturnedNull = "NullReference_TaskProviderReturnedNull";

	internal const string ObjectDisposed_AsyncInfoIsClosed = "ObjectDisposed_AsyncInfoIsClosed";

	internal const string ObjectDisposed_CannotPerformOperation = "ObjectDisposed_CannotPerformOperation";

	internal const string WinRtCOM_Error = "WinRtCOM_Error";

	internal const string NotSupported_Inheritable = "NotSupported_Inheritable";

	internal const string NotSupported_Encrypted = "NotSupported_Encrypted";

	internal const string IO_FileNotFound = "IO_FileNotFound";

	internal const string IO_FileNotFound_FileName = "IO_FileNotFound_FileName";

	internal const string IO_PathNotFound_NoPathName = "IO_PathNotFound_NoPathName";

	internal const string IO_PathNotFound_Path = "IO_PathNotFound_Path";

	internal const string UnauthorizedAccess_IODenied_NoPathName = "UnauthorizedAccess_IODenied_NoPathName";

	internal const string UnauthorizedAccess_IODenied_Path = "UnauthorizedAccess_IODenied_Path";

	internal const string IO_AlreadyExists_Name = "IO_AlreadyExists_Name";

	internal const string IO_PathTooLong = "IO_PathTooLong";

	internal const string IO_SharingViolation_File = "IO_SharingViolation_File";

	internal const string IO_SharingViolation_NoFileName = "IO_SharingViolation_NoFileName";

	internal const string IO_FileExists_Name = "IO_FileExists_Name";

	internal const string DirectUI_Empty = "DirectUI_Empty";

	private static SR loader;

	private ResourceManager resources;

	private static CultureInfo Culture => null;

	public static ResourceManager Resources => GetLoader().resources;

	internal SR()
	{
		resources = new ResourceManager("System.Runtime.WindowsRuntime", GetType().Assembly);
	}

	private static SR GetLoader()
	{
		if (loader == null)
		{
			SR value = new SR();
			Interlocked.CompareExchange(ref loader, value, null);
		}
		return loader;
	}

	public static string GetString(string name, params object[] args)
	{
		SR sR = GetLoader();
		if (sR == null)
		{
			return null;
		}
		string text = sR.resources.GetString(name, Culture);
		if (args != null && args.Length != 0)
		{
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i] is string { Length: >1024 } text2)
				{
					args[i] = text2.Substring(0, 1021) + "...";
				}
			}
			return string.Format(CultureInfo.CurrentCulture, text, args);
		}
		return text;
	}

	public static string GetString(string name)
	{
		return GetLoader()?.resources.GetString(name, Culture);
	}

	public static string GetString(string name, out bool usedFallback)
	{
		usedFallback = false;
		return GetString(name);
	}

	public static object GetObject(string name)
	{
		return GetLoader()?.resources.GetObject(name, Culture);
	}
}
