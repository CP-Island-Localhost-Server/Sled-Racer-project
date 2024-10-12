using System.Runtime.InteropServices;
using UnityEngine;

public class musicOBJ
{
	#if UNITY_IPHONE
		[DllImport("__Internal")]
		private static extern bool _IsMusicPlaying();
	#endif

	public static bool isMusicPlaying()
	{
		#if UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				return _IsMusicPlaying();
			}
		#endif
		return false;
	}
}
