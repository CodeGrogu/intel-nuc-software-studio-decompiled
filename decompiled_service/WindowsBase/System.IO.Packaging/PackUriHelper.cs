using System.Security;

namespace System.IO.Packaging;

/// <summary>Provides utility methods to compose and parse pack URI objects.</summary>
public static class PackUriHelper
{
	internal sealed class ValidatedPartUri : Uri, IComparable<ValidatedPartUri>, IEquatable<ValidatedPartUri>
	{
		internal string PartUriString
		{
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		internal string PartUriExtension
		{
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		internal string NormalizedPartUriString
		{
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		internal ValidatedPartUri NormalizedPartUri
		{
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		internal bool IsNormalized
		{
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		internal bool IsRelationshipPartUri
		{
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		internal ValidatedPartUri(string partUriString)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		internal ValidatedPartUri(string partUriString, bool isRelationshipUri)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		int IComparable<ValidatedPartUri>.CompareTo(ValidatedPartUri otherPartUri)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		bool IEquatable<ValidatedPartUri>.Equals(ValidatedPartUri otherPartUri)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		static ValidatedPartUri()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static readonly char ForwardSlashChar;

	internal static readonly char BackwardSlashChar;

	/// <summary>Defines the pack URI scheme name "pack".</summary>
	/// <returns>The pack URI scheme name string "pack".</returns>
	public static readonly string UriSchemePack;

	internal static Uri PackageRootUri
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Creates a new pack URI that points to a package. </summary>
	/// <param name="packageUri">The URI of the referenced <see cref="T:System.IO.Packaging.Package" />.</param>
	/// <returns>The pack URI for the <see cref="T:System.IO.Packaging.Package" /> referenced by the given <paramref name="packageUri" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="packageUri" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="packageUri" /> is not an absolute URI.</exception>
	public static Uri Create(Uri packageUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a pack URI given a <see cref="T:System.IO.Packaging.Package" /> URI and the URI of a part in the package.</summary>
	/// <param name="packageUri">The URI of the <see cref="T:System.IO.Packaging.Package" />.</param>
	/// <param name="partUri">The URI of the <see cref="T:System.IO.Packaging.PackagePart" /> in the package.</param>
	/// <returns>The pack URI of the given <see cref="T:System.IO.Packaging.PackagePart" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="packageUri" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="packageUri" /> is not an absolute URI.-or-
	///         <paramref name="partUri" /> is not a valid part URI syntax.</exception>
	public static Uri Create(Uri packageUri, Uri partUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a pack URI given a <see cref="T:System.IO.Packaging.Package" /> URI, the URI of a part in the package, and a "#" fragment to append.</summary>
	/// <param name="packageUri">The URI of the <see cref="T:System.IO.Packaging.Package" />.</param>
	/// <param name="partUri">The URI of the <see cref="T:System.IO.Packaging.PackagePart" /> in the package.</param>
	/// <param name="fragment">A "#" reference identifying an element within the package part.</param>
	/// <returns>The pack URI that identifies the specified package, package part, and fragment.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="packageUri" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="packageUri" /> is not an absolute URI.-or-
	///         <paramref name="partUri" /> is not a valid part URI syntax. -or-
	///         <paramref name="fragment" /> is empty or does begin with "#".</exception>
	public static Uri Create(Uri packageUri, Uri partUri, string fragment)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the inner URI that points to the entire package of a specified pack URI. </summary>
	/// <param name="packUri">The pack URI from which to return the URI of the <see cref="T:System.IO.Packaging.Package" />.</param>
	/// <returns>The URI of the <see cref="T:System.IO.Packaging.Package" /> from the specified <paramref name="packUri" />.</returns>
	public static Uri GetPackageUri(Uri packUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the URI of a <see cref="T:System.IO.Packaging.PackagePart" /> within a specified pack URI. </summary>
	/// <param name="packUri">The pack URI from which to return the <see cref="T:System.IO.Packaging.PackagePart" /> URI.</param>
	/// <returns>The URI of the <see cref="T:System.IO.Packaging.PackagePart" /> in the given <paramref name="packUri" />, or <see langword="null" /> if <paramref name="packUri" /> points to a package instead of a <see cref="T:System.IO.Packaging.PackagePart" />. </returns>
	public static Uri GetPartUri(Uri packUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a formatted <see cref="T:System.IO.Packaging.PackagePart" /> URI. </summary>
	/// <param name="partUri">The URI of the <see cref="T:System.IO.Packaging.PackagePart" /> within the package.</param>
	/// <returns>A formatted <see cref="T:System.IO.Packaging.PackagePart" /> URI.</returns>
	public static Uri CreatePartUri(Uri partUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a part URI given a source part URI and a URI with a relative path to a target part.</summary>
	/// <param name="sourcePartUri">The URI of the source part, or "/" to designate the <see cref="T:System.IO.Packaging.Package" /> root.</param>
	/// <param name="targetUri">The relative URI to the target part.</param>
	/// <returns>The URI of the target part resolved between the specified <paramref name="SourcePartUri" /> and the <paramref name="targetUri" /> parameters.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="sourcePartUri" /> or <paramref name="targetUri" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="sourcePartUri" /> is not a valid part URI.-or-
	///         <paramref name="targetUri" /> is not a valid relative URI.</exception>
	public static Uri ResolvePartUri(Uri sourcePartUri, Uri targetUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the relative URI between two specified <see cref="T:System.IO.Packaging.PackagePart" /> URIs.</summary>
	/// <param name="sourcePartUri">The URI of the source part.</param>
	/// <param name="targetPartUri">The URI of the target part.</param>
	/// <returns>The relative URI from <paramref name="sourcePartUri" /> to <paramref name="targetPartUri" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="sourcePartUri" /> or <paramref name="targetPartUri" /> is <see langword="null" />.</exception>
	public static Uri GetRelativeUri(Uri sourcePartUri, Uri targetPartUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the normalized form of a specified <see cref="T:System.IO.Packaging.PackagePart" /> URI. </summary>
	/// <param name="partUri">The <see cref="T:System.IO.Packaging.PackagePart" /> URI to normalize.</param>
	/// <returns>The normalized form of the given <paramref name="partUri" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="partUri" /> is <see langword="null" />.</exception>
	public static Uri GetNormalizedPartUri(Uri partUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a value that indicates whether two pack URIs are equivalent.</summary>
	/// <param name="firstPackUri">The first pack URI.</param>
	/// <param name="secondPackUri">The second pack URI.</param>
	/// <returns>A signed integer indicating the relationship between <paramref name="firstPackUri" /> and <paramref name="secondPackUri" />.ValueMeaningLess than 0
	///             <paramref name="firstPackUri" /> is less than <paramref name="secondPackUri" />.0
	///             <paramref name="firstPackUri" /> is equivalent to <paramref name="secondPackUri" />.Greater than 0
	///             <paramref name="firstPackUri" /> is greater than <paramref name="secondPackUri" />.</returns>
	/// <exception cref="T:System.ArgumentException">Either <paramref name="firstPackUri" /> or <paramref name="secondPackUri" /> is not an absolute URI.-or-Either <paramref name="firstPackUri" /> or <paramref name="secondPackUri" /> do not begin with a "pack://" scheme.</exception>
	public static int ComparePackUri(Uri firstPackUri, Uri secondPackUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a value that indicates whether two package part URIs are equivalent.</summary>
	/// <param name="firstPartUri">The URI of the first <see cref="T:System.IO.Packaging.PackagePart" />.</param>
	/// <param name="secondPartUri">The URI of the second <see cref="T:System.IO.Packaging.PackagePart" />.</param>
	/// <returns>A value that indicates the relationship between <paramref name="firstPartUri" /> and <paramref name="secondPartUri" />.ValueMeaningLess than 0
	///             <paramref name="firstPartUri" /> is less than <paramref name="secondPartUri" />.0
	///             <paramref name="firstPartUri" /> is equivalent to <paramref name="secondPartUri" />.Greater than 0
	///             <paramref name="firstPartUri" /> is greater than <paramref name="secondPartUri" />.</returns>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="firstPartUri" /> or <paramref name="secondPartUri" /> is not a valid part URI syntax.</exception>
	public static int ComparePartUri(Uri firstPartUri, Uri secondPartUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a value that indicates whether a specified URI is the URI of a <see cref="T:System.IO.Packaging.PackageRelationship" /> part.</summary>
	/// <param name="partUri">The URI to check for a <see cref="T:System.IO.Packaging.PackageRelationship" /> part.</param>
	/// <returns>
	///     <see langword="true" /> if <paramref name="partUri" /> identifies a <see cref="T:System.IO.Packaging.PackageRelationship" /> part; otherwise, <see langword="false" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="partUri" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="partUri" /> is an absolute URI.-or-
	///         <paramref name="partUri" /> is an invalid <see cref="T:System.IO.Packaging.PackagePart" /> syntax.</exception>
	public static bool IsRelationshipPartUri(Uri partUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the URI of the relationship part associated with a specified <see cref="T:System.IO.Packaging.PackagePart" />.</summary>
	/// <param name="partUri">The <see cref="P:System.IO.Packaging.PackagePart.Uri" /> of the <see cref="T:System.IO.Packaging.PackagePart" /> to return the URI for the associated <see cref="T:System.IO.Packaging.PackageRelationship" />.</param>
	/// <returns>The URI of the <see cref="T:System.IO.Packaging.PackageRelationship" /> part associated with the <see cref="T:System.IO.Packaging.PackagePart" /> identified by <paramref name="partUri" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="partUri" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="partUri" /> syntax is not valid for a package part URI.-or-
	///         <paramref name="partUri" /> is an absolute URI.-or-
	///         <paramref name="partUri" /> references a relationship part.</exception>
	public static Uri GetRelationshipPartUri(Uri partUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the <see cref="P:System.IO.Packaging.PackageRelationship.SourceUri" /> from the <see cref="T:System.IO.Packaging.PackageRelationship" /> with a specified URI.</summary>
	/// <param name="relationshipPartUri">The URI of the relationship part to return the <see cref="P:System.IO.Packaging.PackageRelationship.SourceUri" /> from.</param>
	/// <returns>The <see cref="P:System.IO.Packaging.PackageRelationship.SourceUri" /> of the <see cref="T:System.IO.Packaging.PackagePart" /> from the relationship with the specified <paramref name="relationshipPartUri" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="relationshipPartUri" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="relationshipPartUri" /> is an absolute URI.-or-
	///         <paramref name="relationshipPartUri" /> syntax is not valid for a <see cref="T:System.IO.Packaging.PackagePart" />.-or-
	///         <paramref name="relationshipPartUri" /> does not reference a relationship part.-or-The <see cref="P:System.IO.Packaging.PackageRelationship.SourceUri" /> of the relationship part references another relationship part (not valid, see Remarks).</exception>
	public static Uri GetSourcePartUriFromRelationshipPartUri(Uri relationshipPartUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static bool IsPackUri(Uri uri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static bool TryValidatePartUri(Uri partUri, out ValidatedPartUri validatedPartUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static ValidatedPartUri ValidatePartUri(Uri partUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static string GetStringForPartUri(Uri partUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void ValidateAndGetPackUriComponents(Uri packUri, out Uri packageUri, out Uri partUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityTreatAsSafe]
	[SecurityCritical]
	static PackUriHelper()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
