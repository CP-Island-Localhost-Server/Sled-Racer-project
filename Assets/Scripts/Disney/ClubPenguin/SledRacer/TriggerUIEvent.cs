using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class TriggerUIEvent : MonoBehaviour
	{
		public UIEvent.uiGameEvent ButtonUIEvent;

		public AppIconsController.AppId AppId;

		public AppIconsController.IconLocation Location;

		public void ButtonClicked()
		{
			object data = null;
			if (ButtonUIEvent == UIEvent.uiGameEvent.AffiliateRequest)
			{
				AppIconsController.TrackedApp trackedApp = default(AppIconsController.TrackedApp);
				trackedApp.id = AppId;
				trackedApp.location = Location;
				data = trackedApp;
			}
			Service.Get<EventDataService>().SendUIEvent(this, new UIEvent(ButtonUIEvent, data));
		}
	}
}
