namespace Disney.ClubPenguin.SledRacer.Tests
{
	public class MainMenuVisibleAfterLoad : UIIntegrationTest
	{
		protected override void RunTest()
		{
			IntegrationTest.Assert(panel.activeSelf);
		}
	}
}
