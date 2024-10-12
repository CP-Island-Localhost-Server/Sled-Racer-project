using System;

namespace Disney.ClubPenguin.Service.PDR
{
	public interface IPDRClient
	{
		IGetPaperDollImageResponse GetPaperDollImage(string swid, int size, bool flag, bool photo, string language, Action<IGetPaperDollImageResponse> responseHandler = null);
	}
}
