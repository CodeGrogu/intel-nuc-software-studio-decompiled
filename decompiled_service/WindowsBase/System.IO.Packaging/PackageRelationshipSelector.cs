using System.Collections.Generic;

namespace System.IO.Packaging;

/// <summary>Defines <see cref="T:System.IO.Packaging.PackageRelationship" /> criteria to select part-level or package-level relationships.</summary>
public sealed class PackageRelationshipSelector
{
	/// <summary>Gets the root package URI ("/") or part <see cref="P:System.IO.Packaging.PackagePart.Uri" /> specified to the <see cref="M:System.IO.Packaging.PackageRelationshipSelector.#ctor(System.Uri,System.IO.Packaging.PackageRelationshipSelectorType,System.String)" /> constructor as the owner of the relationship.</summary>
	/// <returns>The root package URI ("/") or part <see cref="P:System.IO.Packaging.PackagePart.Uri" /> specified to the <see cref="M:System.IO.Packaging.PackageRelationshipSelector.#ctor(System.Uri,System.IO.Packaging.PackageRelationshipSelectorType,System.String)" /> constructor as the owner of the relationship.</returns>
	public Uri SourceUri
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the <see cref="T:System.IO.Packaging.PackageRelationshipSelectorType" /> specified to the <see cref="M:System.IO.Packaging.PackageRelationshipSelector.#ctor(System.Uri,System.IO.Packaging.PackageRelationshipSelectorType,System.String)" /> constructor.</summary>
	/// <returns>The selector type of <see cref="F:System.IO.Packaging.PackageRelationshipSelectorType.Id" /> or <see cref="F:System.IO.Packaging.PackageRelationshipSelectorType.Type" /> specified to the <see cref="M:System.IO.Packaging.PackageRelationshipSelector.#ctor(System.Uri,System.IO.Packaging.PackageRelationshipSelectorType,System.String)" /> constructor.</returns>
	public PackageRelationshipSelectorType SelectorType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the selection criteria specified to the <see cref="M:System.IO.Packaging.PackageRelationshipSelector.#ctor(System.Uri,System.IO.Packaging.PackageRelationshipSelectorType,System.String)" /> constructor.</summary>
	/// <returns>The selection criteria based on the <see cref="P:System.IO.Packaging.PackageRelationshipSelector.SelectorType" /> of <see cref="F:System.IO.Packaging.PackageRelationshipSelectorType.Id" /> or <see cref="F:System.IO.Packaging.PackageRelationshipSelectorType.Type" /> specified to the <see cref="M:System.IO.Packaging.PackageRelationshipSelector.#ctor(System.Uri,System.IO.Packaging.PackageRelationshipSelectorType,System.String)" /> constructor.</returns>
	public string SelectionCriteria
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.IO.Packaging.PackageRelationshipSelector" /> class.</summary>
	/// <param name="sourceUri">The uniform resource identifier (URI) of the <see cref="T:System.IO.Packaging.PackagePart" /> or the <see cref="T:System.IO.Packaging.Package" /> (<paramref name="SourceUri" />="/") that owns the relationship.</param>
	/// <param name="selectorType">The type of the <paramref name="selectionCriteria" />, either by relationship <see cref="F:System.IO.Packaging.PackageRelationshipSelectorType.Id" /> or relationship <see cref="F:System.IO.Packaging.PackageRelationshipSelectorType.Type" />.</param>
	/// <param name="selectionCriteria">The qualification string that is used to select the relationships based on the <paramref name="selectorType" />.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="sourceUri" /> or <paramref name="selectionCriteria" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="selectorType" /> parameter is not valid.</exception>
	/// <exception cref="T:System.Xml.XmlException">The <paramref name="selectorType" /> parameter is <see cref="F:System.IO.Packaging.PackageRelationshipSelectorType.Id" /> but <paramref name="selectionCriteria" /> is not a valid XML Schema Definition (XSD) identifier (ID).</exception>
	/// <exception cref="T:System.ArgumentException">The <paramref name="selectionCriteria" /> is not valid for the specified <paramref name="selectorType" />.</exception>
	/// <exception cref="T:System.ArgumentException">The <paramref name="sourceUri" /> is not the <see cref="T:System.IO.Packaging.Package" /> root ("/") and is also not a valid <see cref="T:System.IO.Packaging.PackagePart" /> URI.</exception>
	public PackageRelationshipSelector(Uri sourceUri, PackageRelationshipSelectorType selectorType, string selectionCriteria)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a list of <see cref="T:System.IO.Packaging.PackageRelationship" /> objects that match the defined <see cref="P:System.IO.Packaging.PackageRelationshipSelector.SourceUri" />, <see cref="P:System.IO.Packaging.PackageRelationshipSelector.SelectorType" />, and <see cref="P:System.IO.Packaging.PackageRelationshipSelector.SelectionCriteria" />.</summary>
	/// <param name="package">The package from which to select the relationships based on the selection criteria.</param>
	/// <returns>A list of relationships that match the selection parameters specified to the <see cref="M:System.IO.Packaging.PackageRelationshipSelector.#ctor(System.Uri,System.IO.Packaging.PackageRelationshipSelectorType,System.String)" /> constructor.</returns>
	public List<PackageRelationship> Select(Package package)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
