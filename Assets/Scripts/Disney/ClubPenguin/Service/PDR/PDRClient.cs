using System;
using Disney.ClubPenguin.Service.DirectoryService;
using Disney.HTTP.Client;
using Disney.HTTP.Client.UnityHTTP;
using strange.extensions.injector.api;

namespace Disney.ClubPenguin.Service.PDR
{
	[Implements(typeof(IPDRClient), InjectionBindingScope.CROSS_CONTEXT)]
	public class PDRClient : IPDRClient
	{
		public class GetPaperDollImageResponse : HTTPResponseDelegate, IHTTPResponse, IGetPaperDollImageResponse
		{
			private byte[] avatarImageBytes;

			public byte[] AvatarImageBytes
			{
				get
				{
					return avatarImageBytes;
				}
			}

			public GetPaperDollImageResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				avatarImageBytes = httpResponse.Bytes;
			}
		}

		private static IPDRClient instance = new PDRClient();

		private IDirectoryServiceClient _directoryServiceClient;

		private IHTTPRequestFactory _httpRequestFactory;

		public static IPDRClient Instance
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

		[Inject]
		public IDirectoryServiceClient directoryServiceClient
		{
			get
			{
				if (_directoryServiceClient == null)
				{
					return DirectoryServiceClient.Instance;
				}
				return _directoryServiceClient;
			}
			set
			{
				_directoryServiceClient = value;
			}
		}

		[Inject]
		public IHTTPRequestFactory httpRequestFactory
		{
			get
			{
				if (_httpRequestFactory == null)
				{
					return UnityHTTPRequestFactory.Instance;
				}
				return _httpRequestFactory;
			}
			set
			{
				_httpRequestFactory = value;
			}
		}

		public string CellophaneToken
		{
			get
			{
				if (directoryServiceClient.IsDev || directoryServiceClient.IsInt || directoryServiceClient.IsQA)
				{
					return "0869F69C-56CC-4EEB-9D5E-D3887F971033:AF5DC9E77FB4FE15F8C051ABC544435A410B46B725CE94B5";
				}
				return "9B407F96-418B-4E0B-93DB-3AD33CC7D72E:205EF7823B24EE5277E318E061E5557F4648F1BF4CCFB457";
			}
		}

		public IGetPaperDollImageResponse GetPaperDollImage(string swid, int size, bool flag, bool photo, string language, Action<IGetPaperDollImageResponse> responseHandler = null)
		{
			if (responseHandler == null)
			{
				string serviceURL = directoryServiceClient.GetServiceURL("avatar-renderer-service-cellophane");
				string text = "/" + swid + "/cp?size=" + size + "&flag=" + flag + "&photo=" + photo + "&language=" + language;
				string uri = serviceURL + text;
				IHTTPRequest iHTTPRequest = httpRequestFactory.CreateRequest("GET", uri);
				iHTTPRequest.Headers.Add("Authorization", "FD " + CellophaneToken);
				IHTTPResponse httpResponse2 = iHTTPRequest.Execute();
				return new GetPaperDollImageResponse(httpResponse2);
			}
			directoryServiceClient.GetServiceURL("avatar-renderer-service-cellophane", delegate(string serviceUrl)
			{
				string text2 = "/" + swid + "/cp?size=" + size + "&flag=" + flag + "&photo=" + photo + "&language=" + language;
				string uri2 = serviceUrl + text2;
				IHTTPRequest iHTTPRequest2 = httpRequestFactory.CreateRequest("GET", uri2);
				iHTTPRequest2.Headers.Add("Authorization", "FD " + CellophaneToken);
				iHTTPRequest2.ExecuteAsync(delegate(IHTTPResponse httpResponse)
				{
					responseHandler(new GetPaperDollImageResponse(httpResponse));
				});
			}, delegate(IHTTPResponse errorResponse)
			{
				responseHandler(new GetPaperDollImageResponse(errorResponse));
			});
			return null;
		}
	}
}
