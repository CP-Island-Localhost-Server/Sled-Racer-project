using Prime31;
using System.Collections.Generic;
using UnityEngine;

public class EtceteraAndroidEventListener : MonoBehaviour
{
	private void OnEnable()
	{
		EtceteraAndroidManager.alertButtonClickedEvent += alertButtonClickedEvent;
		EtceteraAndroidManager.alertCancelledEvent += alertCancelledEvent;
		EtceteraAndroidManager.promptFinishedWithTextEvent += promptFinishedWithTextEvent;
		EtceteraAndroidManager.promptCancelledEvent += promptCancelledEvent;
		EtceteraAndroidManager.twoFieldPromptFinishedWithTextEvent += twoFieldPromptFinishedWithTextEvent;
		EtceteraAndroidManager.twoFieldPromptCancelledEvent += twoFieldPromptCancelledEvent;
		EtceteraAndroidManager.webViewCancelledEvent += webViewCancelledEvent;
		EtceteraAndroidManager.inlineWebViewJSCallbackEvent += inlineWebViewJSCallbackEvent;
		EtceteraAndroidManager.albumChooserCancelledEvent += albumChooserCancelledEvent;
		EtceteraAndroidManager.albumChooserSucceededEvent += albumChooserSucceededEvent;
		EtceteraAndroidManager.photoChooserCancelledEvent += photoChooserCancelledEvent;
		EtceteraAndroidManager.photoChooserSucceededEvent += photoChooserSucceededEvent;
		EtceteraAndroidManager.videoRecordingCancelledEvent += videoRecordingCancelledEvent;
		EtceteraAndroidManager.videoRecordingSucceededEvent += videoRecordingSucceededEvent;
		EtceteraAndroidManager.ttsInitializedEvent += ttsInitializedEvent;
		EtceteraAndroidManager.ttsFailedToInitializeEvent += ttsFailedToInitializeEvent;
		EtceteraAndroidManager.askForReviewDontAskAgainEvent += askForReviewDontAskAgainEvent;
		EtceteraAndroidManager.askForReviewRemindMeLaterEvent += askForReviewRemindMeLaterEvent;
		EtceteraAndroidManager.askForReviewWillOpenMarketEvent += askForReviewWillOpenMarketEvent;
		EtceteraAndroidManager.notificationReceivedEvent += notificationReceivedEvent;
		EtceteraAndroidManager.contactsLoadedEvent += contactsLoadedEvent;
	}

	private void OnDisable()
	{
		EtceteraAndroidManager.alertButtonClickedEvent -= alertButtonClickedEvent;
		EtceteraAndroidManager.alertCancelledEvent -= alertCancelledEvent;
		EtceteraAndroidManager.promptFinishedWithTextEvent -= promptFinishedWithTextEvent;
		EtceteraAndroidManager.promptCancelledEvent -= promptCancelledEvent;
		EtceteraAndroidManager.twoFieldPromptFinishedWithTextEvent -= twoFieldPromptFinishedWithTextEvent;
		EtceteraAndroidManager.twoFieldPromptCancelledEvent -= twoFieldPromptCancelledEvent;
		EtceteraAndroidManager.webViewCancelledEvent -= webViewCancelledEvent;
		EtceteraAndroidManager.inlineWebViewJSCallbackEvent -= inlineWebViewJSCallbackEvent;
		EtceteraAndroidManager.albumChooserCancelledEvent -= albumChooserCancelledEvent;
		EtceteraAndroidManager.albumChooserSucceededEvent -= albumChooserSucceededEvent;
		EtceteraAndroidManager.photoChooserCancelledEvent -= photoChooserCancelledEvent;
		EtceteraAndroidManager.photoChooserSucceededEvent -= photoChooserSucceededEvent;
		EtceteraAndroidManager.videoRecordingCancelledEvent -= videoRecordingCancelledEvent;
		EtceteraAndroidManager.videoRecordingSucceededEvent -= videoRecordingSucceededEvent;
		EtceteraAndroidManager.ttsInitializedEvent -= ttsInitializedEvent;
		EtceteraAndroidManager.ttsFailedToInitializeEvent -= ttsFailedToInitializeEvent;
		EtceteraAndroidManager.askForReviewDontAskAgainEvent -= askForReviewDontAskAgainEvent;
		EtceteraAndroidManager.askForReviewRemindMeLaterEvent -= askForReviewRemindMeLaterEvent;
		EtceteraAndroidManager.askForReviewWillOpenMarketEvent -= askForReviewWillOpenMarketEvent;
		EtceteraAndroidManager.notificationReceivedEvent -= notificationReceivedEvent;
		EtceteraAndroidManager.contactsLoadedEvent -= contactsLoadedEvent;
	}

