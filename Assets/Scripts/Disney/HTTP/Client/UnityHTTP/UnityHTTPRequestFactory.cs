using HTTP;
using strange.extensions.injector.api;

namespace Disney.HTTP.Client.UnityHTTP
{
	[Implements(typeof(IHTTPRequestFactory), InjectionBindingScope.CROSS_CONTEXT, "UnityHTTPRequestFactory")]
	public class UnityHTTPRequestFactory : IHTTPRequestFactory
	{
		private static UnityHTTPRequestFactory instance = new UnityHTTPRequestFactory();

		public static UnityHTTPRequestFactory Instance
		{
			get
			{
				return instance;
			}
			set
			{
				instance = value;
			}
		}

		public bool LogAllRequests
		{
			get
			{
				return Request.LogAllRequests;
			}
			set
			{
				Request.LogAllRequests = value;
			}
		}

		public bool VerboseLogging
		{
			get
			{
				return Request.VerboseLogging;
			}
			set
			{
				Request.VerboseLogging = value;
			}
		}

		public IHTTPRequest CreateRequest(string method, string uri)
		{
			return new UnityHTTPRequest(method, uri);
		}

		public IHTTPRequest CreateRequest(string method, string uri, byte[] data)
		{
			return new UnityHTTPRequest(method, uri, data);
		}
	}
}
