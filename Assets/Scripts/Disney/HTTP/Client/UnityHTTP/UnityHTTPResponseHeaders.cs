using HTTP;

namespace Disney.HTTP.Client.UnityHTTP
{
	public class UnityHTTPResponseHeaders : IHTTPHeaders
	{
		private Response httpResponse;

		public UnityHTTPResponseHeaders(Response httpResponse)
		{
			this.httpResponse = httpResponse;
		}

		public string GetFirst(string name)
		{
			return httpResponse.GetHeader(name);
		}

		public void Add(string name, string value)
		{
		}

		public void Set(string name, string value)
		{
		}
	}
}
