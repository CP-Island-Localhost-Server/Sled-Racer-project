using System.Text;

namespace Disney.HTTP.Client
{
	public class HTTPErrorResponse : IHTTPResponse
	{
		public static HTTPErrorResponse NetworkUnavailable = new HTTPErrorResponse(0, "NETWORK UNAVAILABLE", "{ \"error\" : { \"errorCode\" : -1, \"errorDescription\" : \"Network unavailable\"");

		private int statusCode;

		private string statusMessage;

		private string text;

		public int StatusCode => statusCode;

		public string StatusMessage => statusMessage;

		public string Text => text;

		public byte[] Bytes => (text != null) ? Encoding.UTF8.GetBytes(text) : null;

		public IHTTPHeaders Headers => null;

		public bool IsError => statusCode < 100 || statusCode >= 400;

		public HTTPErrorResponse(int statusCode, string statusMessage, string body = null)
		{
			this.statusCode = statusCode;
			this.statusMessage = statusMessage;
			text = body;
		}
	}
}
