using UnityEngine;

namespace Disney.ClubPenguin.SledRacer.Tests
{
	public class UIManagerClassExists : UIIntegrationTest
	{
		protected override void OnHarnessLoaded()
		{
			GameObject x = GameObject.Find("/UIManager");
			IntegrationTest.Assert(x != null);
			IntegrationTest.Assert(harness.UIManager != null);
		}
	}
}
