using UnityEngine;

namespace Disney.ClubPenguin.WebPageViewer
{
	public class NativeWebView
	{
		public void Show(string URL, Rect pos)
		{
			EtceteraAndroid.inlineWebViewShow(URL, (int)pos.x, (int)pos.y, (int)pos.width, (int)pos.height);
		}

		public void Close()
		{
			EtceteraAndroid.inlineWebViewClose();
		}
	}
}
