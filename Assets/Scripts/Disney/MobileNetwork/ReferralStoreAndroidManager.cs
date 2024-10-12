using UnityEngine;

namespace Disney.MobileNetwork
{
	public class ReferralStoreAndroidManager : ReferralStoreManager
	{
		public override void Show()
		{
			Logger.LogDebug(this, "Referral store show called in Android");
		}
	}
}
