using Disney.ClubPenguin.Service.MWS.Domain.Likes;
using Disney.HTTP.Client;

namespace Disney.ClubPenguin.Service.MWS
{
	public interface IGetLikesResponse : IHTTPResponse
	{
		Entity Entity { get; }
	}
}