	private void alertButtonClickedEvent(string positiveButton)
	{
		UnityEngine.Debug.Log("alertButtonClickedEvent: " + positiveButton);
	}

	private void alertCancelledEvent()
	{
		UnityEngine.Debug.Log("alertCancelledEvent");
	}

	private void promptFinishedWithTextEvent(string param)
	{
		UnityEngine.Debug.Log("promptFinishedWithTextEvent: " + param);
	}

	private void promptCancelledEvent()
	{
		UnityEngine.Debug.Log("promptCancelledEvent");
	}

	private void twoFieldPromptFinishedWithTextEvent(string text1, string text2)
	{
		UnityEngine.Debug.Log("twoFieldPromptFinishedWithTextEvent: " + text1 + ", " + text2);
	}

	private void twoFieldPromptCancelledEvent()
	{
		UnityEngine.Debug.Log("twoFieldPromptCancelledEvent");
	}

	private void webViewCancelledEvent()
	{
		UnityEngine.Debug.Log("webViewCancelledEvent");
	}

	private void inlineWebViewJSCallbackEvent(string message)
	{
		UnityEngine.Debug.Log("inlineWebViewJSCallbackEvent: " + message);
	}

	private void albumChooserCancelledEvent()
	{
		UnityEngine.Debug.Log("albumChooserCancelledEvent");
	}

	private void albumChooserSucceededEvent(string imagePath)
	{
		UnityEngine.Debug.Log("albumChooserSucceededEvent: " + imagePath);
		UnityEngine.Debug.Log("image size: " + EtceteraAndroid.getImageSizeAtPath(imagePath));
	}

	private void photoChooserCancelledEvent()
	{
		UnityEngine.Debug.Log("photoChooserCancelledEvent");
	}

	private void photoChooserSucceededEvent(string imagePath)
	{
		UnityEngine.Debug.Log("photoChooserSucceededEvent: " + imagePath);
		UnityEngine.Debug.Log("image size: " + EtceteraAndroid.getImageSizeAtPath(imagePath));
	}

	private void videoRecordingCancelledEvent()
	{
		UnityEngine.Debug.Log("videoRecordingCancelledEvent");
	}

	private void videoRecordingSucceededEvent(string path)
	{
		UnityEngine.Debug.Log("videoRecordingSucceededEvent: " + path);
	}

	private void ttsInitializedEvent()
	{
		UnityEngine.Debug.Log("ttsInitializedEvent");
	}

	private void ttsFailedToInitializeEvent()
	{
		UnityEngine.Debug.Log("ttsFailedToInitializeEvent");
	}

	private void askForReviewDontAskAgainEvent()
	{
		UnityEngine.Debug.Log("askForReviewDontAskAgainEvent");
	}

	private void askForReviewRemindMeLaterEvent()
	{
		UnityEngine.Debug.Log("askForReviewRemindMeLaterEvent");
	}

	private void askForReviewWillOpenMarketEvent()
	{
		UnityEngine.Debug.Log("askForReviewWillOpenMarketEvent");
	}

	private void notificationReceivedEvent(string extraData)
	{
		UnityEngine.Debug.Log("notificationReceivedEvent: " + extraData);
	}

	private void contactsLoadedEvent(List<EtceteraAndroid.Contact> contacts)
	{
		UnityEngine.Debug.Log("contactsLoadedEvent");
		Utils.logObject(contacts);
	}
}
