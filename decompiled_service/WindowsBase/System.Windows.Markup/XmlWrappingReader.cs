using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace System.Windows.Markup;

internal class XmlWrappingReader : XmlReader, IXmlLineInfo, IXmlNamespaceResolver
{
	protected XmlReader _reader;

	protected IXmlLineInfo _readerAsIXmlLineInfo;

	protected IXmlNamespaceResolver _readerAsResolver;

	public override XmlReaderSettings Settings
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override XmlNodeType NodeType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override string Name
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override string LocalName
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

	public override string Prefix
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override bool HasValue
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override string Value
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

	public override string BaseURI
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override bool IsEmptyElement
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override bool IsDefault
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override char QuoteChar
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override XmlSpace XmlSpace
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override string XmlLang
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override IXmlSchemaInfo SchemaInfo
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override Type ValueType
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

	public override string this[int i]
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override string this[string name]
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override string this[string name, string namespaceURI]
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override bool CanResolveEntity
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override bool EOF
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override ReadState ReadState
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

	public override XmlNameTable NameTable
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public virtual int LineNumber
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public virtual int LinePosition
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	protected XmlReader Reader
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal XmlWrappingReader(XmlReader baseReader)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override string GetAttribute(string name)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override string GetAttribute(string name, string namespaceURI)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override string GetAttribute(int i)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override bool MoveToAttribute(string name)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override bool MoveToAttribute(string name, string ns)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void MoveToAttribute(int i)
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

	public override bool MoveToElement()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override bool Read()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void Close()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void Skip()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override string LookupNamespace(string prefix)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void ResolveEntity()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override bool ReadAttributeValue()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected override void Dispose(bool disposing)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public virtual bool HasLineInfo()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
