using System.Collections.Generic;
using DI.HTTP;

namespace Disney.HTTP.Client.HTTPUnity
{
	public class HTTPUnityResponseHeaders : IHTTPHeaders
	{
		private DI.HTTP.IHTTPResponse httpResponse;

		public HTTPUnityResponseHeaders(DI.HTTP.IHTTPResponse httpResponse)
		{
			this.httpResponse = httpResponse;
		}

		public string GetFirst(string name)
		{
			if (httpResponse.getResponseHeaders().ContainsKey(name))
			{
				return httpResponse.getResponseHeaders()[name][0];
			}
			return null;
		}

		public void Add(string name, string value)
		{
			if (!httpResponse.getResponseHeaders().ContainsKey(name))
			{
				httpResponse.getResponseHeaders()[name] = new List<string>();
			}
			IList<string> list = httpResponse.getResponseHeaders()[name];
			list.Add(value);
		}

		public void Set(string name, string value)
		{
			List<string> list = new List<string>();
			list.Add(value);
			httpResponse.getResponseHeaders()[name] = list;
		}
	}
}
