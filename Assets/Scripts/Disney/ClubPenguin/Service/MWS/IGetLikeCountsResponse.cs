using Disney.ClubPenguin.Service.MWS.Domain.Likes;
using Disney.HTTP.Client;

namespace Disney.ClubPenguin.Service.MWS
{
	public interface IGetLikeCountsResponse : IHTTPResponse
	{
		Entity Entity { get; }
	}
}
