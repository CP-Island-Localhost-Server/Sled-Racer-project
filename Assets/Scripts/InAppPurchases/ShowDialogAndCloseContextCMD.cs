using System;

namespace InAppPurchases
{
	public class ShowDialogAndCloseContextCMD
	{
		private MessageDialogOverlay messageDialogOverlay;

		private string dialogMessage;

		private string statusCode;

		public ShowDialogAndCloseContextCMD(MessageDialogOverlay messageDialogOverlay, string dialogMessage, string statusCode = null)
		{
			this.messageDialogOverlay = messageDialogOverlay;
			this.dialogMessage = dialogMessage;
			this.statusCode = statusCode;
		}

		public void Execute()
		{
			MessageDialogOverlay obj = messageDialogOverlay;
			obj.Closed = (MessageDialogOverlay.ClosedDelegate)Delegate.Combine(obj.Closed, (MessageDialogOverlay.ClosedDelegate)delegate
			{
				messageDialogOverlay.Closed = null;
				new ForceCloseAllIapContextsCMD().Execute();
			});
			messageDialogOverlay.ShowStatusTextFromString(dialogMessage, statusCode);
		}
	}
}
