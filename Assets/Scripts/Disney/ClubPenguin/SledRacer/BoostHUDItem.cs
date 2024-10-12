using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer
{
	public class BoostHUDItem : MonoBehaviour
	{
		private enum VisualState
		{
			DEFAULT,
			ACTIVE,
			ENDING,
			USED
		}

		public Image Image;

		public Animator Animator;

		private IBoost boost;

		private BoostManager.AvailableBoosts boostType;

		private VisualState currentState;

		private bool returnFromPauseOnLoad;

		public void SetSprite(Sprite sprite)
		{
			Image.sprite = sprite;
		}

		public void SetBoostType(BoostManager.AvailableBoosts type)
		{
			boostType = type;
		}

		private void Update()
		{
			if (boost == null)
			{
				boost = Service.Get<BoostManager>().GetBoostObject(boostType);
				if (boost == null)
				{
					return;
				}
			}
			if (Animator == null)
			{
				return;
			}
			if (returnFromPauseOnLoad)
			{
				Animator.SetTrigger("Return");
				returnFromPauseOnLoad = false;
				if (boost.Ending)
				{
					Service.Get<IAudio>().SFX.Play(SFXEvent.UI_HUDWarn);
					SetVisualStateEnding();
				}
				else if (boost.Active)
				{
					SetVisualStateActive();
				}
				else if (boost.Used)
				{
					SetVisualStateUsed();
				}
			}
			else if (currentState != VisualState.USED)
			{
				if (boost.Active && currentState == VisualState.DEFAULT)
				{
					SetVisualStateActive();
				}
				else if (boost.Ending && currentState == VisualState.ACTIVE)
				{
					Service.Get<IAudio>().SFX.Play(SFXEvent.UI_HUDWarn);
					SetVisualStateEnding();
				}
				else if (!boost.Active && (currentState == VisualState.ACTIVE || currentState == VisualState.ENDING))
				{
					SetVisualStateDefault();
				}
				else if (boost.Used)
				{
					Service.Get<IAudio>().SFX.Stop(SFXEvent.UI_HUDWarn);
					SetVisualStateUsed();
				}
			}
		}

		private void OnDestroy()
		{
			Service.Get<IAudio>().SFX.Stop(SFXEvent.UI_HUDWarn);
		}

		private void SetVisualStateActive()
		{
			currentState = VisualState.ACTIVE;
			Animator.SetTrigger("Active");
		}

		private void SetVisualStateEnding()
		{
			currentState = VisualState.ENDING;
			Animator.SetTrigger("Ending");
		}

		private void SetVisualStateDefault()
		{
			currentState = VisualState.DEFAULT;
			Animator.SetTrigger("Idle");
		}

		private void SetVisualStateUsed()
		{
			currentState = VisualState.USED;
			Animator.SetTrigger("Used");
		}

		public void ReturnFromPause()
		{
			returnFromPauseOnLoad = true;
		}
	}
}
