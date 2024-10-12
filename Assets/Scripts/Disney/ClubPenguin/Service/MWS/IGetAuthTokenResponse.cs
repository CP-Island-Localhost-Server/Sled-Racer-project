using Disney.ClubPenguin.Service.MWS.Domain;
using Disney.HTTP.Client;

namespace Disney.ClubPenguin.Service.MWS
{
	public interface IGetAuthTokenResponse : IHTTPResponse
	{
		AuthData AuthData { get; }

		ResponseError ResponseError { get; }
	}
}
