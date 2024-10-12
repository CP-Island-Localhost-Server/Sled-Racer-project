using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class PlayerRollAudioBehaviour : MonoBehaviour
	{
		private PlayerController player;

		private bool isTumbling;

		private void Awake()
		{
		}

		private void OnTumbleStart()
		{
			if (!isTumbling)
			{
				Service.Get<IAudio>().SFX.Play(SFXEvent.SFX_PlayerRoll);
				isTumbling = true;
			}
		}

		private void OnTumbleEnd()
		{
			if (isTumbling)
			{
				Service.Get<IAudio>().SFX.AdvanceSequence(SFXEvent.SFX_PlayerRoll);
				isTumbling = false;
			}
		}
	}
}
