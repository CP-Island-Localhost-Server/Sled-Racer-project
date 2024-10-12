using HTTP;

namespace Disney.HTTP.Client.UnityHTTP
{
	public class UnityHTTPRequestHeaders : IHTTPHeaders
	{
		private Request httpRequest;

		public UnityHTTPRequestHeaders(Request httpRequest)
		{
			this.httpRequest = httpRequest;
		}

		public string GetFirst(string name)
		{
			return httpRequest.GetHeader(name);
		}

		public void Add(string name, string value)
		{
			httpRequest.AddHeader(name, value);
		}

		public void Set(string name, string value)
		{
			httpRequest.SetHeader(name, value);
		}
	}
}
