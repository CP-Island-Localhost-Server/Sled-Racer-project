using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class FluffyController : MonoBehaviour
	{
		private enum FluffyState
		{
			Hiding,
			Visible
		}

		public Animator AnimationController;

		private PlayerController Player;

		private float storedSFXVolume;

		private ConfigController config;

		private FluffyState state;

		private void Awake()
		{
			Player = GameObject.Find("MainPlayer").GetComponent<PlayerController>();
		}

		private void OnEnable()
		{
			state = FluffyState.Hiding;
			config = Service.Get<ConfigController>();
		}

		private void Update()
		{
			if (state == FluffyState.Hiding && Player.currentLifeState != PlayerController.PlayerLifeState.Done)
			{
				if (Vector3.Distance(base.transform.position, Player.transform.position) < config.FluffyDistanceBeforePopup)
				{
					state = FluffyState.Visible;
					AnimationController.SetBool("show", value: true);
					Service.Get<IAudio>().SFX.Play(SFXEvent.SFX_FluffyPopUp);
				}
			}
			else if (state == FluffyState.Visible && Player.currentLifeState == PlayerController.PlayerLifeState.Done)
			{
				state = FluffyState.Hiding;
				AnimationController.SetBool("show", value: false);
			}
		}
	}
}
