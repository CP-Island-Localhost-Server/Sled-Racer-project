using System;
using System.Collections.Generic;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class HighScore
	{
		private static string OFFLINE_HIGH_SCORE_PREF_KEY = "OfflineHighScore";

		private IList<Action<int?>> boundToScore = new List<Action<int?>>();

		public int? Score
		{
			get;
			private set;
		}

		public static int GetOfflineHighScoreFromPrefs()
		{
			return PlayerPrefs.GetInt(OFFLINE_HIGH_SCORE_PREF_KEY);
		}

		public static void SaveOfflineHighScoreInPrefs(int score)
		{
			PlayerPrefs.SetInt(OFFLINE_HIGH_SCORE_PREF_KEY, score);
			PlayerPrefs.Save();
		}

		public void SetScore(int score)
		{
			if (Score != score)
			{
				Score = score;
				foreach (Action<int?> item in boundToScore)
				{
					item(Score);
				}
			}
		}

		public void BindToScore(Action<int?> handleScoreChange)
		{
			boundToScore.Add(handleScoreChange);
			handleScoreChange(Score);
		}

		public void UnBindFromScore(Action<int?> handleScoreChange)
		{
			boundToScore.Remove(handleScoreChange);
		}
	}
}
