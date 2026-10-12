using System;
using System.Collections.Generic;
using System.Text;

namespace OpenHardwareMonitor.Hardware;

public class Identifier : IComparable<Identifier>
{
	private readonly string identifier;

	private const char Separator = '/';

	private static void CheckIdentifiers(IEnumerable<string> identifiers)
	{
		foreach (string identifier in identifiers)
		{
			if (identifier.Contains(" ") || identifier.Contains('/'.ToString()))
			{
				throw new ArgumentException("Invalid identifier");
			}
		}
	}

	public Identifier(params string[] identifiers)
	{
		CheckIdentifiers(identifiers);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < identifiers.Length; i++)
		{
			stringBuilder.Append('/');
			stringBuilder.Append(identifiers[i]);
		}
		identifier = stringBuilder.ToString();
	}

	public Identifier(Identifier identifier, params string[] extensions)
	{
		CheckIdentifiers(extensions);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(identifier.ToString());
		for (int i = 0; i < extensions.Length; i++)
		{
			stringBuilder.Append('/');
			stringBuilder.Append(extensions[i]);
		}
		this.identifier = stringBuilder.ToString();
	}

	public override string ToString()
	{
		return identifier;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		Identifier identifier = obj as Identifier;
		if (identifier == null)
		{
			return false;
		}
		return this.identifier == identifier.identifier;
	}

	public override int GetHashCode()
	{
		return identifier.GetHashCode();
	}

	public int CompareTo(Identifier other)
	{
		if (other == null)
		{
			return 1;
		}
		return string.Compare(identifier, other.identifier, StringComparison.Ordinal);
	}

	public static bool operator ==(Identifier id1, Identifier id2)
	{
		if (id1.Equals(null))
		{
			return id2.Equals(null);
		}
		return id1.Equals(id2);
	}

	public static bool operator !=(Identifier id1, Identifier id2)
	{
		return !(id1 == id2);
	}

	public static bool operator <(Identifier id1, Identifier id2)
	{
		if (id1 == null)
		{
			return id2 != null;
		}
		return id1.CompareTo(id2) < 0;
	}

	public static bool operator >(Identifier id1, Identifier id2)
	{
		if (id1 == null)
		{
			return false;
		}
		return id1.CompareTo(id2) > 0;
	}
}
