using Disney.ClubPenguin.CPModuleUtils;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace InAppPurchases
{
	public class ShowMemberBenefitsCMD
	{
		private MembershipBenefits memberBenefitsPrefab;

		private RectTransform parentContainer;

		private RectTransform iapContentPanel;

		private Button iapBackButton;

		private IAPModel iapModel;

		private MembershipBenefits membershipBenefitsInstance;

		public ShowMemberBenefitsCMD(MembershipBenefits memberBenefitsPrefab, RectTransform parentContainer, RectTransform iapContentPanel, Button iapBackButton, IAPModel iapModel)
		{
			this.memberBenefitsPrefab = memberBenefitsPrefab;
			this.parentContainer = parentContainer;
			this.iapContentPanel = iapContentPanel;
			this.iapBackButton = iapBackButton;
			this.iapModel = iapModel;
		}

		public void Execute()
		{
			membershipBenefitsInstance = (UnityEngine.Object.Instantiate(memberBenefitsPrefab) as MembershipBenefits);
			membershipBenefitsInstance.PlayerID = iapModel.PlayerID;
			MembershipBenefits membershipBenefits = membershipBenefitsInstance;
			membershipBenefits.BackButtonPressed = (MembershipBenefits.MembershipBenefitsBackDelegate)Delegate.Combine(membershipBenefits.BackButtonPressed, new MembershipBenefits.MembershipBenefitsBackDelegate(OnBackButtonPressed));
			membershipBenefitsInstance.GetComponent<RectTransform>().SetParent(parentContainer, worldPositionStays: false);
			membershipBenefitsInstance.ShowWebPage();
			iapContentPanel.gameObject.SetActive(value: false);
		}

		private void OnBackButtonPressed()
		{
			MembershipBenefits membershipBenefits = membershipBenefitsInstance;
			membershipBenefits.BackButtonPressed = (MembershipBenefits.MembershipBenefitsBackDelegate)Delegate.Remove(membershipBenefits.BackButtonPressed, new MembershipBenefits.MembershipBenefitsBackDelegate(OnBackButtonPressed));
			iapContentPanel.gameObject.SetActive(value: true);
			UnityEngine.Object.Destroy(membershipBenefitsInstance.gameObject);
			HardwareBackButtonDispatcher.SetTargetClickHandler(iapBackButton, visible: false);
		}
	}
}
