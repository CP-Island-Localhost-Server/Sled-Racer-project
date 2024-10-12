// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Disney.ClubPenguin.Login.Authentication.LoginContext
using System;
using System.Collections;
using Disney.ClubPenguin.CPModuleUtils;
using Disney.ClubPenguin.Login;
using Disney.ClubPenguin.Login.Authentication;
using Disney.ClubPenguin.Login.BI;
using Disney.ClubPenguin.Login.Creation;
using Disney.ClubPenguin.Login.UI;
using Disney.ClubPenguin.ParentPermission;
using Disney.ClubPenguin.Service.MWS;
using Disney.ClubPenguin.Service.PDR;
using Disney.ClubPenguin.WebPageViewer;
using Disney.HTTP.Client;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class LoginContext : MonoBehaviour
{
	public delegate void LoginSucceededDelegate(IGetAuthTokenResponse response, string username, string password);

	public delegate void LoginClosedDelegate();

	public delegate void AboutMembershipClickedDelegate();

	public LoginSucceededDelegate LoginSucceeded;

	public LoginClosedDelegate LoginClosed;

	public AboutMembershipClickedDelegate AboutMembershipClicked;

	private IMWSClient mwsClient;

	private IPDRClient pdrClient;

	private ILoginBIUtils loginBIUtils;

	public string AppID = "CPMCAPP";

	public string AppVersion = "1.6";

	public GameObject NewLoginPanelPrefab;

	public GameObject CreatePanelPrefab;

	public GameObject ReturningLoginPanelPrefab;

	public GameObject SavedPlayersPanelPrefab;

	public GameObject CreatePenguinPanelPrefab;

	public RectTransform LayoutPanel;

	public Button BackButton;

	public Text HeaderTextWelcome;

	public Text HeaderTextCreate;

	public Text HeaderTextSelectSaved;

	public Text HeaderTextActivate;

	public Text HeaderTextParentPermissions;

	public AudioClip BackButtonAudioClip;

	public Button CloseButton;

	public AudioClip CloseButtonAudioClip;

	public RectTransform Background;

	public Text LegalText;

	public RectTransform ParentPermissionPanel;

	public ParentPermissionController ParentPermissionPrefab;

	public RectTransform ActivatePenguinPanel;

	public ActivatePenguinController ActivatePenguinPrefab;

	public RectTransform WebViewPanel;

	public DisplayNativeWebPage NativeWebPagePrefab;

	public AudioClip KeyboardOpenAudioClip;

	public AudioClip KeyboardCloseAudioClip;

	public AudioClip OKButtonAudioClip;

	[HideInInspector]
	public RectTransform[] MemberBenefitViewPrefabs;

	[HideInInspector]
	public bool ShowMembershipButtonOnActivate;

	[HideInInspector]
	public float LoginRequestTimeoutSec = 2f;

	private GameObject newLoginPanel;

	private GameObject createPanel;

	private GameObject createPenguinPanel;

	private GameObject returningLoginPanel;

	private GameObject savedPlayersPanel;

	private ParentPermissionController parentPermissionController;

	private ActivatePenguinController activatePenguinController;

	private DisplayNativeWebPage nativeWebPageinstance;

	private string parentPermissionURL;

	private Button permissionBackButton;

	private bool previousBackButtonState;

	private LoginController currentLoginController;

	private LoginController.LoginSucceededDelegate currentSuccessHandler;

	private Action<IHTTPResponse> currentFailureHandler;

	private SavedPlayerCollection savedPlayerCollection;

	private SavedPlayerData returningPlayerData;

	private bool IsFirstTimeLogin;

	private IGetAuthTokenResponse accountCreatedResponse;

	private string username;

	private string password;

	private Text currentHeaderText;

	private long playerID;

	public IMWSClient MwsClient
	{
		get
		{
			IMWSClient result;
			if (mwsClient == null)
			{
				IMWSClient instance = MWSClient.Instance;
				result = instance;
			}
			else
			{
				result = mwsClient;
			}
			return result;
		}
		set
		{
			mwsClient = value;
		}
	}

	public IPDRClient PdrClient
	{
		get
		{
			IPDRClient result;
			if (pdrClient == null)
			{
				IPDRClient instance = PDRClient.Instance;
				result = instance;
			}
			else
			{
				result = pdrClient;
			}
			return result;
		}
		set
		{
			pdrClient = value;
		}
	}

	public ILoginBIUtils LoginBiUtils
	{
		get
		{
			ILoginBIUtils result;
			if (this.loginBIUtils == null)
			{
				ILoginBIUtils loginBIUtils = new LoginBIUtils();
				result = loginBIUtils;
			}
			else
			{
				result = this.loginBIUtils;
			}
			return result;
		}
		set
		{
			loginBIUtils = value;
		}
	}

	public event Action<IHTTPResponse> LoginFailed;

	public event Action<byte[]> PaperDollReceived;

	private void Awake()
	{
		CloseButton.onClick.AddListener(delegate
			{
				StartCoroutine(CloseLoginAfterAudio());
			});
		HideAllHeaderTexts();
		HeaderTextWelcome.gameObject.SetActive(true);
		currentHeaderText = HeaderTextWelcome;
	}

	public void SetLoginBIContext(string value)
	{
		LoginBiUtils.ContextName = value;
	}

	public void SetPlayerID(long value)
	{
		playerID = value;
	}

	private void OnDestroy()
	{
		BackButton.onClick.RemoveAllListeners();
		CloseButton.onClick.RemoveAllListeners();
		LoginSucceeded = null;
		this.LoginFailed = null;
		LoginClosed = null;
	}

	private void SetupLoginControllerAndHandlers(LoginController loginController, LoginController.LoginSucceededDelegate successHandler, Action<IHTTPResponse> failureHandler)
	{
		currentLoginController = loginController;
		currentSuccessHandler = successHandler;
		currentFailureHandler = failureHandler;
		currentLoginController.AppID = AppID;
		currentLoginController.AppVersion = AppVersion;
		currentLoginController.Client = MwsClient;
		currentLoginController.PDRClient = PdrClient;
		LoginController loginController2 = currentLoginController;
		loginController2.LoginSucceeded = (LoginController.LoginSucceededDelegate)Delegate.Combine(loginController2.LoginSucceeded, new LoginController.LoginSucceededDelegate(HideLoadingIcon));
		currentLoginController.LoginFailed += new Action<IHTTPResponse>(response => this.HideLoadingIcon(response, null, null));
		LoginController loginController3 = currentLoginController;
		loginController3.LoginSucceeded = (LoginController.LoginSucceededDelegate)Delegate.Combine(loginController3.LoginSucceeded, currentSuccessHandler);
		currentLoginController.LoginFailed += currentFailureHandler;
		LoginController loginController4 = currentLoginController;
		loginController4.LoginAttemptStarted = (LoginController.LoginAttempStartedDelegate)Delegate.Combine(loginController4.LoginAttemptStarted, new LoginController.LoginAttempStartedDelegate(ShowLoadingIcon));
		LoginController loginController5 = currentLoginController;
		loginController5.PaperDollRecevied = (LoginController.PaperDollReceivedDelegate)Delegate.Combine(loginController5.PaperDollRecevied, new LoginController.PaperDollReceivedDelegate(OnPaperDollReceived));
		currentLoginController.LoginRequestTimeout = LoginRequestTimeoutSec;
		LoginController loginController6 = currentLoginController;
		loginController6.ForgotPasswordClicked = (LoginController.ForgotPasswordClickedDelegate)Delegate.Combine(loginController6.ForgotPasswordClicked, new LoginController.ForgotPasswordClickedDelegate(ShowParentPermission));
		currentLoginController.RootAudioSource = base.GetComponent<AudioSource>();
	}

	private void CleanUpLoginControllerAndHandlers()
	{
		if (currentLoginController != null)
		{
			LoginController loginController = currentLoginController;
			loginController.LoginSucceeded = (LoginController.LoginSucceededDelegate)Delegate.Remove(loginController.LoginSucceeded, currentSuccessHandler);
			LoginController loginController2 = currentLoginController;
			loginController2.LoginSucceeded = (LoginController.LoginSucceededDelegate)Delegate.Remove(loginController2.LoginSucceeded, new LoginController.LoginSucceededDelegate(HideLoadingIcon));
			currentLoginController.LoginFailed -= currentFailureHandler;
			currentLoginController.LoginFailed -= new Action<IHTTPResponse>(response => this.HideLoadingIcon(response, null, null));
			LoginController loginController3 = currentLoginController;
			loginController3.LoginAttemptStarted = (LoginController.LoginAttempStartedDelegate)Delegate.Remove(loginController3.LoginAttemptStarted, new LoginController.LoginAttempStartedDelegate(ShowLoadingIcon));
			LoginController loginController4 = currentLoginController;
			loginController4.PaperDollRecevied = (LoginController.PaperDollReceivedDelegate)Delegate.Remove(loginController4.PaperDollRecevied, new LoginController.PaperDollReceivedDelegate(OnPaperDollReceived));
			LoginController loginController5 = currentLoginController;
			loginController5.ForgotPasswordClicked = (LoginController.ForgotPasswordClickedDelegate)Delegate.Remove(loginController5.ForgotPasswordClicked, new LoginController.ForgotPasswordClickedDelegate(ShowParentPermission));
			currentLoginController.Client = null;
			currentLoginController.PDRClient = null;
			currentLoginController = null;
		}
		currentSuccessHandler = null;
		currentFailureHandler = null;
	}

	private void OnPaperDollReceived(byte[] paperDollBytes)
	{
		if (this.PaperDollReceived != null)
		{
			this.PaperDollReceived(paperDollBytes);
		}
	}

	public void DetermineAndShowLoginState()
	{
		Background.gameObject.SetActive(true);
		savedPlayerCollection = new SavedPlayerCollection();
		if (savedPlayerCollection.ExistsOnDisk())
		{
			savedPlayerCollection.LoadFromDisk();
			if (savedPlayerCollection.SavedPlayers.Count > 0)
			{
				IsFirstTimeLogin = false;
				ShowSavedPlayers();
				return;
			}
		}
		IsFirstTimeLogin = true;
		ShowNewPlayerLogin();
	}

	public void ShowNewPlayerLogin()
	{
		HideAllHeaderTexts();
		HeaderTextWelcome.gameObject.SetActive(true);
		currentHeaderText = HeaderTextWelcome;
		newLoginPanel = (GameObject)UnityEngine.Object.Instantiate(NewLoginPanelPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
		RectTransform component = newLoginPanel.GetComponent<RectTransform>();
		component.SetParent(LayoutPanel, false);
		createPanel = (GameObject)UnityEngine.Object.Instantiate(CreatePanelPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
		createPanel.GetComponent<CreateClickedController>().RootAudioSource = base.GetComponent<AudioSource>();
		component = createPanel.GetComponent<RectTransform>();
		component.SetParent(LayoutPanel, false);
		CreateClickedController component2 = createPanel.GetComponent<CreateClickedController>();
		component2.CreateButtonClicked = (CreateClickedController.CreateButtonClickedDelegate)Delegate.Combine(component2.CreateButtonClicked, new CreateClickedController.CreateButtonClickedDelegate(ShowCreatePenguinPanel));
		SetupLoginControllerAndHandlers(newLoginPanel.GetComponent<LoginController>(), OnNewLoginSucceeded, OnNewLoginFailed);
		LoginBiUtils.SendPageviewOnLogin(BiPageNames.NEWLOGINSTART, playerID);
		BackButton.onClick.RemoveAllListeners();
		if (IsFirstTimeLogin)
		{
			BackButton.gameObject.SetActive(false);
			HardwareBackButtonDispatcher.SetTargetClickHandler(CloseButton);
		}
		else
		{
			BackButton.gameObject.SetActive(true);
			HardwareBackButtonDispatcher.SetTargetClickHandler(BackButton, false);
			BackButton.onClick.AddListener(delegate
				{
					UnityEngine.Object.Destroy(newLoginPanel);
					UnityEngine.Object.Destroy(createPanel);
					base.GetComponent<AudioSource>().PlayOneShot(BackButtonAudioClip);
					ShowSavedPlayers();
				});
		}
		LegalText.gameObject.SetActive(true);
		Background.gameObject.SetActive(true);
	}

	private void OnNewLoginSucceeded(IGetAuthTokenResponse response, string username, string password)
	{
		this.username = username;
		this.password = password;
		playerID = response.AuthData.PlayerId;
		LoginBiUtils.SendPlayerInfo(response.AuthData.PlayerId, response.AuthData.Username);
		LoginBiUtils.SendPageviewOnLogin(BiPageNames.LOGINSUCCESS, playerID);
		CleanUpLoginControllerAndHandlers();
		UnityEngine.Object.Destroy(newLoginPanel);
		UnityEngine.Object.Destroy(createPanel);
		if (LoginSucceeded != null)
		{
			LoginSucceeded(response, username, password);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnNewLoginFailed(IHTTPResponse response)
	{
		LoginBiUtils.SendPageviewOnLogin(BiPageNames.FAILEDLOGIN, playerID);
		if (this.LoginFailed != null)
		{
			this.LoginFailed(response);
		}
	}

	public void ShowSavedPlayers()
	{
		HideAllHeaderTexts();
		HeaderTextSelectSaved.gameObject.SetActive(true);
		currentHeaderText = HeaderTextSelectSaved;
		savedPlayersPanel = (GameObject)UnityEngine.Object.Instantiate(SavedPlayersPanelPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
		RectTransform component = savedPlayersPanel.GetComponent<RectTransform>();
		component.SetParent(LayoutPanel, false);
		SavedPlayersController component2 = savedPlayersPanel.GetComponent<SavedPlayersController>();
		SavedPlayerCollection savedPlayerCollection = new SavedPlayerCollection();
		savedPlayerCollection.LoadFromDisk();
		component2.SetupSavedPlayers(savedPlayerCollection, base.GetComponent<AudioSource>());
		component2.PlayerSelected = (SavedPlayersController.PlayerSelectedDelegate)Delegate.Combine(component2.PlayerSelected, new SavedPlayersController.PlayerSelectedDelegate(OnSavedPlayerSelected));
		component2.AddPlayerClicked = (SavedPlayersController.AddPlayerClickedDelegate)Delegate.Combine(component2.AddPlayerClicked, new SavedPlayersController.AddPlayerClickedDelegate(OnAddPlayerClicked));
		HardwareBackButtonDispatcher.SetTargetClickHandler(CloseButton);
		LoginBiUtils.SendPageviewOnLogin(BiPageNames.CHOOSEPENGUIN, playerID);
		BackButton.onClick.RemoveAllListeners();
		BackButton.gameObject.SetActive(false);
		LegalText.gameObject.SetActive(false);
		Background.gameObject.SetActive(true);
	}

	private IEnumerator CloseLoginAfterAudio()
	{
		base.GetComponent<AudioSource>().PlayOneShot(CloseButtonAudioClip);
		yield return new WaitForSeconds(CloseButtonAudioClip.length);
		CleanUpLoginControllerAndHandlers();
		if (accountCreatedResponse != null && LoginSucceeded != null)
		{
			LoginSucceeded(accountCreatedResponse, username, password);
		}
		if (LoginClosed != null)
		{
			LoginClosed();
		}
		if (nativeWebPageinstance != null)
		{
			nativeWebPageinstance.OnCloseButton();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnSavedPlayerSelected(SavedPlayerData savedPlayerData)
	{
		returningPlayerData = savedPlayerData;
		if (returningPlayerData.Password != null && returningPlayerData.Password != string.Empty)
		{
			returningLoginPanel = UnityEngine.Object.Instantiate(ReturningLoginPanelPrefab) as GameObject;
			LoginController component = returningLoginPanel.GetComponent<LoginController>();
			SetupLoginControllerAndHandlers(component, OnReturningLoginSucceeded, ShowReturningPlayerLogin);
			component.DoLogin(returningPlayerData.UserName, returningPlayerData.Password, true);
		}
		else
		{
			ShowReturningPlayerLogin();
		}
	}

	private void ShowReturningPlayerLogin(IHTTPResponse failure = null)
	{
		if (failure != null)
		{
			CleanUpLoginControllerAndHandlers();
		}
		SavedPlayersController component = savedPlayersPanel.GetComponent<SavedPlayersController>();
		component.PlayerSelected = (SavedPlayersController.PlayerSelectedDelegate)Delegate.Remove(component.PlayerSelected, new SavedPlayersController.PlayerSelectedDelegate(OnSavedPlayerSelected));
		component.AddPlayerClicked = (SavedPlayersController.AddPlayerClickedDelegate)Delegate.Remove(component.AddPlayerClicked, new SavedPlayersController.AddPlayerClickedDelegate(OnAddPlayerClicked));
		UnityEngine.Object.Destroy(savedPlayersPanel);
		returningLoginPanel = (GameObject)UnityEngine.Object.Instantiate(ReturningLoginPanelPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
		RectTransform component2 = returningLoginPanel.GetComponent<RectTransform>();
		component2.SetParent(LayoutPanel, false);
		SetupLoginControllerAndHandlers(returningLoginPanel.GetComponent<LoginController>(), OnReturningLoginSucceeded, OnReturningLoginFailed);
		ReturningLoginController component3 = returningLoginPanel.GetComponent<ReturningLoginController>();
		component3.SetSavedPlayer(returningPlayerData);
		LoginBiUtils.SendPageviewOnLogin(BiPageNames.RETURNLOGINSTART, playerID);
		BackButton.onClick.RemoveAllListeners();
		BackButton.gameObject.SetActive(true);
		HardwareBackButtonDispatcher.SetTargetClickHandler(BackButton, false);
		BackButton.onClick.AddListener(delegate
			{
				UnityEngine.Object.Destroy(returningLoginPanel);
				base.GetComponent<AudioSource>().PlayOneShot(BackButtonAudioClip);
				ShowSavedPlayers();
			});
	}

	private void OnAddPlayerClicked()
	{
		SavedPlayersController component = savedPlayersPanel.GetComponent<SavedPlayersController>();
		component.PlayerSelected = (SavedPlayersController.PlayerSelectedDelegate)Delegate.Remove(component.PlayerSelected, new SavedPlayersController.PlayerSelectedDelegate(OnSavedPlayerSelected));
		component.AddPlayerClicked = (SavedPlayersController.AddPlayerClickedDelegate)Delegate.Remove(component.AddPlayerClicked, new SavedPlayersController.AddPlayerClickedDelegate(OnAddPlayerClicked));
		UnityEngine.Object.Destroy(savedPlayersPanel);
		ShowNewPlayerLogin();
	}

	private void OnReturningLoginSucceeded(IGetAuthTokenResponse response, string username, string password)
	{
		this.username = username;
		this.password = password;
		playerID = response.AuthData.PlayerId;
		LoginBiUtils.SendPlayerInfo(response.AuthData.PlayerId, response.AuthData.Username);
		LoginBiUtils.SendPageviewOnLogin(BiPageNames.LOGINSUCCESS, playerID);
		CleanUpLoginControllerAndHandlers();
		if (savedPlayersPanel != null)
		{
			UnityEngine.Object.Destroy(savedPlayersPanel);
		}
		if (returningLoginPanel != null)
		{
			UnityEngine.Object.Destroy(returningLoginPanel);
		}
		if (LoginSucceeded != null)
		{
			LoginSucceeded(response, username, password);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnReturningLoginFailed(IHTTPResponse response)
	{
		LoginBiUtils.SendPageviewOnLogin(BiPageNames.FAILEDLOGIN, playerID);
		if (this.LoginFailed != null)
		{
			this.LoginFailed(response);
		}
	}

	public void ShowCreatePenguinPanel()
	{
		HideAllHeaderTexts();
		HeaderTextCreate.gameObject.SetActive(true);
		currentHeaderText = HeaderTextCreate;
		BackButton.gameObject.SetActive(true);
		HardwareBackButtonDispatcher.SetTargetClickHandler(BackButton, false);
		CleanUpLoginControllerAndHandlers();
		UnityEngine.Object.Destroy(newLoginPanel);
		UnityEngine.Object.Destroy(createPanel);
		createPenguinPanel = (GameObject)UnityEngine.Object.Instantiate(CreatePenguinPanelPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
		RectTransform component = createPenguinPanel.GetComponent<RectTransform>();
		component.SetParent(LayoutPanel, false);
		CreatePenguinController createPenguinController = createPenguinPanel.GetComponent<CreatePenguinController>();
		createPenguinController.ProtectedWebLinkClicked += ShowParentPermission;
		LoginBiUtils.SendPageviewOnLogin(BiPageNames.CREATE, playerID);
		createPenguinController.RootAudioSource = base.GetComponent<AudioSource>();
		createPenguinPanel.GetComponent<PenguinColor>().RootAudioSource = base.GetComponent<AudioSource>();
		createPenguinController.Client = MwsClient;
		createPenguinController.PDRClient = PdrClient;
		CreatePenguinController createPenguinController2 = createPenguinController;
		createPenguinController2.CreationAttemptStarted = (CreatePenguinController.CreationAttempStartedDelegate)Delegate.Combine(createPenguinController2.CreationAttemptStarted, new CreatePenguinController.CreationAttempStartedDelegate(ShowLoadingIcon));
		CreatePenguinController createPenguinController3 = createPenguinController;
		createPenguinController3.CreationFailed = (CreatePenguinController.CreationFailedDelegate)Delegate.Combine(createPenguinController3.CreationFailed, new CreatePenguinController.CreationFailedDelegate(() => this.HideLoadingIcon(null, null, null)));
		CreatePenguinController createPenguinController4 = createPenguinController;
		createPenguinController4.CreationSucceeded = (CreatePenguinController.CreationSucceededDelegate)Delegate.Combine(createPenguinController4.CreationSucceeded, new CreatePenguinController.CreationSucceededDelegate(OnCreationSucceeded));
		CreatePenguinController createPenguinController5 = createPenguinController;
		createPenguinController5.HtmlLinkClicked = (CreatePenguinController.HtmlLinkClickedDelegate)Delegate.Combine(createPenguinController5.HtmlLinkClicked, new CreatePenguinController.HtmlLinkClickedDelegate(OnWebLinkClicked));
		BackButton.onClick.RemoveAllListeners();
		BackButton.onClick.AddListener(delegate
			{
				createPenguinController.ProtectedWebLinkClicked -= ShowParentPermission;
				UnityEngine.Object.Destroy(createPenguinPanel);
				base.GetComponent<AudioSource>().PlayOneShot(BackButtonAudioClip);
				ShowNewPlayerLogin();
			});
		LegalText.gameObject.SetActive(false);
	}

	private void OnCreationSucceeded(IGetAuthTokenResponse response, string parentEmail, string penguinName, string password)
	{
		createPenguinPanel.GetComponent<CreatePenguinController>().ProtectedWebLinkClicked -= ShowParentPermission;
		accountCreatedResponse = response;
		username = penguinName;
		this.password = password;
		playerID = response.AuthData.PlayerId;
		UnityEngine.Object.Destroy(createPenguinPanel);
		LoginBiUtils.SendAccoundCreated(response.AuthData.PlayerId, penguinName);
		HideAllHeaderTexts();
		HeaderTextActivate.gameObject.SetActive(true);
		currentHeaderText = HeaderTextActivate;
		activatePenguinController = UnityEngine.Object.Instantiate(ActivatePenguinPrefab) as ActivatePenguinController;
		activatePenguinController.GetComponent<RectTransform>().SetParent(ActivatePenguinPanel, false);
		activatePenguinController.SetParentEmail(parentEmail);
		activatePenguinController.SetPenguinName(penguinName);
		activatePenguinController.SetMembershipButtonVisibility(ShowMembershipButtonOnActivate);
		if (MemberBenefitViewPrefabs != null && MemberBenefitViewPrefabs.Length > 0)
		{
			activatePenguinController.SetMembershipBenefitViewsPrefabs(MemberBenefitViewPrefabs);
		}
		ActivatePenguinController obj = activatePenguinController;
		obj.AboutMembershipClicked = (ActivatePenguinController.AboutMembershipClickedDelegate)Delegate.Combine(obj.AboutMembershipClicked, new ActivatePenguinController.AboutMembershipClickedDelegate(OnAboutMembershipClicked));
		BackButton.gameObject.SetActive(false);
		HideLoadingIcon();
	}

	private void OnAboutMembershipClicked()
	{
		if (AboutMembershipClicked != null)
		{
			AboutMembershipClicked();
		}
	}

	private void ShowLoadingIcon()
	{
		GetComponent<LoadingIcon>().Show(true);
		HardwareBackButtonDispatcher.ListenForInput = false;
	}

	private void HideLoadingIcon(object response = null, string username = null, string password = null)
	{
		GetComponent<LoadingIcon>().Hide();
		HardwareBackButtonDispatcher.ListenForInput = true;
	}

	private void ShowParentPermission(string parentPermissionURL)
	{
		Application.OpenURL(parentPermissionURL);
	}

	private void OnParentPermissionSuccess(int age)
	{
		LoginBiUtils.SendAgeEnteredGameAction(age, playerID);
		Application.OpenURL(parentPermissionURL);
		HideParentPermission();
	}

	private void OnParentPermissionFail(int age)
	{
		LoginBiUtils.SendAgeEnteredGameAction(age, playerID);
		base.GetComponent<AudioSource>().PlayOneShot(OKButtonAudioClip);
		HideParentPermission();
	}

	private void HideParentPermission()
	{
		CloseButton.gameObject.SetActive(true);
		if (permissionBackButton != null)
		{
			permissionBackButton.onClick.RemoveAllListeners();
			UnityEngine.Object.Destroy(permissionBackButton.gameObject);
			permissionBackButton = null;
		}
		BackButton.gameObject.SetActive(previousBackButtonState);
		if (previousBackButtonState)
		{
			HardwareBackButtonDispatcher.SetTargetClickHandler(BackButton, false);
		}
		HideAllHeaderTexts();
		currentHeaderText.gameObject.SetActive(true);
		parentPermissionController.onFailClose -= OnParentPermissionFail;
		parentPermissionController.onSuccess -= OnParentPermissionSuccess;
		UnityEngine.Object.Destroy(parentPermissionController.gameObject);
		parentPermissionController = null;
		LayoutPanel.gameObject.SetActive(true);
	}

	private void OnWebLinkClicked(string url)
	{
		CloseButton.gameObject.SetActive(false);
		if (!(nativeWebPageinstance != null))
		{
			previousBackButtonState = BackButton.gameObject.activeSelf;
			BackButton.gameObject.SetActive(false);
			nativeWebPageinstance = UnityEngine.Object.Instantiate(NativeWebPagePrefab) as DisplayNativeWebPage;
			nativeWebPageinstance.GetComponent<RectTransform>().SetParent(WebViewPanel, false);
			DisplayNativeWebPage displayNativeWebPage = nativeWebPageinstance;
			displayNativeWebPage.Closed = (DisplayNativeWebPage.ClosedDelegate)Delegate.Combine(displayNativeWebPage.Closed, new DisplayNativeWebPage.ClosedDelegate(OnWebPageClosed));
			LayoutPanel.gameObject.SetActive(false);
			nativeWebPageinstance.DisplayPage(url);
		}
	}

	private void OnWebPageClosed()
	{
		CloseButton.gameObject.SetActive(true);
		DisplayNativeWebPage displayNativeWebPage = nativeWebPageinstance;
		displayNativeWebPage.Closed = (DisplayNativeWebPage.ClosedDelegate)Delegate.Remove(displayNativeWebPage.Closed, new DisplayNativeWebPage.ClosedDelegate(OnWebPageClosed));
		UnityEngine.Object.Destroy(nativeWebPageinstance.gameObject);
		LayoutPanel.gameObject.SetActive(true);
		BackButton.gameObject.SetActive(previousBackButtonState);
		if (previousBackButtonState)
		{
			HardwareBackButtonDispatcher.SetTargetClickHandler(BackButton, false);
		}
		base.GetComponent<AudioSource>().PlayOneShot(CloseButtonAudioClip);
	}

	private void HideAllHeaderTexts()
	{
		HeaderTextWelcome.gameObject.SetActive(false);
		HeaderTextActivate.gameObject.SetActive(false);
		HeaderTextCreate.gameObject.SetActive(false);
		HeaderTextSelectSaved.gameObject.SetActive(false);
		HeaderTextParentPermissions.gameObject.SetActive(false);
	}
}
