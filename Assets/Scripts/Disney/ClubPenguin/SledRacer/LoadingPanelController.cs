using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class LoadingPanelController : MonoBehaviour
	{
		private int loadingComponents;

		public bool Loading => loadingComponents > 0;

		public void AddLoadingComponent(object component, bool incrementRefCount = true)
		{
			if (incrementRefCount)
			{
				loadingComponents++;
			}
			if (loadingComponents == 1 || (loadingComponents == 0 && !incrementRefCount))
			{
				UnityEngine.Debug.Log("SHOW LOADING SCREEN");
				base.gameObject.SetActive(value: true);
				Service.Get<EventDataService>().SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.LoadingStarting));
			}
		}

		public void RemoveLoadingComponent(object component)
		{
			if (loadingComponents > 0)
			{
				loadingComponents--;
			}
			if (loadingComponents == 0)
			{
				UnityEngine.Debug.Log("HIDE LOADING SCREEN");
				base.gameObject.SetActive(value: false);
				Service.Get<EventDataService>().SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.LoadingComplete));
			}
		}
	}
}
