using Disney.ClubPenguin.Service.DirectoryService.Domain;
using Disney.HTTP.Client;

namespace Disney.ClubPenguin.Service.DirectoryService
{
	public interface IGetCompatibilityResponse : IHTTPResponse
	{
		CompatibilityStatus CompatibilityStatus { get; }
	}
}
