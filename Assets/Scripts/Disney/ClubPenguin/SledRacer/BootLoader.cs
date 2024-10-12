using System;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer
{
	public class BootLoader : MonoBehaviour
	{
		public GameObject LoadingCanvas;

		public Text LegaText;

		private SimpleBootLoader loader;

		public static event Action BootLoadComplete;

		public static void DispatchBootLoadComplete()
		{
			if (BootLoader.BootLoadComplete != null)
			{
				BootLoader.BootLoadComplete();
			}
		}

		private void Awake()
		{
			SledRacerGameManager.OnGameInitFinished += DestroySelf;
			Type typeFromHandle = typeof(SimpleBootLoader);
			//typeFromHandle = typeof(ObbBootLoader);
			GameObject gameObject = new GameObject("Loader", typeFromHandle);
			loader = gameObject.GetComponent<SimpleBootLoader>();
			loader.LoadingCanvas = LoadingCanvas;
			loader.LegalText = LegaText;
		}

		private void Destroy()
		{
			SledRacerGameManager.OnGameInitFinished -= DestroySelf;
		}

		private void DestroySelf(object sender, EventArgs e)
		{
			UnityEngine.Object.Destroy(loader.gameObject);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
