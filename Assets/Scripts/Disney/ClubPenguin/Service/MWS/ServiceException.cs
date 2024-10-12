using System;

namespace Disney.ClubPenguin.Service.MWS
{
	public class ServiceException : Exception
	{
		private int errorCode;

		private string errorMessage;
	}
}
