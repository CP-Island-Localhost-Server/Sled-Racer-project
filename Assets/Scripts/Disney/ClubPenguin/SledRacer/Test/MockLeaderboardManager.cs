using Disney.ClubPenguin.Service.MWS.Domain;
using System;
using System.Collections.Generic;

namespace Disney.ClubPenguin.SledRacer.Test
{
	public class MockLeaderboardManager : LeaderboardManager
	{
		public override void LoadAllHighScores(string gameType, Action<LeaderBoardResponse> SetHighScoresCallback)
		{
			LeaderBoardResponse leaderBoardResponse = new LeaderBoardResponse();
			leaderBoardResponse.Players = new List<LeaderBoardHighScore>();
			for (int i = 0; i < 100; i++)
			{
				LeaderBoardHighScore leaderBoardHighScore = new LeaderBoardHighScore();
				leaderBoardHighScore.Rank = i;
				leaderBoardHighScore.IsFriend = (i % 2 == 0);
				if (i != 50)
				{
					leaderBoardHighScore.PlayerSWID = "{" + i + "}";
				}
				else
				{
					leaderBoardHighScore.PlayerSWID = "{-1}";
				}
				leaderBoardHighScore.Name = "Player " + leaderBoardHighScore.PlayerSWID;
				leaderBoardHighScore.Score = i * 25;
				leaderBoardResponse.Players.Add(leaderBoardHighScore);
			}
			SetHighScoresCallback(leaderBoardResponse);
		}
	}
}
