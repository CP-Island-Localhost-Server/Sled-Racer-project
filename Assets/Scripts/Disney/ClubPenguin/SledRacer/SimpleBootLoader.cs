using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Disney.ClubPenguin.SledRacer
{
	public class SimpleBootLoader : MonoBehaviour
	{
		public const float LabelSpacing = 25f;

		public const float INTRO_MOVIE_DELAY = 3f;

		public const string LAUNCH_COUNT_KEY = "LaunchCount";

		public const string MUTED_MUSIC_PREF_KEY = "Audio.All.Music.Mute";

		public GameObject LoadingCanvas;

		public Text LegalText;

		public string MainScene = "SledEngine";

		protected string errorMessage;

		private float nextLineOffset;

		private bool triedToLoadMainScene;

		private Type loaderType;

		private bool showIntro;

		private void Start()
		{
			LegalText.gameObject.SetActive(value: false);
			loaderType = GetType();
			VStart();
		}

		protected virtual void VStart()
		{
			StartGame();
		}

		protected void StartGame()
		{
			StartCoroutine(StartGameRoutine());
		}

		private IEnumerator StartGameRoutine()
		{
			int introFrequency = Service.Get<ConfigController>().IntroFrequency;
			int launchCount = PlayerPrefs.GetInt("LaunchCount", 0);
			if (musicOBJ.isMusicPlaying() || PlayerPrefs.GetInt("Audio.All.Music.Mute", 0) == 1)
			{
				showIntro = false;
			}
			else
			{
				showIntro = (launchCount % introFrequency == 0);
			}
			BootLoader.DispatchBootLoadComplete();
			if (showIntro)
			{
				LegalText.gameObject.SetActive(value: true);
				yield return new WaitForSeconds(3f);
				LegalText.gameObject.SetActive(value: false);
				yield return null;
			}
			triedToLoadMainScene = true;
			SceneManager.LoadSceneAsync(MainScene, LoadSceneMode.Additive);
		}

		private void OnGUI()
		{
			nextLineOffset = 10f;
			if (!string.IsNullOrEmpty(errorMessage))
			{
				AddLabelLine(errorMessage);
			}
		}

		protected virtual void VOnGUI()
		{
		}

		protected void AddLabelLine(string message)
		{
			GUI.Label(new Rect(10f, nextLineOffset, Screen.width - 10, 20f), message);
			nextLineOffset += 25f;
		}
	}
}
