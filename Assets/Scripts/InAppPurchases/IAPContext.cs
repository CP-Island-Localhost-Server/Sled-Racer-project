using DevonLocalization.Core;
using Disney.ClubPenguin.CPModuleUtils;
using Disney.ClubPenguin.Login.Authentication;
using Disney.ClubPenguin.Service.MWS;
using Disney.ClubPenguin.Service.MWS.Domain;
using InAppPurchases.BI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InAppPurchases
{
	[RequireComponent(typeof(CommerceProcessorInit), typeof(LoadingOverlay), typeof(AudioSource))]
	public class IAPContext : MonoBehaviour
	{
		public delegate void IAPContextClosedDelegate(HashSet<string> ownedItemSKUs);

		private static string IAP_LOC_PATH = "Assets/Framework/InAppPurchases/Resources/Translations";

		private static string IAP_MODULE = "INAPPPURCHASES";

		private static string PV_STORE_FRONT = "store_front";

		private static float TIMEOUT_WAIT_TIME = 30f;

		public IAPContextClosedDelegate IAPContextClosed;

		public StoreItemPopupWindow storeItemPopupWindowPrefab;

		public RectTransform IapContentPanel;

		public StoreFrontController storeFrontController;

		public RectTransform UIContainer;

		public string AppID = "StrikeForce";

		public string AppVersion = "1.0";

		public string googlePlayToken = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAicbY8N3b9eX8rcsEI+fRPEjpOLGMnkwFb4hnsyclPe+TTRmPv3ccHLh3/S/NrBgyXamTXgfxBS7h4jtGVwpf2DpAcrCczvn7siyjlpFgxscgJI34Ym9WD1NB3OmVZg0ysfo8RrLkZP1EgPY2Bpj+aNP3zfZrWpyy+g3ZWNLM9VtnnCgoq6Pi2fD6TNq96WpYC/duawKpYEjHalNXrl2sMC2NQWJl7bigjzOf92w6+TL5OSTmtww868CQbnoK4WsOISwygdV+eZsOorX1Q2a+I90pbagthuaqs//y6UTNZaZkDnAdKSlekWf1pd0gfnOxLGBKGH60kngzHuskS0JhGwIDAQAB";

		public MessageDialogOverlay MessageDialogOverlayPrefab;

		public MembershipBenefits MembershipBenefitsPrefab;

		public RectTransform MemberBenefitsBgContainer;

		public Button BackButton;

		public AudioClip BackButtonSound;

		public long PlayerID;

		private IMWSClient lazyMWSClientInstance;

		private bool storeTypeSpecified;

		private StoreType storeType;

		private CommerceProcessor commerceProcessor;

		private LoginContext loginContextInstance;

		private StoreItemPopupWindow storeItemPopupWindowInstance;

		private IAPModel iapModel;

		private LoadingOverlay loadingOverlay;

		private MessageDialogOverlay messageDialogOverlay;

		private CommerceErrorCodeTokens commErrorCodeTokens;

		private List<SkuInfo> skuList;

		private SavedStorePurchasesCollection savedStorePurchasesCollection;

		public IMWSClient MwsClient
		{
			get
			{
				object result;
				if (lazyMWSClientInstance != null)
				{
					IMWSClient iMWSClient = lazyMWSClientInstance;
					result = iMWSClient;
				}
				else
				{
					result = MWSClient.Instance;
				}
				return (IMWSClient)result;
			}
			set
			{
				lazyMWSClientInstance = value;
			}
		}

		public StoreType AppStoreType
		{
			get
			{
				if (storeTypeSpecified)
				{
					return storeType;
				}
				return StoreType.GOOGLE_PLAY;
			}
			set
			{
				storeType = value;
				storeTypeSpecified = true;
			}
		}

		private void Awake()
		{
			loadingOverlay = GetComponent<LoadingOverlay>();
			messageDialogOverlay = (UnityEngine.Object.Instantiate(MessageDialogOverlayPrefab) as MessageDialogOverlay);
			messageDialogOverlay.ParentContainer = GetComponent<RectTransform>();
			commErrorCodeTokens = new CommerceErrorCodeTokens();
			SetupLocalization();
		}

		private void Start()
		{
			HardwareBackButtonDispatcher.SetTargetClickHandler(BackButton, visible: false);
			savedStorePurchasesCollection = new SavedStorePurchasesCollection();
			savedStorePurchasesCollection.LoadFromDisk();
			iapModel = new IAPModel();
			iapModel.PlayerID = PlayerID;
			if (!string.IsNullOrEmpty(MwsClient.AuthToken) && iapModel.PlayerID == 0L)
			{
				throw new Exception("IAPContext requires you to pass in Player ID if MWS Client is already authenticated (logged in)");
			}
			SetupCommerce();
		}

		private void SetupLocalization()
		{
			ILocalizedTokenFilePath path = new ModuleTokensFilePath(IAP_LOC_PATH, IAP_MODULE, Platform.global);
			Localizer.Instance.LoadTokensFromLocalJSON(path);
		}

		public void SetMemberBenefitsBackground(MemberBenefitsClickedHandler memberBenefitsBg)
		{
			for (int i = 0; i < MemberBenefitsBgContainer.childCount; i++)
			{
				MemberBenefitsClickedHandler component = MemberBenefitsBgContainer.GetChild(i).GetComponent<MemberBenefitsClickedHandler>();
				component.ShowBenefitsClicked = (MemberBenefitsClickedHandler.ShowBenefitsClickedDelegate)Delegate.Remove(component.ShowBenefitsClicked, new MemberBenefitsClickedHandler.ShowBenefitsClickedDelegate(OnMemberBenefitsButtonClicked));
				UnityEngine.Object.Destroy(MemberBenefitsBgContainer.GetChild(i).gameObject);
			}
			memberBenefitsBg.GetComponent<RectTransform>().SetParent(MemberBenefitsBgContainer, worldPositionStays: false);
			memberBenefitsBg.ShowBenefitsClicked = (MemberBenefitsClickedHandler.ShowBenefitsClickedDelegate)Delegate.Combine(memberBenefitsBg.ShowBenefitsClicked, new MemberBenefitsClickedHandler.ShowBenefitsClickedDelegate(OnMemberBenefitsButtonClicked));
		}

		private void OnMemberBenefitsButtonClicked()
		{
			new ShowMemberBenefitsCMD(MembershipBenefitsPrefab, UIContainer, IapContentPanel, BackButton, iapModel).Execute();
		}

		private void OnDestroy()
		{
			commerceProcessor.PurchaseResponse = null;
			UnityEngine.Object.Destroy(commerceProcessor.gameObject);
			IAPContextClosed = null;
			storeItemPopupWindowPrefab = null;
			IapContentPanel = null;
			storeFrontController = null;
			UIContainer = null;
			MessageDialogOverlayPrefab = null;
			MembershipBenefitsPrefab = null;
			MemberBenefitsBgContainer = null;
			BackButton = null;
			BackButtonSound = null;
			commerceProcessor = null;
			loginContextInstance = null;
			storeItemPopupWindowInstance = null;
			iapModel = null;
			loadingOverlay = null;
			messageDialogOverlay = null;
			commErrorCodeTokens = null;
		}

		private void SetupCommerce()
		{
			SetupCommerceCMD setupCommerceCMD = new SetupCommerceCMD(GetComponent<CommerceProcessorInit>(), iapModel, googlePlayToken, commErrorCodeTokens, AppStoreType, storeItemPopupWindowPrefab, MwsClient, UIContainer, storeFrontController, savedStorePurchasesCollection, AppID, loadingOverlay, messageDialogOverlay, BackButton);
			SetupCommerceCMD setupCommerceCMD2 = setupCommerceCMD;
			setupCommerceCMD2.CommerceSetupCompleted = (SetupCommerceCMD.CommerceSetupCompletedDelegate)Delegate.Combine(setupCommerceCMD2.CommerceSetupCompleted, new SetupCommerceCMD.CommerceSetupCompletedDelegate(OnCommerceSetupCompleted));
			setupCommerceCMD.Execute();
		}

		private void OnCommerceSetupCompleted(CommerceProcessor commerceProcessor)
		{
			this.commerceProcessor = commerceProcessor;
			SetupStorefrontCMD setupStorefrontCMD = new SetupStorefrontCMD(MwsClient, storeFrontController, AppStoreType, commerceProcessor, iapModel, storeItemPopupWindowPrefab, UIContainer, loadingOverlay, messageDialogOverlay, savedStorePurchasesCollection, BackButton, TIMEOUT_WAIT_TIME);
			SetupStorefrontCMD setupStorefrontCMD2 = setupStorefrontCMD;
			setupStorefrontCMD2.StoreFrontSetupCompleted = (SetupStorefrontCMD.StoreFrontSetupCompletedDelegate)Delegate.Combine(setupStorefrontCMD2.StoreFrontSetupCompleted, new SetupStorefrontCMD.StoreFrontSetupCompletedDelegate(OnStoreFrontSetupCompleted));
			setupStorefrontCMD.Execute();
		}

		private void OnStoreFrontSetupCompleted()
		{
			new SendPageViewBICMD(PV_STORE_FRONT, iapModel.PlayerID).Execute();
			new SetupPurchasableProductsCMD(AppID, AppVersion, AppStoreType, commErrorCodeTokens, commerceProcessor, storeFrontController, savedStorePurchasesCollection, iapModel, MwsClient, loadingOverlay, messageDialogOverlay, OnPurchasableProdSetupCompleted, TIMEOUT_WAIT_TIME).Execute();
		}

		private void OnPurchasableProdSetupCompleted(bool complete)
		{
			new ShowReRegistrationAttempCMD(MwsClient, AppStoreType, AppID, messageDialogOverlay, loadingOverlay).Execute();
		}

		public void OnBackButton()
		{
			GetComponent<AudioSource>().PlayOneShot(BackButtonSound);
			StartCoroutine(WaitForSound());
		}

		private IEnumerator WaitForSound()
		{
			yield return null;
			Close();
		}

		public void Close()
		{
			GameObjectUtil.CleanupImageReferences(base.gameObject);
			if (IAPContextClosed != null)
			{
				if (iapModel != null)
				{
					IAPContextClosed(new HashSet<string>(iapModel.AllItemList));
				}
				else
				{
					IAPContextClosed(null);
				}
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
