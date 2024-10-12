using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Disney.ClubPenguin.SledRacer
{
	public class ReplaceWithScene : MonoBehaviour
	{
		public string NewScene;

		private float delay;

		private bool loading;

		private void Awake()
		{
			SledRacerGameManager.OnGameInitFinished += DestroySelf;
		}

		private void Update()
		{
			if (!loading)
			{
				delay -= Time.deltaTime;
				if (delay <= 0f)
				{
					SceneManager.LoadSceneAsync (NewScene, LoadSceneMode.Additive);
					loading = true;
				}
			}
		}

		private void Destroy()
		{
			SledRacerGameManager.OnGameInitFinished -= DestroySelf;
		}

		private void DestroySelf(object sender, EventArgs e)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
