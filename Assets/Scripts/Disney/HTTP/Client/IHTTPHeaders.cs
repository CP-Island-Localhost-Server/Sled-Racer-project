namespace Disney.HTTP.Client
{
	public interface IHTTPHeaders
	{
		string GetFirst(string name);

		void Add(string name, string value);

		void Set(string name, string value);
	}
}
