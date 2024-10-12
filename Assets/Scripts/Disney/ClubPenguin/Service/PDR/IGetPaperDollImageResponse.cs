using Disney.HTTP.Client;

namespace Disney.ClubPenguin.Service.PDR
{
	public interface IGetPaperDollImageResponse : IHTTPResponse
	{
		byte[] AvatarImageBytes { get; }
	}
}
