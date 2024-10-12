using Disney.ClubPenguin.CPModuleUtils;
using System;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class AboutMembershipMenuController : BaseMenuController
	{
		private MembershipBenefits membershipBenefitsContext;

		protected override void VStart()
		{
			ShowAboutMembership();
		}

		protected override void Init()
		{
		}

		private void ShowAboutMembership()
		{
			GameObject original = Resources.Load<GameObject>("Prefabs/MemberBenefitsPanel");
			membershipBenefitsContext = (UnityEngine.Object.Instantiate(original) as GameObject).GetComponent<MembershipBenefits>();
			membershipBenefitsContext.GetComponent<RectTransform>().SetParent(base.transform, worldPositionStays: false);
			MembershipBenefits membershipBenefits = membershipBenefitsContext;
			membershipBenefits.BackButtonPressed = (MembershipBenefits.MembershipBenefitsBackDelegate)Delegate.Combine(membershipBenefits.BackButtonPressed, new MembershipBenefits.MembershipBenefitsBackDelegate(OnAboutMembershipClosed));
			PlayerDataService playerDataService = Service.Get<PlayerDataService>();
			membershipBenefitsContext.PlayerID = ((!playerDataService.IsPlayerLoggedIn()) ? 0 : playerDataService.PlayerData.Account.PlayerId);
			membershipBenefitsContext.ShowWebPage();
		}

		private void OnAboutMembershipClosed()
		{
			if (membershipBenefitsContext != null)
			{
				MembershipBenefits membershipBenefits = membershipBenefitsContext;
				membershipBenefits.BackButtonPressed = (MembershipBenefits.MembershipBenefitsBackDelegate)Delegate.Remove(membershipBenefits.BackButtonPressed, new MembershipBenefits.MembershipBenefitsBackDelegate(OnAboutMembershipClosed));
				GameObjectUtil.CleanupImageReferences(membershipBenefitsContext.gameObject);
				UnityEngine.Object.Destroy(membershipBenefitsContext.gameObject);
				membershipBenefitsContext = null;
			}
			Service.Get<UIManager>().ShowPreviousUIScene();
		}
	}
}
