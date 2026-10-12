namespace System.IO.Packaging;

/// <summary>Specifies whether the target of a <see cref="T:System.IO.Packaging.PackageRelationship" /> is inside or outside the <see cref="T:System.IO.Packaging.Package" />.</summary>
public enum TargetMode
{
	/// <summary>The relationship references a part that is inside the package. </summary>
	Internal,
	/// <summary>The relationship references a resource that is external to the package. </summary>
	External
}
