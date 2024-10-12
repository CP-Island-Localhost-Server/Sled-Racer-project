namespace Disney.HTTP.Client
{
	public abstract class HTTPResponseDelegate : IHTTPResponse
	{
		private IHTTPResponse httpResponse;

		public int StatusCode => httpResponse.StatusCode;

		public string StatusMessage => httpResponse.StatusMessage;

		public string Text => httpResponse.Text;

		public byte[] Bytes => httpResponse.Bytes;

		public IHTTPHeaders Headers => httpResponse.Headers;

		public bool IsError => httpResponse.IsError;

		public HTTPResponseDelegate(IHTTPResponse httpResponse)
		{
			this.httpResponse = httpResponse;
		}
	}
}
