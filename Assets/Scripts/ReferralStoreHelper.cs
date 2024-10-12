using Disney.MobileNetwork;
using UnityEngine;

public class ReferralStoreHelper : MonoBehaviour
{
	public ReferralStoreManager EditorReferralStore;

	public ReferralStoreAndroidManager AndroidReferralStore;

	public ReferralStoreIOSManager IOSReferralStore;

	private ReferralStoreManager runtimeStore;

	public void Awake()
	{
		switch (Application.platform)
		{
		case RuntimePlatform.Android:
			runtimeStore = AndroidReferralStore;
			break;
		case RuntimePlatform.IPhonePlayer:
			runtimeStore = IOSReferralStore;
			break;
		default:
			runtimeStore = EditorReferralStore;
			break;
		}
	}

	public void Show()
	{
		runtimeStore.Show();
	}
}
