using System.Collections.Generic;
using MS.Internal.WindowsBase;

namespace MS.Internal;

[FriendAccessAllowed]
internal sealed class ContentType
{
	internal class StrongComparer : IEqualityComparer<ContentType>
	{
		public bool Equals(ContentType x, ContentType y)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		public int GetHashCode(ContentType obj)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		public StrongComparer()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal class WeakComparer : IEqualityComparer<ContentType>
	{
		public bool Equals(ContentType x, ContentType y)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		public int GetHashCode(ContentType obj)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		public WeakComparer()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal string TypeComponent
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal string SubTypeComponent
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal Dictionary<string, string>.Enumerator ParameterValuePairs
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static ContentType Empty
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal string OriginalString
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal ContentType(string contentType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool AreTypeAndSubTypeEqual(ContentType contentType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool AreTypeAndSubTypeEqual(ContentType contentType, bool allowParameterValuePairs)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override string ToString()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static ContentType()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
