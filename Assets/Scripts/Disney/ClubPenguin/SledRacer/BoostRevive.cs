using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class BoostRevive : Boost, IBoost
	{
		public GameObject EffectPrefab;

		private GameObject effectInstance;

		private bool canUse = true;

		private float usedDisplayDelay;

		private ConfigController config;

		public BoostRevive(PlayerController _player)
			: base(_player)
		{
			config = Service.Get<ConfigController>();
			myPhase = BoostType.Death;
			usedDisplayDelay = Service.Get<ConfigController>().BoostReviveUsedDisplayTime;
			if (effectInstance != null)
			{
				effectInstance = (GameObject)Object.Instantiate(EffectPrefab);
				effectInstance.transform.parent = _player.transform;
				effectInstance.SetActive(false);
			}
		}

		public override void Trigger()
		{
			if (!canUse)
			{
				if (effectInstance != null)
				{
					effectInstance.SetActive(false);
				}
				return;
			}
			active = true;
			if (canUse)
			{
				Vector3 position = player.transform.position;
				Service.Get<IAudio>().SFX.Play(SFXEvent.SFX_Boost_Revive);
				Service.Get<IAudio>().Music.Play(MusicTrack.Revive);
				//position.y += config.BoostReviveHeight;
				//player.transform.position = position;
				player.ChangeStateToRevive();
				player.ChangeLifeState(PlayerController.PlayerLifeState.Invincible);
				canUse = false;
			}
		}

		public override void Update()
		{
			if (active)
			{
				usedDisplayDelay -= Time.deltaTime;
				if (usedDisplayDelay <= 0f)
				{
					player.ChangeLifeState(PlayerController.PlayerLifeState.Alive);
					active = false;
					used = true;
				}
			}
		}

		public override void Abort()
		{
		}
	}
}
