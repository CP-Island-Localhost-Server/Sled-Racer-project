using Disney.ClubPenguin.ParentPermission;
using System;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class ParentGatePanelController : BaseMenuController
	{
		public delegate void OnCompleteDelegate();

		public OnCompleteDelegate OnComplete;

		public ParentPermissionController parentPermissionPrefab;

		public RectTransform parentPermissionContent;

		private ParentPermissionController parentPermissionInstance;

		private Action<string> parentPermissionCallbackFunction;

		private string parentPermissionURL;

		protected override void VStart()
		{
		}

		protected override void Init()
		{
		}

		public void ShowParentPermisson(Action<string> callback, string url = "")
		{
			UnityEngine.Debug.Log("obj " + base.gameObject.activeSelf);
			base.gameObject.SetActive(value: true);
			parentPermissionURL = url;
			parentPermissionCallbackFunction = callback;
			if (parentPermissionInstance == null)
			{
				parentPermissionInstance = (UnityEngine.Object.Instantiate(parentPermissionPrefab) as ParentPermissionController);
				parentPermissionInstance.GetComponent<RectTransform>().SetParent(parentPermissionContent, worldPositionStays: false);
			}
			parentPermissionInstance.onFailClose += OnHideParentPermission;
			parentPermissionInstance.onSuccess += OnParentPermissionSuccess;
			ParentPermissionController parentPermissionController = parentPermissionInstance;
			IBILogging iBILogging = Service.Get<IBILogging>();
			parentPermissionController.onFailClose += iBILogging.ParentGateClosed;
			ParentPermissionController parentPermissionController2 = parentPermissionInstance;
			IBILogging iBILogging2 = Service.Get<IBILogging>();
			parentPermissionController2.onSuccess += iBILogging2.ParentGateClosed;
		}

		public void OnCloseButton()
		{
			OnHideParentPermission(-1);
		}

		private void OnHideParentPermission(int age)
		{
			if (OnComplete != null)
			{
				OnComplete();
			}
			parentPermissionInstance.onFailClose -= OnHideParentPermission;
			parentPermissionInstance.onSuccess -= OnParentPermissionSuccess;
			ParentPermissionController parentPermissionController = parentPermissionInstance;
			IBILogging iBILogging = Service.Get<IBILogging>();
			parentPermissionController.onFailClose -= iBILogging.ParentGateClosed;
			ParentPermissionController parentPermissionController2 = parentPermissionInstance;
			IBILogging iBILogging2 = Service.Get<IBILogging>();
			parentPermissionController2.onSuccess -= iBILogging2.ParentGateClosed;
		}

		private void OnParentPermissionSuccess(int age)
		{
			parentPermissionCallbackFunction(parentPermissionURL);
			OnHideParentPermission(age);
		}
	}
}
