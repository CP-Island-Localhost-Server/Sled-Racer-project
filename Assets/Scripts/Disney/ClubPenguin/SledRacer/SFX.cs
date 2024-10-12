using Fabric;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class SFX : AudioGroup, IAudioGroup, ISFX
	{
		private Dictionary<SFXEvent, string> eventNames;

		public GroupComponent raceGroup;

		public SFX()
			: base("Audio_All_SFX")
		{
			CacheEventNames();
			raceGroup = (fabric.GetComponentByName("Audio_All_SFX_Race") as GroupComponent);
			if (raceGroup == null)
			{
				UnityEngine.Debug.LogError("Could not find race sfx audio group");
			}
		}

		public void Stop(SFXEvent sfx)
		{
			events.PostEvent(eventNames[sfx], EventAction.StopSound);
		}

		public void Play(SFXEvent sfx)
		{
			events.PostEvent(eventNames[sfx], EventAction.PlaySound);
		}

		public void Play(SFXEvent sfx, GameObject go)
		{
			events.PostEvent(eventNames[sfx], EventAction.PlaySound, go);
		}

		public void Pause(SFXEvent sfx)
		{
			events.PostEvent(eventNames[sfx], EventAction.PauseSound);
		}

		public void Unpause(SFXEvent sfx)
		{
			events.PostEvent(eventNames[sfx], EventAction.UnpauseSound);
		}

		public void AdvanceSequence(SFXEvent sfx)
		{
			events.PostEvent(eventNames[sfx], EventAction.AdvanceSequence);
		}

		public void ResetSequence(SFXEvent sfx)
		{
			events.PostEvent(eventNames[sfx], EventAction.ResetSequence);
		}

		public void MuteRaceSFX(bool mute)
		{
			if (mute)
			{
				raceGroup.Volume = 0f;
			}
			if (!mute)
			{
				raceGroup.Volume = 1f;
			}
		}

		public void StopRaceSFX()
		{
			raceGroup.Stop(stopInstances: true, forceStop: true, ignoreFade: true);
		}

		private void CacheEventNames()
		{
			eventNames = new Dictionary<SFXEvent, string>();
			foreach (object value in Enum.GetValues(typeof(SFXEvent)))
			{
				SFXEvent sFXEvent = (SFXEvent)(int)value;
				eventNames.Add(sFXEvent, sFXEvent.ToString());
			}
		}
	}
}
