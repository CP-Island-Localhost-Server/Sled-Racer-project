namespace Disney.ClubPenguin.SledRacer.Tests
{
	public class LoadUIManager : UIIntegrationTest
	{
		protected override void OnHarnessLoaded()
		{
			IntegrationTest.Assert(harness != null);
			IntegrationTest.Assert(gameManagerObject != null);
		}
	}
}
