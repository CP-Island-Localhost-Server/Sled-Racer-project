using Disney.ClubPenguin.Service.MWS.Domain;
using Disney.HTTP.Client;

namespace Disney.ClubPenguin.Service.MWS
{
	public interface ICreatePuffleResponse : IHTTPResponse
	{
		Puffle Puffle { get; }
	}
}
