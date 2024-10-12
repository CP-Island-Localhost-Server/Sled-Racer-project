using System.Collections.Generic;
using DI.JSON;

namespace DI.Storage
{
	public class JSONDocument : IDocument, IJSONDocument
	{
		private IDocument inner;

		private IJSONParser parser;

		public JSONDocument(IDocument inner, IJSONParser parser)
		{
			if (parser == null)
			{
				throw new StorageException("A JSON parser is required when creating a JSONDocument.");
			}
			this.inner = inner;
			this.parser = parser;
		}

		public string getReference()
		{
			return (inner != null) ? inner.getReference() : null;
		}

		public string getName()
		{
			return (inner != null) ? inner.getName() : null;
		}

		public string getContents()
		{
			return (inner != null) ? inner.getContents() : null;
		}

		public byte[] getData()
		{
			return (inner != null) ? inner.getData() : null;
		}

		public IDictionary<string, object> getDocument()
		{
			if (inner != null && parser.Parse(inner.getContents()))
			{
				return parser.AsDictionary();
			}
			return null;
		}
	}
}
