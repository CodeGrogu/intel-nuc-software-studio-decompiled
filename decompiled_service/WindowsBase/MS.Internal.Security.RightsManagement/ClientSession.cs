using System;
using System.Collections.Generic;
using System.Security;
using System.Security.RightsManagement;

namespace MS.Internal.Security.RightsManagement;

[SecurityCritical(SecurityCriticalScope.Everything)]
internal class ClientSession : IDisposable
{
	internal ClientSession(ContentUser user)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ClientSession(ContentUser user, UserActivationMode userActivationMode)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	~ClientSession()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public void Dispose()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected virtual void Dispose(bool disposing)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static ClientSession DefaultUserClientSession(AuthenticationType authentication)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool IsMachineActivated()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void ActivateMachine(AuthenticationType authentication)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool IsUserActivated()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ContentUser ActivateUser(AuthenticationType authentication, UserActivationMode userActivationMode)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool IsClientLicensorCertificatePresent()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void AcquireClientLicensorCertificate()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void BuildSecureEnvironment(string applicationManifest)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal string GetMachineCert()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal List<string> EnumerateUsersCertificateIds(ContentUser user, EnumerateLicenseFlags certificateType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void DeleteLicense(string licenseId)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void RemoveUsersCertificates(EnumerateLicenseFlags certificateType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static string GetLicenseOnSession(SafeRightsManagementSessionHandle sessionHandle, EnumerateLicenseFlags enumerateLicenseFlags, int index)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal string EnumerateLicense(EnumerateLicenseFlags enumerateLicenseFlags, int index)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal PublishLicense SignIssuanceLicense(IssuanceLicense issuanceLicense, out UseLicense authorUseLicense)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal UseLicense AcquireUseLicense(string publishLicense, bool noUI)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal CryptoProvider TryBindUseLicenseToAllIdentites(string serializedUseLicense)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static string GetOwnerLicense(SafeRightsManagementPubHandle issuanceLicenseHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static ContentUser ExtractUserFromCertificateChain(string certificateChain)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static ContentUser ExtractUserFromCertificate(string certificate)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static string ExtractCertificateIdFromCertificateChain(string certificateChain)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static string ExtractCertificateIdFromCertificate(string certificate)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static Dictionary<string, string> ExtractApplicationSpecificDataFromLicense(string useLicenseChain)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void GetContentIdFromLicense(string useLicenseChain, out string contentId, out string contentIdType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static string GetContentIdFromPublishLicense(string publishLicense)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static Uri GetUseLicenseAcquisitionUriFromPublishLicense(string publishLicense)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void GetReferralInfoFromPublishLicense(string publishLicense, out string referralInfoName, out Uri referralInfoUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static string GetSecurityProviderPath()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static ContentRight? GetRightFromString(string rightName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static string GetStringFromRight(ContentRight right)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static ClientSession()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
