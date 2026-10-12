using System;
using System.Collections.Generic;
using System.Security;
using System.Security.RightsManagement;

namespace MS.Internal.Security.RightsManagement;

[SecurityCritical(SecurityCriticalScope.Everything)]
internal class IssuanceLicense : IDisposable
{
	internal SafeRightsManagementPubHandle Handle
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal IssuanceLicense(DateTime validFrom, DateTime validUntil, string referralInfoName, Uri referralInfoUri, ContentUser owner, string issuanceLicense, SafeRightsManagementHandle boundLicenseHandle, Guid contentId, ICollection<ContentGrant> grantCollection, IDictionary<int, LocalizedNameDescriptionPair> localizedNameDescriptionDictionary, IDictionary<string, string> applicationSpecificDataDictionary, int rightValidityIntervalDays, RevocationPoint revocationPoint)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override string ToString()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void UpdateUnsignedPublishLicense(UnsignedPublishLicense unsignedPublishLicense)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	~IssuanceLicense()
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
}
