using System.Collections.Generic;
using Disney.ClubPenguin.Service.MWS.Domain;
using Disney.HTTP.Client;

namespace Disney.ClubPenguin.Service.MWS
{
	public interface IGetIgloosResponse : IHTTPResponse
	{
		List<IglooVillageEntry> Igloos { get; }
	}
}
