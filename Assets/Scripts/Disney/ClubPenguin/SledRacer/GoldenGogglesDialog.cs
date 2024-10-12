using Disney.ClubPenguin.CPModuleUtils;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer
{
	public class GoldenGogglesDialog : MonoBehaviour
	{
		public Button CloseButton;

		private void Start()
		{
			HardwareBackButtonDispatcher.SetTargetClickHandler(CloseButton);
		}

		public void LaunchClubPenguinApp()
		{
			AppIconsController.TrackedApp trackedApp = default(AppIconsController.TrackedApp);
			trackedApp.id = AppIconsController.AppId.CLUB_PENGUIN;
			trackedApp.location = AppIconsController.IconLocation.GOLDEN_ITEM;
			CloseDialog();
			Service.Get<UIManager>().OpenExternalApp(trackedApp);
		}

		public void CloseDialog()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
