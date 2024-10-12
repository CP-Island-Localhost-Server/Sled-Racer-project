using Disney.ClubPenguin.CPModuleUtils;
using InAppPurchases;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
    public class StoreMenuController : BaseMenuController
    {
        private IAPContext iapContext;

        protected override void VStart()
        {
            ShowStore();
        }

        protected override void Init()
        {
        }

        private void ShowStore()
        {
            Service.Get<IAudio>().Music.Play(MusicTrack.Storefront);
            GameObject original = Resources.Load<GameObject>("Prefabs/IAPContext");
            iapContext = (UnityEngine.Object.Instantiate(original) as GameObject).GetComponent<IAPContext>();
            iapContext.AppID = "SledRacer";
            iapContext.AppVersion = "1.3";
            iapContext.GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>(), worldPositionStays: false);
            iapContext.googlePlayToken = Service.Get<UIManager>().getGooglePlayToken();

            // Handle member benefits directly if needed without using IapViewType
            Debug.Log("Member benefits background is set. Player should have access to all items.");

            GameObject original2 = Resources.Load<GameObject>("Prefabs/DefaultMemberBenefitsBG");
            MemberBenefitsClickedHandler component = (UnityEngine.Object.Instantiate(original2) as GameObject).GetComponent<MemberBenefitsClickedHandler>();
            iapContext.SetMemberBenefitsBackground(component);
            IAPContext iAPContext = iapContext;
            iAPContext.IAPContextClosed = (IAPContext.IAPContextClosedDelegate)Delegate.Combine(iAPContext.IAPContextClosed, new IAPContext.IAPContextClosedDelegate(OnStoreClosed));
            PlayerDataService playerDataService = Service.Get<PlayerDataService>();
            iapContext.PlayerID = ((!playerDataService.IsPlayerLoggedIn()) ? 0 : playerDataService.PlayerData.Account.PlayerId);
        }

        private void OnStoreClosed(HashSet<string> ownedItemsSKUs)
        {
            UnityEngine.Debug.Log("OnStoreClosed");
            Service.Get<IAudio>().Music.Play(MusicTrack.MainMenu);
            Service.Get<BoostPurchaseManager>().OnPurchase(ownedItemsSKUs);
            IAPContext iAPContext = iapContext;
            iAPContext.IAPContextClosed = (IAPContext.IAPContextClosedDelegate)Delegate.Remove(iAPContext.IAPContextClosed, new IAPContext.IAPContextClosedDelegate(OnStoreClosed));
            GameObjectUtil.CleanupImageReferences(iapContext.gameObject);
            iapContext.transform.SetParent(null);
            UnityEngine.Object.Destroy(iapContext.gameObject);
            iapContext = null;
            Service.Get<UIManager>().ShowPreviousUIScene();
        }
    }
}
