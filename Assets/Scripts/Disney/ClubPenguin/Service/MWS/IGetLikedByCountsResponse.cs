using Disney.ClubPenguin.Service.MWS.Domain.Likes;
using Disney.HTTP.Client;

namespace Disney.ClubPenguin.Service.MWS
{
	public interface IGetLikedByCountsResponse : IHTTPResponse
	{
		Entity Entity { get; }
	}
}
