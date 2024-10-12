using DevonLocalization;
using DevonLocalization.Core;
using Disney.ClubPenguin.Service.MWS;
using Disney.ClubPenguin.Service.PDR;
using Disney.HTTP.Client;
using ErrorPopup;
using ErrorPopup.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.Login.Authentication
{
	public class LoginController : MonoBehaviour
	{
		public delegate void LoginSucceededDelegate(IGetAuthTokenResponse response, string username, string password);

		public delegate void PaperDollReceivedDelegate(byte[] paperDollBytes);

		public delegate void PaperDollFailedDelegate();

		public delegate void LoginAttempStartedDelegate();

		public delegate void ForgotPasswordClickedDelegate(string url);

		private IMWSClient _Client;

		private IPDRClient _PDRClient;

		public LoginSucceededDelegate LoginSucceeded;

		public PaperDollReceivedDelegate PaperDollRecevied;

		public PaperDollFailedDelegate PaperDollFailed;

		public LoginAttempStartedDelegate LoginAttemptStarted;

		public ForgotPasswordClickedDelegate ForgotPasswordClicked;

		public string AppID = "CPMCAPP";

		public string AppVersion = "1.6";

		public AudioClip LoginClickedAudioClip;

		public AudioClip ErrorPopupAudioClip;

		public AudioClip SelectForgotPasswordAudioClip;

		public InputField nameInputField;

		public InputField passwordInputField;

		public Toggle savePasswordToggle;

		public LocalizedWebLink localizedWebLink;

		[HideInInspector]
		public AudioSource RootAudioSource;

		[HideInInspector]
		public bool ShowErrorPopups = true;

		[HideInInspector]
		public float LoginRequestTimeout = 2f;

		public IMWSClient Client
		{
			get;
			set;
		}

		public IPDRClient PDRClient
		{
			get;
			set;
		}

		public event Action<IHTTPResponse> LoginFailed;

		private void Start()
		{
			if (localizedWebLink != null)
			{
				LocalizedWebLink obj = localizedWebLink;
				obj.WebLinkClicked = (LocalizedWebLink.WebLinkClickedDelegate)Delegate.Combine(obj.WebLinkClicked, new LocalizedWebLink.WebLinkClickedDelegate(OnForgotPasswordClicked));
			}
		}

		private void OnDestroy()
		{
			if (localizedWebLink != null)
			{
				LocalizedWebLink obj = localizedWebLink;
				obj.WebLinkClicked = (LocalizedWebLink.WebLinkClickedDelegate)Delegate.Remove(obj.WebLinkClicked, new LocalizedWebLink.WebLinkClickedDelegate(OnForgotPasswordClicked));
			}
		}

		public void DoLogin()
		{
			PlayAudioAndLogin(nameInputField.text, passwordInputField.text, savePasswordToggle.isOn);
		}

		public void PlayAudioAndLogin(string username, string password, bool savePassword)
		{
			RootAudioSource.PlayOneShot(LoginClickedAudioClip);
			DoLogin(username, password, savePassword);
		}

		public void DoLogin(string username, string password, bool savePassword)
		{
			hideAllErrorPopups();
			DoLoginCMD doLoginCMD = new DoLoginCMD(username, password, savePassword, AppID, AppVersion, Client, PDRClient, this, LoginRequestTimeout);
			doLoginCMD.LoginRequestSent += OnLoginRequestSent;
			doLoginCMD.LoginSucceeded += OnLoginSucceeded;
			doLoginCMD.InvalidInputSpecified += OnInvalidInputSpecified;
			doLoginCMD.LoginFailed += onLoginFailure;
			doLoginCMD.PaperDollFailed += OnPaperDollFailed;
			doLoginCMD.PaperDollReceived += OnPaperDollReceived;
			doLoginCMD.Execute();
		}

		private void OnLoginRequestSent()
		{
			if (LoginAttemptStarted != null)
			{
				LoginAttemptStarted();
			}
		}

		private void OnInvalidInputSpecified(string errorCode, bool isPasswordError)
		{
			showErrorPopup(errorCode, isPasswordError);
			if (this.LoginFailed != null)
			{
				this.LoginFailed(new HTTPErrorResponse(401, "Invalid input specified"));
			}
		}

		private void OnLoginSucceeded(IGetAuthTokenResponse response, string username, string password)
		{
			if (LoginSucceeded != null)
			{
				LoginSucceeded(response, username, password);
			}
		}

		private void onLoginFailure(IHTTPResponse failure)
		{
			string errorCode = (!(failure is IGetAuthTokenResponse) || ((IGetAuthTokenResponse)failure).ResponseError == null) ? failure.StatusCode.ToString() : ((IGetAuthTokenResponse)failure).ResponseError.ErrorResponse.errorCode.ToString();
			showErrorPopup(errorCode);
			if (this.LoginFailed != null)
			{
				this.LoginFailed(failure);
			}
		}

		private void OnPaperDollFailed(string errorMessage)
		{
			if (PaperDollFailed != null)
			{
				PaperDollFailed();
			}
		}

		private void OnPaperDollReceived(byte[] paperDollBytes)
		{
			if (PaperDollRecevied != null)
			{
				PaperDollRecevied(paperDollBytes);
			}
		}

		private void showErrorPopup(string errorCode, bool showInPasswordField = false)
		{
			if (ShowErrorPopups)
			{
				string errorMessage = ErrorsMap.Instance.GetErrorMessage(errorCode);
				string tokenTranslation = Localizer.Instance.GetTokenTranslation(errorMessage);
				if (nameInputField != null && !showInPasswordField)
				{
					nameInputField.GetComponent<ErrorPopupComponent>().ShowError(tokenTranslation);
				}
				else
				{
					passwordInputField.GetComponent<ErrorPopupComponent>().ShowError(tokenTranslation);
				}
				RootAudioSource.PlayOneShot(ErrorPopupAudioClip);
			}
		}

		public void hideAllErrorPopups()
		{
			if (nameInputField != null)
			{
				nameInputField.GetComponent<ErrorPopupComponent>().HideError();
			}
			if (passwordInputField != null)
			{
				passwordInputField.GetComponent<ErrorPopupComponent>().HideError();
			}
		}

		private void OnForgotPasswordClicked(string url)
		{
			if (ForgotPasswordClicked != null)
			{
				hideAllErrorPopups();
				RootAudioSource.PlayOneShot(SelectForgotPasswordAudioClip);
				ForgotPasswordClicked(url);
			}
		}
	}
}
