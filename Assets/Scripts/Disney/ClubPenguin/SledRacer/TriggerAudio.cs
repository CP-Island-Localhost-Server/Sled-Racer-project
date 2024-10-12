using Disney.ClubPenguin.Common.Utils;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class TriggerAudio : MonoBehaviour
	{
		public SFXEvent TriggerEnterSfxEvent;

		public SFXEvent TriggerExitSfxEvent;

		public SFXEvent CollisionEnterSfxEvent;

		public SFXEvent CollisionExitSfxEvent;

		public LayerMask Mask;

		public bool EmitFromOther = true;

		private void Start()
		{
		}

		private void OnTriggerEnter(Collider other)
		{
			CheckPlaySound(other, TriggerEnterSfxEvent);
		}

		private void OnTriggerExit(Collider other)
		{
			CheckPlaySound(other, TriggerExitSfxEvent);
		}

		private void OnCollisionEnter(Collision other)
		{
			CheckPlaySound(other.collider, CollisionEnterSfxEvent);
		}

		private void OnCollisionExit(Collision other)
		{
			CheckPlaySound(other.collider, CollisionExitSfxEvent);
		}

		private void CheckPlaySound(Collider other, SFXEvent sfx)
		{
			if (Mask.IsSet(other.gameObject.layer))
			{
				GameObject go = (!EmitFromOther) ? base.gameObject : other.gameObject;
				Service.Get<IAudio>().SFX.Play(sfx, go);
			}
		}
	}
}
