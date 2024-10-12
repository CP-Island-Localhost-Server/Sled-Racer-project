using Disney.ClubPenguin.CPModuleUtils;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer
{
	public class PauseMenuController : BaseMenuController
	{
		public Toggle MusicToggle;

		public Toggle SoundToggle;

		public Button ContinueButton;

		private bool ButtonsInitialized;

		private ISFX sfx;

		private IMusic music;

		protected override void VStart()
		{
			sfx = Service.Get<IAudio>().SFX;
			music = Service.Get<IAudio>().Music;
			HardwareBackButtonDispatcher.SetTargetClickHandler(ContinueButton);
			InitButtonStates();
			PauseAudio();
		}

		private void PauseAudio()
		{
			sfx.Pause(SFXEvent.SFX_Race);
			sfx.MuteRaceSFX(mute: true);
			music.Volume = 0.5f;
		}

		private void UnpauseAudio()
		{
			sfx.Unpause(SFXEvent.SFX_Race);
			sfx.MuteRaceSFX(mute: false);
			music.Volume = 1f;
		}

		protected override void Init()
		{
		}

		public void QuitClicked()
		{
			if (!Service.Get<UIManager>().IsLoadingPanel)
			{
				UnpauseAudio();
				sfx.StopRaceSFX();
				Service.Get<EventDataService>().SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.MainMenuRequest));
				Service.Get<TrackManager>().ResetEnd();
			}
		}

		public void ContinueClicked()
		{
			if (!Service.Get<UIManager>().IsLoadingPanel)
			{
				UnpauseAudio();
				Service.Get<EventDataService>().SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.RequestUnpause));
				ContinueButton.enabled = false;
			}
		}

		public void UIEvent_MusicVolumeChanged()
		{
			if (ButtonsInitialized)
			{
				if (MusicToggle.isOn)
				{
					Service.Get<IBILogging>().ButtonPressed(BIButton.MUTE_MUSIC, BIScreen.PAUSE, BIScreen.PAUSE);
					music.Mute();
				}
				else
				{
					Service.Get<IBILogging>().ButtonPressed(BIButton.UNMUTE_MUSIC, BIScreen.PAUSE, BIScreen.PAUSE);
					music.UnMute();
				}
			}
		}

		public void UIEvent_EffectsVolumeChanged()
		{
			ISFX sFX = Service.Get<IAudio>().SFX;
			if (ButtonsInitialized)
			{
				if (SoundToggle.isOn)
				{
					Service.Get<IBILogging>().ButtonPressed(BIButton.MUTE_SOUND, BIScreen.PAUSE, BIScreen.PAUSE);
					sFX.Mute();
				}
				else
				{
					Service.Get<IBILogging>().ButtonPressed(BIButton.UNMUTE_SOUND, BIScreen.PAUSE, BIScreen.PAUSE);
					sFX.UnMute();
				}
			}
		}

		private void InitButtonStates()
		{
			MusicToggle.GetComponent<ToggleAudioTrigger>().enabled = false;
			MusicToggle.isOn = music.IsMuted();
			MusicToggle.GetComponent<ToggleAudioTrigger>().enabled = true;
			SoundToggle.GetComponent<ToggleAudioTrigger>().enabled = false;
			SoundToggle.isOn = sfx.IsMuted();
			SoundToggle.GetComponent<ToggleAudioTrigger>().enabled = true;
			ButtonsInitialized = true;
		}
	}
}
