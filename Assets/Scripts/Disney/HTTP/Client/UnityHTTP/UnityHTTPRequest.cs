using System;
using HTTP;
using UnityEngine;

namespace Disney.HTTP.Client.UnityHTTP
{
	public class UnityHTTPRequest : IHTTPRequest
	{
		private Request httpRequest;

		private UnityHTTPRequestHeaders httpHeaders;

		public IHTTPHeaders Headers => httpHeaders;

		public Uri Uri => httpRequest.uri;

		public byte[] Body => httpRequest.bytes;

		public UnityHTTPRequest(string method, string uri)
		{
			httpRequest = new Request(method, uri);
			httpHeaders = new UnityHTTPRequestHeaders(httpRequest);
		}

		public UnityHTTPRequest(string method, string uri, byte[] data)
		{
			httpRequest = new Request(method, uri, data);
			httpHeaders = new UnityHTTPRequestHeaders(httpRequest);
		}

		public IHTTPResponse Execute()
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				return HTTPErrorResponse.NetworkUnavailable;
			}
			IHTTPResponse httpResponse = null;
			httpRequest.synchronous = true;
			httpRequest.Send(delegate(Request request)
			{
				httpResponse = new UnityHTTPResponse(request.response);
			});
			return httpResponse;
		}

		public void ExecuteAsync(Action<IHTTPResponse> responseHandler)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				responseHandler(HTTPErrorResponse.NetworkUnavailable);
				return;
			}
			httpRequest.Send(delegate(Request request)
			{
				UnityHTTPResponse obj = new UnityHTTPResponse(request.response);
				responseHandler(obj);
			});
		}
	}
}
