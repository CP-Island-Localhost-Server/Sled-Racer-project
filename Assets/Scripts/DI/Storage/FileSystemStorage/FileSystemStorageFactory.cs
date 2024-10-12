using DI.JSON;

namespace DI.Storage.FileSystemStorage
{
	public class FileSystemStorageFactory : IStorageFactory
	{
		private static FileSystemStorageFactory factory;

		private static IJSONParser _parser;

		private IJSONParser parser;

		private FileSystemStorageFactory(IJSONParser parser)
		{
			this.parser = parser;
		}

		public static IStorageFactory getFactory()
		{
			if (factory == null)
			{
				factory = new FileSystemStorageFactory(_parser);
			}
			return factory;
		}

		public static void setParser(IJSONParser parser)
		{
			if (_parser != null)
			{
				throw new StorageException("IJSONParser interface has already been initialized.");
			}
			_parser = parser;
		}

		public IStorage getStorage()
		{
			return new FileSystemStorage(this);
		}

		public IJSONParser getParser()
		{
			return parser;
		}
	}
}
