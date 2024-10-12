using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class BoostInvulnerable : Boost, IBoost
	{
		public GameObject EffectPrefab;

		private float duration;

		private float warnTime;

		private float activeTime;

		private GameObject effectInstance;

		private Animator effectAnimator;

		private ConfigController config;

		public BoostInvulnerable(PlayerController _player, GameObject _stateObject, float _time)
			: base(_player)
		{
			config = Service.Get<ConfigController>();
			myPhase = BoostType.Start;
			duration = _time;
			warnTime = duration - config.BoostInvulnerableWarningDuration;
			if (warnTime < 0f)
			{
				warnTime = 0f;
			}
			if (_stateObject != null)
			{
				effectInstance = _stateObject;
				effectInstance.SetActive(value: false);
				effectAnimator = effectInstance.GetComponent<Animator>();
				DevTrace("Animator: " + effectAnimator.name);
			}
			DevTrace("BoostInvulnerable Constructed");
		}

		public override void Execute()
		{
			DevTrace("BoostInvulnerable Execute");
			if (effectInstance != null)
			{
				effectInstance.SetActive(value: true);
			}
			DevTrace("Still have animator? " + (effectAnimator != null));
			effectAnimator.SetTrigger("InvulnerabilityON");
			Service.Get<IAudio>().SFX.Play(SFXEvent.SFX_Boost_Invincitube);
			ending = false;
		}

		public override void Trigger()
		{
			if (!active)
			{
				activeTime = 0f;
				player.ChangeLifeState(PlayerController.PlayerLifeState.Invincible);
				Service.Get<IAudio>().SFX.AdvanceSequence(SFXEvent.SFX_Boost_Invincitube);
			}
			active = true;
			effectAnimator.SetTrigger("InvulnerabilityPOWERUP");
		}

		public override void Update()
		{
			if (!active)
			{
				return;
			}
			activeTime += Time.deltaTime;
			DevTrace("Time Remaining: " + (duration - activeTime));
			if (!ending && activeTime > warnTime && activeTime < duration)
			{
				ending = true;
			}
			if (activeTime > duration - config.BoostInvulnerableShiledOffTime && duration != 0f)
			{
				effectAnimator.SetTrigger("InvulnerabilityOFF");
				Service.Get<IAudio>().SFX.AdvanceSequence(SFXEvent.SFX_Boost_Invincitube);
			}
			if (activeTime > duration && duration != 0f)
			{
				DevTrace("BoostInvulnerable Deactivate runTime=" + activeTime + ", Duration=" + duration);
				effectAnimator.ResetTrigger("InvulnerabilityOFF");
				active = false;
				ending = false;
				used = true;
				activeTime = 0f;
				if (effectInstance != null)
				{
					effectInstance.SetActive(value: false);
				}
				if (!player.isIcePatchInvulnerable())
				{
					player.ChangeLifeState(PlayerController.PlayerLifeState.Alive);
				}
			}
		}

		public override void Abort()
		{
			if (active)
			{
				effectAnimator.SetTrigger("InvulnerabilityOFF");
				active = false;
				ending = false;
				activeTime = 0f;
				if (effectInstance != null)
				{
					effectInstance.SetActive(value: false);
				}
			}
		}
	}
}
