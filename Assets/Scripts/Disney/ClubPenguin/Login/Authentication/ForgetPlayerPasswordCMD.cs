namespace Disney.ClubPenguin.Login.Authentication
{
	public class ForgetPlayerPasswordCMD
	{
		private string userName;

		public ForgetPlayerPasswordCMD(string userName)
		{
			this.userName = userName;
		}

		public void Execute()
		{
			SavedPlayerCollection savedPlayerCollection = new SavedPlayerCollection();
			savedPlayerCollection.LoadFromDisk();
			SavedPlayerData savedPlayerData = savedPlayerCollection.SavedPlayers.Find((SavedPlayerData spd) => spd.UserName == userName);
			if (savedPlayerData != null)
			{
				savedPlayerData.Password = string.Empty;
				savedPlayerCollection.SaveToDisk();
			}
		}
	}
}
