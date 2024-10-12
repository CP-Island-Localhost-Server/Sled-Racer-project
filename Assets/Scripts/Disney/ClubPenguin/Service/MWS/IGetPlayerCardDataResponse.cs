using Disney.ClubPenguin.Service.MWS.Domain;
using Disney.HTTP.Client;

namespace Disney.ClubPenguin.Service.MWS
{
	public interface IGetPlayerCardDataResponse : IHTTPResponse
	{
		PlayerCardData PlayerCardData { get; }
	}
}
