namespace Disney.HTTP.Client
{
	public interface IHTTPRequestFactory
	{
		bool LogAllRequests { get; set; }

		bool VerboseLogging { get; set; }

		IHTTPRequest CreateRequest(string method, string uri);

		IHTTPRequest CreateRequest(string method, string uri, byte[] data);
	}
}
