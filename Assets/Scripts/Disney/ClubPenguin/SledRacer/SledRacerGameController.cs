using Disney.ClubPenguin.CPModuleUtils;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class SledRacerGameController : MonoBehaviour
	{
		public float REV = 1f;

		private SledRacerGameManager gameManager;

		private TrackManager trackManager;

		private void Start()
		{
			gameManager = SledRacerGameManager.Instance;
			trackManager = GameObject.Find(base.gameObject.GetPath() + "/TrackManager").GetComponent<TrackManager>();
			PlayerController.OnGameEvent += GameEventHandler;
		}

		private void GameEventHandler(object sender, GameEvent _e)
		{
			GameEvent.Type type = _e.type;
			if (type == GameEvent.Type.End)
			{
				trackManager.ResetEnd();
			}
		}
	}
}
