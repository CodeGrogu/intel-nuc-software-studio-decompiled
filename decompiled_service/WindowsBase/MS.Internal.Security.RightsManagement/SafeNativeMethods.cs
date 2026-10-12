using System;
using System.Security;
using System.Text;

namespace MS.Internal.Security.RightsManagement;

[SecurityCritical(SecurityCriticalScope.Everything)]
internal static class SafeNativeMethods
{
	internal static int DRMCreateClientSession(CallbackDelegate pfnCallback, uint uCallbackVersion, string GroupIDProviderType, string GroupID, out SafeRightsManagementSessionHandle phSession)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMCloseSession(uint sessionHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMCloseHandle(uint handle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMCloseQueryHandle(uint queryHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMCloseEnvironmentHandle(uint envHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMInitEnvironment(uint eSecurityProviderType, uint eSpecification, string securityProvider, string manifestCredentials, string machineCredentials, out SafeRightsManagementEnvironmentHandle environmentHandle, out SafeRightsManagementHandle defaultLibrary)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMIsActivated(SafeRightsManagementSessionHandle hSession, uint uFlags, ActivationServerInfo activationServerInfo)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMActivate(SafeRightsManagementSessionHandle hSession, uint uFlags, uint uLangID, ActivationServerInfo activationServerInfo, IntPtr context, IntPtr parentWindowHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMCreateLicenseStorageSession(SafeRightsManagementEnvironmentHandle hEnv, SafeRightsManagementHandle hDefLib, SafeRightsManagementSessionHandle hClientSession, uint uFlags, string IssuanceLicense, out SafeRightsManagementSessionHandle phLicenseStorageSession)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMAcquireLicense(SafeRightsManagementSessionHandle hSession, uint uFlags, string GroupIdentityCredential, string RequestedRights, string CustomData, string url, IntPtr context)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMEnumerateLicense(SafeRightsManagementSessionHandle hSession, uint uFlags, uint uIndex, ref bool pfSharedFlag, ref uint puCertDataLen, StringBuilder wszCertificateData)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetServiceLocation(SafeRightsManagementSessionHandle clientSessionHandle, uint serviceType, uint serviceLocation, string issuanceLicense, ref uint serviceUrlLength, StringBuilder serviceUrl)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMDeconstructCertificateChain(string chain, uint index, ref uint certificateLength, StringBuilder certificate)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMParseUnboundLicense(string certificate, out SafeRightsManagementQueryHandle queryRootHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetUnboundLicenseObjectCount(SafeRightsManagementQueryHandle queryRootHandle, string subObjectType, out uint objectCount)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetBoundLicenseObject(SafeRightsManagementHandle queryRootHandle, string subObjectType, uint index, out SafeRightsManagementHandle subQueryHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetUnboundLicenseObject(SafeRightsManagementQueryHandle queryRootHandle, string subObjectType, uint index, out SafeRightsManagementQueryHandle subQueryHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetUnboundLicenseAttribute(SafeRightsManagementQueryHandle queryRootHandle, string attributeType, uint index, out uint encodingType, ref uint bufferSize, byte[] buffer)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetBoundLicenseAttribute(SafeRightsManagementHandle queryRootHandle, string attributeType, uint index, out uint encodingType, ref uint bufferSize, byte[] buffer)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMCreateIssuanceLicense(SystemTime timeFrom, SystemTime timeUntil, string referralInfoName, string referralInfoUrl, SafeRightsManagementPubHandle ownerUserHandle, string issuanceLicense, SafeRightsManagementHandle boundLicenseHandle, out SafeRightsManagementPubHandle issuanceLicenseHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMCreateUser(string userName, string userId, string userIdType, out SafeRightsManagementPubHandle userHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetUsers(SafeRightsManagementPubHandle issuanceLicenseHandle, uint index, out SafeRightsManagementPubHandle userHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetUserRights(SafeRightsManagementPubHandle issuanceLicenseHandle, SafeRightsManagementPubHandle userHandle, uint index, out SafeRightsManagementPubHandle rightHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetUserInfo(SafeRightsManagementPubHandle userHandle, ref uint userNameLength, StringBuilder userName, ref uint userIdLength, StringBuilder userId, ref uint userIdTypeLength, StringBuilder userIdType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetRightInfo(SafeRightsManagementPubHandle rightHandle, ref uint rightNameLength, StringBuilder rightName, SystemTime timeFrom, SystemTime timeUntil)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMCreateRight(string rightName, SystemTime timeFrom, SystemTime timeUntil, uint countExtendedInfo, string[] extendedInfoNames, string[] extendedInfoValues, out SafeRightsManagementPubHandle rightHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetIssuanceLicenseTemplate(SafeRightsManagementPubHandle issuanceLicenseHandle, ref uint issuanceLicenseTemplateLength, StringBuilder issuanceLicenseTemplate)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMClosePubHandle(uint pubHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMAddRightWithUser(SafeRightsManagementPubHandle issuanceLicenseHandle, SafeRightsManagementPubHandle rightHandle, SafeRightsManagementPubHandle userHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMSetMetaData(SafeRightsManagementPubHandle issuanceLicenseHandle, string contentId, string contentIdType, string SkuId, string SkuIdType, string contentType, string contentName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetIssuanceLicenseInfo(SafeRightsManagementPubHandle issuanceLicenseHandle, SystemTime timeFrom, SystemTime timeUntil, uint flags, ref uint distributionPointNameLength, StringBuilder DistributionPointName, ref uint distributionPointUriLength, StringBuilder DistributionPointUri, out SafeRightsManagementPubHandle ownerHandle, out bool officialFlag)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetSecurityProvider(uint flags, ref uint typeLength, StringBuilder type, ref uint pathLength, StringBuilder path)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMDeleteLicense(SafeRightsManagementSessionHandle hSession, string wszLicenseId)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMSetNameAndDescription(SafeRightsManagementPubHandle issuanceLicenseHandle, bool flagDelete, uint localeId, string name, string description)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetNameAndDescription(SafeRightsManagementPubHandle issuanceLicenseHandle, uint uIndex, out uint localeId, ref uint nameLength, StringBuilder name, ref uint descriptionLength, StringBuilder description)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetSignedIssuanceLicense(SafeRightsManagementEnvironmentHandle environmentHandle, SafeRightsManagementPubHandle issuanceLicenseHandle, uint flags, byte[] symmetricKey, uint symmetricKeyByteCount, string symmetricKeyType, string clientLicensorCertificate, CallbackDelegate pfnCallback, string Url, uint context)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetOwnerLicense(SafeRightsManagementPubHandle issuanceLicenseHandle, ref uint ownerLicenseLength, StringBuilder ownerLicense)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMCreateBoundLicense(SafeRightsManagementEnvironmentHandle environmentHandle, BoundLicenseParams boundLicenseParams, string licenseChain, out SafeRightsManagementHandle boundLicenseHandle, out uint errorLogHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMCreateEnablingBitsDecryptor(SafeRightsManagementHandle boundLicenseHandle, string right, uint auxLibrary, string auxPlugin, out SafeRightsManagementHandle decryptorHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMCreateEnablingBitsEncryptor(SafeRightsManagementHandle boundLicenseHandle, string right, uint auxLibrary, string auxPlugin, out SafeRightsManagementHandle encryptorHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMDecrypt(SafeRightsManagementHandle cryptoProvHandle, uint position, uint inputByteCount, byte[] inputBuffer, ref uint outputByteCount, byte[] outputBuffer)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMEncrypt(SafeRightsManagementHandle cryptoProvHandle, uint position, uint inputByteCount, byte[] inputBuffer, ref uint outputByteCount, byte[] outputBuffer)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetInfo(SafeRightsManagementHandle handle, string attributeType, out uint encodingType, ref uint outputByteCount, byte[] outputBuffer)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetApplicationSpecificData(SafeRightsManagementPubHandle issuanceLicenseHandle, uint index, ref uint nameLength, StringBuilder name, ref uint valueLength, StringBuilder value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMSetApplicationSpecificData(SafeRightsManagementPubHandle issuanceLicenseHandle, bool flagDelete, string name, string value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetIntervalTime(SafeRightsManagementPubHandle issuanceLicenseHandle, ref uint days)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMSetIntervalTime(SafeRightsManagementPubHandle issuanceLicenseHandle, uint days)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMGetRevocationPoint(SafeRightsManagementPubHandle issuanceLicenseHandle, ref uint idLength, StringBuilder id, ref uint idTypeLength, StringBuilder idType, ref uint urlLength, StringBuilder url, SystemTime frequency, ref uint nameLength, StringBuilder name, ref uint publicKeyLength, StringBuilder publicKey)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int DRMSetRevocationPoint(SafeRightsManagementPubHandle issuanceLicenseHandle, bool flagDelete, string id, string idType, string url, SystemTime frequency, string name, string publicKey)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
