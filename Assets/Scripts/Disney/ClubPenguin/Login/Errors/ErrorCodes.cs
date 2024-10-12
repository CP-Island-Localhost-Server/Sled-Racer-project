namespace Disney.ClubPenguin.Login.Errors
{
	public class ErrorCodes
	{
		public const string CONNECTION_ERROR = "0";

		public const string PASSWORD_SHORT = "1";

		public const string PASSWORD_LONG = "2";

		public const string USERNAME_LENGTH = "3";

		public const string PASSWORD_MISMATCH = "4";

		public const string PASSWORD_CONTAINS_NAME = "5";

		public const string TERMS_OF_USE_UNCHECKED = "6";

		public const string RULES_UNCHECKED = "7";

		public const string PRIVACY_UNCHECKED = "8";

		public const string EMAIL_BAD_FORMAT = "9";

		public const string USERNAME_TOO_MANY_SPACES = "10";

		public const int INTERNAL_SERVER_ERROR = 500;

		public const int ACCOUNT_BANNED = 403;

		public const int USERNAME_TAKEN = 409;

		public const int EMAIL_BANNED = -32275;

		public const int USERNAME_INVALID_CHARS = -32283;

		public const int USERNAME_BANNED = -32284;

		public const int PASSWORD_EASY = -32292;

		public const int EMAIL_BANNED_DOMAIN = -32296;

		public const int EMAIL_TOO_MANY_PENGUINS = -32297;

		public const int EMAIL_INVALID_CHARS = -32295;

		public const int EMAIL_BANNED_ISP = -32299;
	}
}
