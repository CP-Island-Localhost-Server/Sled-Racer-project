using System;

[Serializable]
public class SavedPlayerData
{
	public const string USER_NAME_PROPERTY = "username";

	public const string DISPLAY_NAME_PROPERTY = "displayName";

	public const string PLAYER_SWID_PROPERTY = "playerSwid";

	public string UserName;

	public string DisplayName;

	public string Swid;

	public byte[] PaperDollBytes;

	[NonSerialized]
	public string Password;
}
