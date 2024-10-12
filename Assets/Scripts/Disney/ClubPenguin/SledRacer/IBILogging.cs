using System.Collections.Generic;

namespace Disney.ClubPenguin.SledRacer
{
	public interface IBILogging
	{
		void StartGame(IList<BoostManager.AvailableBoosts> equippedBoosts);

		void EndGame(BIGameObjectType killedBy, int finalScore);

		void ButtonPressed(BIButton button, BIScreen startingSceen, BIScreen targetSceen);

		void BeatFriendsHighScore(bool isTopFriend);

		void OpenExternalApp(AppIconsController.AppId appId);

		void DisneyRefferalStoreIconShown();

		void AdShown(string adId);

		void ParentGateClosed(int age);

		void RewardItemGranted(int itemId, string itemType, string itemName);
	}
}
