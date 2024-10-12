using System.Collections;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class ObbBootLoader : SimpleBootLoader
	{
		private string expansionPath;

		private string mainExpansionPath;

		private bool isDownloadingOBB;

		protected override void VStart()
		{
			if (GooglePlayDownloader.RunningOnAndroid())
			{
				expansionPath = GooglePlayDownloader.GetExpansionFilePath();
				if (!CheckHasExternalStorage())
				{
					errorMessage = "External storage is not available!";
					UnityEngine.Debug.LogWarning(errorMessage + " Attempting to load main scene anyways.");
					StartGame();
				}
				else if (!CheckHasOBB())
				{
					isDownloadingOBB = true;
					GooglePlayDownloader.FetchOBB();
					StartCoroutine(WaitForOBB());
				}
				else
				{
					StartGame();
				}
			}
			else
			{
				errorMessage = "GooglePlayDownloader.RunningOnAndroid() == false!";
				UnityEngine.Debug.LogWarning(errorMessage + " Attempting to load main scene anyways.");
				StartGame();
			}
		}

		private IEnumerator WaitForOBB()
		{
			while (isDownloadingOBB && !CheckHasOBB())
			{
				yield return new WaitForSeconds(0.5f);
			}
			isDownloadingOBB = false;
			StartGame();
		}

		protected override void VOnGUI()
		{
			base.VOnGUI();
			AddLabelLine("isDownloadingOBB = " + isDownloadingOBB);
			AddLabelLine("expansionPath = " + expansionPath);
			AddLabelLine("mainExpansionPath = " + ((!string.IsNullOrEmpty(mainExpansionPath)) ? mainExpansionPath.Substring(expansionPath.Length) : " NOT AVAILABLE"));
		}

		private bool CheckHasExternalStorage()
		{
			return expansionPath != null;
		}

		private bool CheckHasOBB()
		{
			mainExpansionPath = GooglePlayDownloader.GetMainOBBPath(expansionPath);
			return mainExpansionPath != null;
		}
	}
}
