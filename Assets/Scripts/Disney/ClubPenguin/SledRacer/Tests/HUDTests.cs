using Disney.ExtendedTestTools.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer.Tests
{
	public class HUDTests : UIIntegrationTest
	{
		private GameObject HUDPanel;

		protected override void RunTest()
		{
			HUDPanel = Utils.FindChildObject(panel, "TopEdge Panel");
			HUDPanel.SetActive(value: false);
			CheckHUDComponentsAreSetup();
		}

		private void CheckHUDComponentsAreSetup()
		{
			TestPauseButton();
		}

		private void TestPauseButton()
		{
			UnityEngine.Debug.Log("TestPauseButton");
			GameObject parentObject = Utils.FindChildObject(HUDPanel, "HUDPause Panel");
			GameObject gameObject = Utils.FindChildObject(parentObject, "Pause Button");
			Button component = gameObject.GetComponent<Button>();
			UnityEngine.Debug.Log("Found Button: " + component.name);
			IntegrationTest.Assert(component.onClick.GetPersistentEventCount() >= 1);
			UnityEngine.Debug.Log("Button has callback: " + component.onClick.GetPersistentMethodName(0));
		}
	}
}
