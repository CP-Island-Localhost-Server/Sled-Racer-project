namespace Disney.ClubPenguin.SledRacer.Tests
{
	public class MainMenuButtonsAreSetup : UIIntegrationTest
	{
		public string[] ExcludedButtonContainerNames;

		protected override void RunTest()
		{
			CheckButtonsAreSetup(ExcludedButtonContainerNames);
		}
	}
}
