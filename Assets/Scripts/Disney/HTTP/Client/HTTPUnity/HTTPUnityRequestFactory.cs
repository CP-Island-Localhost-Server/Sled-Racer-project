using DI.HTTP;
using DI.HTTP.Threaded;
using strange.extensions.injector.api;

namespace Disney.HTTP.Client.HTTPUnity
{
	[Implements(typeof(IHTTPRequestFactory), InjectionBindingScope.CROSS_CONTEXT, "HTTPUnityRequestFactory")]
	public class HTTPUnityRequestFactory : IHTTPRequestFactory
	{
		private static HTTPUnityRequestFactory instance = new HTTPUnityRequestFactory();

		private IHTTPClient httpClient;

		public static HTTPUnityRequestFactory Instance
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
				return ThreadedHTTPFactory.LogAllRequests;
			}
			set
			{
				ThreadedHTTPFactory.LogAllRequests = value;
			}
		}

		public bool VerboseLogging
		{
			get
			{
				return ThreadedHTTPFactory.VerboseLogging;
			}
			set
			{
				ThreadedHTTPFactory.VerboseLogging = value;
			}
		}

		public HTTPUnityRequestFactory()
		{
			IHTTPFactory factory = ThreadedHTTPFactory.getFactory();
			httpClient = factory.getClient();
		}

		public IHTTPRequest CreateRequest(string method, string uri)
		{
			return new HTTPUnityRequest(httpClient, method, uri);
		}

		public IHTTPRequest CreateRequest(string method, string uri, byte[] data)
		{
			return new HTTPUnityRequest(httpClient, method, uri, data);
		}
	}
}
