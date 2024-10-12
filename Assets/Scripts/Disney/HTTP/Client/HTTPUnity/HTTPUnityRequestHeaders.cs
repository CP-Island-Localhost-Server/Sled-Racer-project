using System.Collections.Generic;
using DI.HTTP;

namespace Disney.HTTP.Client.HTTPUnity
{
	public class HTTPUnityRequestHeaders : IHTTPHeaders
	{
		private DI.HTTP.IHTTPRequest httpRequest;

		public HTTPUnityRequestHeaders(DI.HTTP.IHTTPRequest httpRequest)
		{
			this.httpRequest = httpRequest;
		}

		public string GetFirst(string name)
		{
			if (httpRequest.getRequestHeaders().ContainsKey(name))
			{
				return httpRequest.getRequestHeaders()[name][0];
			}
			return null;
		}

		public void Add(string name, string value)
		{
			if (!httpRequest.getRequestHeaders().ContainsKey(name))
			{
				httpRequest.getRequestHeaders()[name] = new List<string>();
			}
			IList<string> list = httpRequest.getRequestHeaders()[name];
			list.Add(value);
		}

		public void Set(string name, string value)
		{
			List<string> list = new List<string>();
			list.Add(value);
			httpRequest.getRequestHeaders()[name] = list;
		}
	}
}
