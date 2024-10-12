using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public interface ISFX : IAudioGroup
	{
		void Stop(SFXEvent sfx);

		void Play(SFXEvent sfx);

		void Play(SFXEvent sfx, GameObject go);

		void Pause(SFXEvent sfx);

		void Unpause(SFXEvent sfx);

		void AdvanceSequence(SFXEvent sfx);

		void ResetSequence(SFXEvent sfx);

		void MuteRaceSFX(bool mute);

		void StopRaceSFX();
	}
}
