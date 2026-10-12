using System.Collections.Generic;
using System.Text;
using System.Xml;
using MS.Internal.WindowsBase;

namespace System.Windows.Markup;

[FriendAccessAllowed]
internal sealed class XmlCompatibilityReader : XmlWrappingReader
{
	public override string Value
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override string NamespaceURI
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override int Depth
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override bool HasAttributes
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override int AttributeCount
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public bool Normalization
	{
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal Encoding Encoding
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public XmlCompatibilityReader(XmlReader baseReader)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public XmlCompatibilityReader(XmlReader baseReader, IsXmlNamespaceSupportedCallback isXmlNamespaceSupported)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public XmlCompatibilityReader(XmlReader baseReader, IsXmlNamespaceSupportedCallback isXmlNamespaceSupported, IEnumerable<string> supportedNamespaces)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public XmlCompatibilityReader(XmlReader baseReader, IEnumerable<string> supportedNamespaces)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public void DeclareNamespaceCompatibility(string newNamespace, string oldNamespace)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override bool Read()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override string GetAttribute(int i)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override string GetAttribute(string name)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override string GetAttribute(string localName, string namespaceURI)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void MoveToAttribute(int i)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override bool MoveToAttribute(string name)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override bool MoveToAttribute(string localName, string namespaceURI)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override bool MoveToFirstAttribute()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override bool MoveToNextAttribute()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override string LookupNamespace(string prefix)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static XmlCompatibilityReader()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
