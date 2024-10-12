using Disney.ClubPenguin.Service.MWS.Domain.Likes;
using Disney.HTTP.Client;

namespace Disney.ClubPenguin.Service.MWS
{
	public interface IUnLikeResponse : IHTTPResponse
	{
		Entity Entity { get; }
	}
}
