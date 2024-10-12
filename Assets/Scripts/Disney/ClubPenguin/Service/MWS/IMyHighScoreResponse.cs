using Disney.HTTP.Client;

namespace Disney.ClubPenguin.Service.MWS
{
	public interface IMyHighScoreResponse : IHTTPResponse
	{
		int Score { get; }
	}
}
