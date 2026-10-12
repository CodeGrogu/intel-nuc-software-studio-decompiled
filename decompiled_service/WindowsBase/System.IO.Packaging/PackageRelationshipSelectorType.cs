namespace System.IO.Packaging;

/// <summary>Specifies the type of selection criteria that is used to match and return <see cref="T:System.IO.Packaging.PackageRelationship" /> selections through a <see cref="T:System.IO.Packaging.PackageRelationshipSelector" />.   </summary>
public enum PackageRelationshipSelectorType
{
	/// <summary>
	///     <see cref="T:System.IO.Packaging.PackageRelationship" /> selections are by <see cref="P:System.IO.Packaging.PackageRelationship.Id" />.</summary>
	Id,
	/// <summary>
	///     <see cref="T:System.IO.Packaging.PackageRelationship" /> selections are by <see cref="P:System.IO.Packaging.PackageRelationship.RelationshipType" />.</summary>
	Type
}
