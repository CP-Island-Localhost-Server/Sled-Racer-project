namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class ResponseError
	{
		public class ErrorResponseObj
		{
			public int errorCode { get; set; }

			public string message { get; set; }
		}

		public ErrorResponseObj ErrorResponse { get; set; }

		public string UserSuggestion { get; set; }
	}
}
