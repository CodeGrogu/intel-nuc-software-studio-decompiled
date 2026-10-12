namespace MS.Internal.IO.Zip;

internal interface IZipIOBlock
{
	long Offset { get; }

	long Size { get; }

	bool GetDirtyFlag(bool closingFlag);

	void Move(long shiftSize);

	void Save();

	void UpdateReferences(bool closingFlag);

	PreSaveNotificationScanControlInstruction PreSaveNotification(long offset, long size);
}
