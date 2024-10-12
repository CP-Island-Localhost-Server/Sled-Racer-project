using HTTP;

namespace Disney.HTTP.Client.UnityHTTP
{
	public class UnityHTTPResponse : IHTTPResponse
	{
		private Response httpResponse;

		private UnityHTTPResponseHeaders httpHeaders;

		public int StatusCode => (httpResponse == null) ? 404 : httpResponse.status;

		public string StatusMessage => (httpResponse == null) ? string.Empty : httpResponse.message;

		public bool IsError => httpResponse == null || httpResponse.status < 100 || httpResponse.status >= 400;

		public string Text => (httpResponse == null) ? string.Empty : httpResponse.Text;

		public byte[] Bytes => (httpResponse == null) ? null : httpResponse.bytes;

		public IHTTPHeaders Headers => httpHeaders;

		public UnityHTTPResponse(Response response)
		{
			httpResponse = response;
			httpHeaders = new UnityHTTPResponseHeaders(response);
		}
	}
}
