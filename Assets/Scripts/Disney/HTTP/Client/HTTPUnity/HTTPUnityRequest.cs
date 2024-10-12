using System;
using DI.HTTP;
using UnityEngine;

namespace Disney.HTTP.Client.HTTPUnity
{
	public class HTTPUnityRequest : IHTTPRequest
	{
		private class AsyncHTTPListener : IHTTPListener
		{
			private Action<IHTTPResponse> responseHandler;

			public AsyncHTTPListener(Action<IHTTPResponse> responseHandler)
			{
				this.responseHandler = responseHandler;
			}

			public void OnStart(DI.HTTP.IHTTPRequest request)
			{
			}

			public void OnProgress(DI.HTTP.IHTTPRequest request, byte[] data, int bytesRead, int bytesReceived, int bytesExpected)
			{
			}

			public void OnSuccess(DI.HTTP.IHTTPRequest request, DI.HTTP.IHTTPResponse response)
			{
				HTTPUnityResponse obj = new HTTPUnityResponse(response);
				responseHandler(obj);
			}

			public void OnError(DI.HTTP.IHTTPRequest request, DI.HTTP.IHTTPResponse response, Exception exception)
			{
				if (response != null)
				{
					HTTPUnityResponse obj = new HTTPUnityResponse(response);
					responseHandler(obj);
				}
			}

			public void OnComplete(DI.HTTP.IHTTPRequest request)
			{
			}
		}

		private DI.HTTP.IHTTPRequest httpRequest;

		private HTTPUnityRequestHeaders httpHeaders;

		public IHTTPHeaders Headers => httpHeaders;

		public Uri Uri => new Uri(httpRequest.getUrl());

		public byte[] Body => (httpRequest.getDocument() == null) ? null : httpRequest.getDocument().getData();

		public HTTPUnityRequest(IHTTPClient httpClient, string method, string uri)
		{
			httpRequest = httpClient.getRequest();
			httpRequest.setMethod(toHTTPMethod(method));
			httpRequest.setUrl(uri);
			httpHeaders = new HTTPUnityRequestHeaders(httpRequest);
		}

		public HTTPUnityRequest(IHTTPClient httpClient, string method, string uri, byte[] data)
		{
			httpRequest = httpClient.getRequest();
			httpRequest.setMethod(toHTTPMethod(method));
			httpRequest.setUrl(uri);
			HTTPBaseDocumentImpl document = new HTTPBaseDocumentImpl(data);
			httpRequest.setDocument(document);
			httpHeaders = new HTTPUnityRequestHeaders(httpRequest);
		}

		public IHTTPResponse Execute()
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				return HTTPErrorResponse.NetworkUnavailable;
			}
			return new HTTPUnityResponse(httpRequest.performSync());
		}

		public void ExecuteAsync(Action<IHTTPResponse> responseHandler)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				responseHandler(HTTPErrorResponse.NetworkUnavailable);
				return;
			}
			AsyncHTTPListener listener = new AsyncHTTPListener(responseHandler);
			httpRequest.setListener(listener);
			httpRequest.performAsync();
		}

		private HTTPMethod toHTTPMethod(string method)
		{
			if (method.ToUpper().Equals("GET"))
			{
				return HTTPMethod.GET;
			}
			if (method.ToUpper().Equals("POST"))
			{
				return HTTPMethod.POST;
			}
			if (method.ToUpper().Equals("PUT"))
			{
				return HTTPMethod.PUT;
			}
			if (method.ToUpper().Equals("DELETE"))
			{
				return HTTPMethod.DELETE;
			}
			throw new ArgumentException(method + " is not a valid HTTPMethod type");
		}
	}
}
