namespace Disney.HTTP.Client
{
	public interface IHTTPResponse
	{
		int StatusCode { get; }

		string StatusMessage { get; }

		string Text { get; }

		byte[] Bytes { get; }

		IHTTPHeaders Headers { get; }

		bool IsError { get; }
	}
}
