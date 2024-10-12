namespace Disney.ClubPenguin.Login.BI
{
	public interface ILoginBIUtils
	{
		string ContextName
		{
			get;
			set;
		}

		void SendPlayerInfo(long playerID, string playerName);

		void SendPageviewOnLogin(BiPageNames pageName, long passedPlayerID = 0);

		void SendAccoundCreated(long playerID, string playerName);

		void SendAgeEnteredGameAction(int age, long passedPlayerID = 0);
	}
}
