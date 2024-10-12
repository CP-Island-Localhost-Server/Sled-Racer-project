using Disney.ClubPenguin.CPModuleUtils;
using Disney.ClubPenguin.SledRacer;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.MoPubAlt
{
	public class MoPubAltController : MonoBehaviour
	{
		public Button CloseButton;

		public Image AdImage;

		public BILogButtonNavigation buttonLogger;

		private string adId;

		private BIScreen finalScreen;

		public event Action OnClose;

		public event Action OnAdClicked;

		private void Start()
		{
			HardwareBackButtonDispatcher.SetTargetClickHandler(CloseButton);
		}

		public void SetFinalScreen(BIScreen screen)
		{
			finalScreen = screen;
		}

		public void OnCloseClicked()
		{
			if (this.OnClose != null)
			{
				this.OnClose();
			}
		}

		public void OnAdButtonClicked()
		{
			buttonLogger.Log();
			buttonLogger.TargetSceen = finalScreen;
			this.OnAdClicked();
		}
	}
}
