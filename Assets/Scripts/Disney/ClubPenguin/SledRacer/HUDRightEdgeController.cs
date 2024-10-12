using System.Collections.Generic;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class HUDRightEdgeController : BaseMenuController
	{
		public GameObject BoostPrefab;

		public Animator LoadAnimator;

		public List<GameObject> BoostSlots;

		private IList<BoostManager.AvailableBoosts> boosts;

		protected override void VStart()
		{
		}

		protected override void Init()
		{
			UIManager uIManager = Service.Get<UIManager>();
			bool flag = uIManager.GetPreviousUIScene() == uIManager.PauseMenuPanel;
			boosts = Service.Get<BoostManager>().EquipedBoosts;
			LoadAnimator.SetInteger("BoostCount", boosts.Count);
			for (int i = 0; i < boosts.Count && i < BoostSlots.Count; i++)
			{
				GameObject gameObject = Object.Instantiate(BoostPrefab) as GameObject;
				BoostHUDItem componentInChildren = gameObject.GetComponentInChildren<BoostHUDItem>();
				componentInChildren.SetSprite(boosts[i].GetHudSprite());
				componentInChildren.SetBoostType(boosts[i]);
				if (flag)
				{
					componentInChildren.ReturnFromPause();
				}
				gameObject.transform.SetParent(BoostSlots[i].transform, worldPositionStays: false);
				gameObject.transform.localScale = Vector3.one;
				gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			}
		}

		public override void InitAnimations()
		{
			UIManager uIManager = Service.Get<UIManager>();
			if (uIManager.GetPreviousUIScene() == uIManager.PauseMenuPanel)
			{
				SetLoadingAnimationTrigger("Return");
			}
			else
			{
				base.InitAnimations();
			}
		}
	}
}
