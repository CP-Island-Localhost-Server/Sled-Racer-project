using Disney.ClubPenguin.Service.MWS.Domain;
using Disney.HTTP.Client;

namespace Disney.ClubPenguin.Service.MWS
{
	public interface ICreateAccountResponse : IHTTPResponse
	{
		Account Account { get; }

		ResponseError ResponseError { get; }
	}
}
