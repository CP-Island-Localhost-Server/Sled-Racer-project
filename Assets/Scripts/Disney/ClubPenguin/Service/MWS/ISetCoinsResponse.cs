using Disney.HTTP.Client;

namespace Disney.ClubPenguin.Service.MWS
{
	public interface ISetCoinsResponse : IHTTPResponse
	{
		int Coins { get; }
	}
}
