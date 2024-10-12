using System;

namespace Disney.HTTP.Client
{
	public interface IHTTPRequest
	{
		Uri Uri { get; }

		IHTTPHeaders Headers { get; }

		byte[] Body { get; }

		IHTTPResponse Execute();

		void ExecuteAsync(Action<IHTTPResponse> responseHandler);
	}
}
