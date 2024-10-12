using System;

namespace Disney.ClubPenguin.Service.MWS
{
	public class Configuration
	{
		public string host;

		public int port;

		public string userName;

		public string password;

		public string rootPath = "/";

		public string language;

		public NetworkID network;

		public NetworkID viewNetwork;

		public ProductID product;

		public string accessToken;

		public Uri GetBaseUri()
		{
			UriBuilder uriBuilder = new UriBuilder("http", host);
			uriBuilder.Port = port;
			uriBuilder.Path = rootPath;
			return uriBuilder.Uri;
		}
	}
}
