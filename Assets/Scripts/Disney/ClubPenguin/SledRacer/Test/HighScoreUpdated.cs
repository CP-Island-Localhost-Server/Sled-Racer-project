using Disney.ClubPenguin.SledRacer.Tests;

namespace Disney.ClubPenguin.SledRacer.Test
{
	public class HighScoreUpdated : UIIntegrationTest
	{
		private const int lastScore = 10;

		private const int newScore = 20;

		private EndGameMenuController controller;

		private bool running;

		protected override void OnHarnessLoaded()
		{
			Service.Get<PlayerDataService>().PlayerData.HighScore.SetScore(10);
			Service.Get<PlayerDataService>().PlayerData.HighScore.SetScore(20);
		}

		protected override void RunTest()
		{
			controller = panel.GetComponent<EndGameMenuController>();
			running = true;
		}

		protected override void DoUpdate()
		{
			if (running && 20.ToString().Equals(controller.HighScoreText.text))
			{
				IntegrationTest.Pass();
			}
		}
	}
}
