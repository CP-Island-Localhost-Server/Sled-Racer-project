using System.Text;
using DI.HTTP;

namespace Disney.HTTP.Client.HTTPUnity
{
	public class HTTPUnityResponse : IHTTPResponse
	{
		private DI.HTTP.IHTTPResponse httpResponse;

		private HTTPUnityResponseHeaders httpHeaders;

		public int StatusCode => (httpResponse == null) ? 404 : httpResponse.getStatusCode();

		public string StatusMessage => (httpResponse == null) ? string.Empty : httpResponse.getReasonPhrase();

		public bool IsError => httpResponse == null || httpResponse.getStatusCode() < 100 || httpResponse.getStatusCode() >= 400;

		public string Text
		{
			get
			{
				if (httpResponse == null || httpResponse.getDocument() == null)
				{
					return string.Empty;
				}
				byte[] data = httpResponse.getDocument().getData();
				return Encoding.UTF8.GetString(data);
			}
		}

		public byte[] Bytes => (httpResponse == null) ? null : httpResponse.getDocument().getData();

		public IHTTPHeaders Headers => httpHeaders;

		public HTTPUnityResponse(DI.HTTP.IHTTPResponse response)
		{
			httpResponse = response;
			httpHeaders = new HTTPUnityResponseHeaders(response);
		}
	}
}
