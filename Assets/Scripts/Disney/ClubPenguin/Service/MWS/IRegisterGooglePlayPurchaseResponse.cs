using System.Collections.Generic;
using Disney.ClubPenguin.Service.MWS.Domain;
using Disney.HTTP.Client;

namespace Disney.ClubPenguin.Service.MWS
{
	public interface IRegisterGooglePlayPurchaseResponse : IHTTPResponse
	{
		IList<ProductPurchase> Result { get; }
	}
}