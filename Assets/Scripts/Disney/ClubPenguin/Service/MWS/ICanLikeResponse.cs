using Disney.ClubPenguin.Service.MWS.Domain.Likes;
using Disney.HTTP.Client;

namespace Disney.ClubPenguin.Service.MWS
{
	public interface ICanLikeResponse : IHTTPResponse
	{
		CanLike CanLike { get; }
	}
}
