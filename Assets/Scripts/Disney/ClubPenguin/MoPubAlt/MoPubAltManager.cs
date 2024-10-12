using Disney.ClubPenguin.CPModuleUtils;
using Disney.ClubPenguin.SledRacer;
using System;
using UnityEngine;

namespace Disney.ClubPenguin.MoPubAlt
{
	public class MoPubAltManager
	{
		private const string adPrefabPath = "Prefabs/MoPubAltOverlayAd";

		private const int adCount = 5;

		private MoPubAltController adContext;

		private UIEvent.uiGameEvent clickEvent;

		public event Action OnClose;

		public void ShowAd(Transform parent, string language)
		{
			MoPubAltController original = Resources.Load<MoPubAltController>("Prefabs/MoPubAltOverlayAd");
			adContext = (UnityEngine.Object.Instantiate(original) as MoPubAltController);
			adContext.OnClose += CleanUpContext;
			adContext.OnAdClicked += OnAdClicked;
			adContext.transform.SetParent(parent, worldPositionStays: false);
			setRandomAd(language);
		}

		private void setRandomAd(string language)
		{
			language = language.ToUpper();
			int num = UnityEngine.Random.Range(1, 6);
			string path = "Adverts/" + language + "/Ad" + num + "_" + language + ".jpg";
			TextAsset textAsset = Resources.Load<TextAsset>(path);
			Texture2D texture2D = new Texture2D(4, 4, TextureFormat.RGB24, false);
			texture2D.LoadImage(textAsset.bytes);
			Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
			adContext.AdImage.sprite = sprite;
			if (num > 2)
			{
				clickEvent = UIEvent.uiGameEvent.AffiliateRequest;
				adContext.SetFinalScreen(BIScreen.LEAVING_APP);
			}
			else
			{
				clickEvent = UIEvent.uiGameEvent.AboutMembershipRequest;
				adContext.SetFinalScreen(BIScreen.STORE_FRONT);
			}
			Disney.ClubPenguin.SledRacer.Service.Get<IBILogging>().AdShown("advert" + num);
		}

		private void OnAdClicked()
		{
			AppIconsController.TrackedApp trackedApp = default(AppIconsController.TrackedApp);
			trackedApp.id = AppIconsController.AppId.CLUB_PENGUIN;
			trackedApp.location = AppIconsController.IconLocation.ADVERT;
			Disney.ClubPenguin.SledRacer.Service.Get<EventDataService>().SendUIEvent(this, new UIEvent(clickEvent, trackedApp));
			CleanUpContext();
		}

		private void CleanUpContext()
		{
			adContext.OnClose -= CleanUpContext;
			adContext.OnAdClicked -= OnAdClicked;
			GameObjectUtil.CleanupImageReferences(adContext.gameObject);
			UnityEngine.Object.Destroy(adContext.gameObject);
			adContext = null;
			if (this.OnClose != null)
			{
				this.OnClose();
			}
		}
	}
}
