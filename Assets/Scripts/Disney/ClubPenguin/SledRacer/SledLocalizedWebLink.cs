using DevonLocalization;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer
{
	[RequireComponent(typeof(Button))]
	public class SledLocalizedWebLink : LocalizedWebLink
	{
		public enum OpenMode
		{
			InternalWebView,
			ExternalApp
		}

		public OpenMode Mode = OpenMode.ExternalApp;

		protected override void OpenURL()
		{
			switch (Mode)
			{
			case OpenMode.ExternalApp:
				Service.Get<EventDataService>().SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.OpenExternalURL, base.URL));
				break;
			case OpenMode.InternalWebView:
				Service.Get<EventDataService>().SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.WebViewRequest, base.URL));
				break;
			}
		}
	}
}
