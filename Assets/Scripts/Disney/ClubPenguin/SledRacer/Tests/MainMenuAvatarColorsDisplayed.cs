using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer.Tests
{
	public class MainMenuAvatarColorsDisplayed : UIIntegrationTest
	{
		private MainMenuController controller;

		protected override void RunTest()
		{
			controller = panel.GetComponent<MainMenuController>();
			CheckAvatarSpritesNotNull();
			CheckShowAvatars();
		}

		private void CheckAvatarSpritesNotNull()
		{
			IntegrationTest.Assert(AvatarUtil.COLOR_TO_RESOURCE_MAP != null);
		}

		private void CheckShowAvatars()
		{
			MethodInfo method = typeof(MainMenuController).GetMethod("ShowAvatar", BindingFlags.Instance | BindingFlags.NonPublic);
			IntegrationTest.Assert(method != null);
			for (int i = 0; i < AvatarUtil.COLOR_TO_RESOURCE_MAP.Length; i++)
			{
				UnityEngine.Debug.Log("Checking Icon index " + i);
				PlayerData playerData = Service.Get<PlayerDataService>().PlayerData;
				IntegrationTest.Assert(playerData != null);
				Service.Get<PlayerDataService>().PlayerData.Account.Colour = i;
				method.Invoke(controller, null);
				FieldInfo field = typeof(MainMenuController).GetField("avatarImage", BindingFlags.Instance | BindingFlags.NonPublic);
				Image x = field.GetValue(controller) as Image;
				IntegrationTest.Assert(x != null);
			}
		}
	}
}
