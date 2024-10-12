using Disney.ClubPenguin.Login.BI;
using Disney.ClubPenguin.Service.MWS;
using Disney.ClubPenguin.Service.PDR;
using Disney.HTTP.Client;
using System;
using UnityEngine;

namespace Disney.ClubPenguin.Login.Authentication
{
	public class DoAutoLoginCMD
	{
		private string appId;

		private string appVersion;

		private IMWSClient mwsClient;

		private IPDRClient pdrClient;

		private ILoginBIUtils loginBiUtils;

		private MonoBehaviour timeoutCoRoutineBehaviour;

		private float requestTimeoutSec;

		public event Action LoginRequestSent;

		public event Action<IHTTPResponse> LoginFailed;

		public event Action<byte[]> PaperDollReceived;

		public event Action<IGetAuthTokenResponse, string, string> LoginSucceeded;

		public DoAutoLoginCMD(string appId, string appVersion, IMWSClient mwsClient, IPDRClient pdrClient, ILoginBIUtils loginBiUtils, MonoBehaviour timeoutCoRoutineBehaviour, float requestTimeoutSec = 30f)
		{
			this.appId = appId;
			this.appVersion = appVersion;
			this.mwsClient = mwsClient;
			this.pdrClient = pdrClient;
			this.loginBiUtils = loginBiUtils;
			this.timeoutCoRoutineBehaviour = timeoutCoRoutineBehaviour;
			this.requestTimeoutSec = requestTimeoutSec;
		}

		public void Execute()
		{
			SavedPlayerCollection savedPlayerCollection = new SavedPlayerCollection();
			if (savedPlayerCollection.ExistsOnDisk())
			{
				savedPlayerCollection.LoadFromDisk();
				SavedPlayerData mostRecentlyLoggedInPlayer = savedPlayerCollection.GetMostRecentlyLoggedInPlayer();
				if (mostRecentlyLoggedInPlayer != null && mostRecentlyLoggedInPlayer.Password != null && mostRecentlyLoggedInPlayer.Password != string.Empty)
				{
					DoLoginCMD doLoginCMD = new DoLoginCMD(mostRecentlyLoggedInPlayer.UserName, mostRecentlyLoggedInPlayer.Password,  true, appId, appVersion, mwsClient, pdrClient, timeoutCoRoutineBehaviour, requestTimeoutSec);
					doLoginCMD.LoginSucceeded += OnLoginSucceeded;
					doLoginCMD.LoginFailed += OnLoginFailed;
					doLoginCMD.InvalidInputSpecified += OnLoginFailed;
					doLoginCMD.LoginRequestSent += OnLoginRequestSent;
					doLoginCMD.PaperDollReceived += OnPaperDollReceived;
					doLoginCMD.Execute();
					return;
				}
			}
			OnLoginFailed(null);
		}

		public void OnLoginSucceeded(IGetAuthTokenResponse response, string username, string password)
		{
			loginBiUtils.SendPlayerInfo(response.AuthData.PlayerId, response.AuthData.Username);
			if (this.LoginSucceeded != null)
			{
				this.LoginSucceeded(response, username, password);
			}
		}

		public void OnLoginFailed(IHTTPResponse response)
		{
			if (this.LoginFailed != null)
			{
				this.LoginFailed(response);
			}
		}

		private void OnLoginFailed(string arg1, bool arg2)
		{
			if (this.LoginFailed != null)
			{
				this.LoginFailed(null);
			}
		}

		public void OnLoginRequestSent()
		{
			if (this.LoginRequestSent != null)
			{
				this.LoginRequestSent();
			}
		}

		public void OnPaperDollReceived(byte[] paperDollBytes)
		{
			if (this.PaperDollReceived != null)
			{
				this.PaperDollReceived(paperDollBytes);
			}
		}
	}
}
