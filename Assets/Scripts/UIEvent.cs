using System;

public class UIEvent : EventArgs
{
	public enum uiGameEvent
	{
		Quit,
		Play,
		RequestPause,
		RequestUnpause,
		AccountRetrieved,
		LoginRequest,
		LoginSuccess,
		LoginFailed,
		LoginCancelled,
		Logout,
		Back,
		AboutMembershipRequest,
		SettingsRequest,
		AffiliateRequest,
		DisneyAffiliateRequest,
		WebViewRequest,
		OpenExternalURL,
		MainMenuRequest,
		LeaderboardRequest,
		FriendHighScoresLoaded,
		RequestDisablePanelInput,
		RequestEnablePanelInput,
		SelectBoosts,
		BoostEquiped,
		BoostUnequiped,
		LoadingStarting,
		LoadingComplete,
		IAPRequest,
		RestorePurchaseRequest,
		SwitchingUser
	}

	private readonly uiGameEvent eventType;

	private readonly object eventData;

	public uiGameEvent type => eventType;

	public object data => eventData;

	public UIEvent(uiGameEvent _type)
	{
		eventType = _type;
	}

	public UIEvent(uiGameEvent _type, object _data)
	{
		eventType = _type;
		eventData = _data;
	}
}
