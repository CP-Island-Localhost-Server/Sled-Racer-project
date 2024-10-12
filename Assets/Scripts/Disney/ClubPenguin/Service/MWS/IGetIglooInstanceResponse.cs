using Disney.HTTP.Client;

namespace Disney.ClubPenguin.Service.MWS
{
	public interface IGetIglooInstanceResponse : IHTTPResponse
	{
		string IglooInstanceId { get; }
	}
}
