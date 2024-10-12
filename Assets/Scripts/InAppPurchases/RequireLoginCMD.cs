using Disney.ClubPenguin.Login.Authentication;
using Disney.ClubPenguin.Service.MWS;
using System;
using UnityEngine;

namespace InAppPurchases
{
	public class RequireLoginCMD
	{
		private LoginContext loginContextPrefab;

		private RectTransform uiContainer;

		private LoginContext loginContextInstance;

		private IAPModel iapModel;

		private string appId;

		private Action<bool> loginCompleteHandler;

		public RequireLoginCMD(LoginContext loginContextPrefab, RectTransform uiContainer, IAPModel iapModel, string appId, Action<bool> loginCompleteHandler)
		{
			this.loginContextPrefab = loginContextPrefab;
			this.uiContainer = uiContainer;
			this.iapModel = iapModel;
			this.appId = appId;
			this.loginCompleteHandler = loginCompleteHandler;
		}

		public void Execute()
		{
			loginContextInstance = (UnityEngine.Object.Instantiate(loginContextPrefab) as LoginContext);
			loginContextInstance.GetComponent<RectTransform>().SetParent(uiContainer, worldPositionStays: false);
			LoginContext loginContext = loginContextInstance;
			loginContext.LoginSucceeded = (LoginContext.LoginSucceededDelegate)Delegate.Combine(loginContext.LoginSucceeded, new LoginContext.LoginSucceededDelegate(OnLoginSucceededHandler));
			LoginContext loginContext2 = loginContextInstance;
			loginContext2.LoginClosed = (LoginContext.LoginClosedDelegate)Delegate.Combine(loginContext2.LoginClosed, new LoginContext.LoginClosedDelegate(OnLoginClosedHandler));
			loginContextInstance.AppID = appId;
			loginContextInstance.DetermineAndShowLoginState();
		}

		private void OnLoginSucceededHandler(IGetAuthTokenResponse response, string username, string password)
		{
			LoginContext loginContext = loginContextInstance;
			loginContext.LoginSucceeded = (LoginContext.LoginSucceededDelegate)Delegate.Remove(loginContext.LoginSucceeded, new LoginContext.LoginSucceededDelegate(OnLoginSucceededHandler));
			LoginContext loginContext2 = loginContextInstance;
			loginContext2.LoginClosed = (LoginContext.LoginClosedDelegate)Delegate.Remove(loginContext2.LoginClosed, new LoginContext.LoginClosedDelegate(OnLoginClosedHandler));
			if (response.AuthData.Member)
			{
				iapModel.IapViewType = IAPViewType.MEMBER;
			}
			else
			{
				iapModel.IapViewType = IAPViewType.NONMEMBER;
			}
			iapModel.PlayerID = response.AuthData.PlayerId;
			loginCompleteHandler(obj: true);
		}

		private void OnLoginClosedHandler()
		{
			LoginContext loginContext = loginContextInstance;
			loginContext.LoginSucceeded = (LoginContext.LoginSucceededDelegate)Delegate.Remove(loginContext.LoginSucceeded, new LoginContext.LoginSucceededDelegate(OnLoginSucceededHandler));
			LoginContext loginContext2 = loginContextInstance;
			loginContext2.LoginClosed = (LoginContext.LoginClosedDelegate)Delegate.Remove(loginContext2.LoginClosed, new LoginContext.LoginClosedDelegate(OnLoginClosedHandler));
			loginCompleteHandler(obj: false);
		}
	}
}
