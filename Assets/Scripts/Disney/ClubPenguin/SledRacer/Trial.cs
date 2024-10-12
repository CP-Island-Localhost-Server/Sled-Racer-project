using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class Trial : MonoBehaviour
	{
		public GameObject player;

		public PlayerController playerScript;

		private void Start()
		{
			playerScript = player.GetComponent<PlayerController>();
			playerScript.ResetStart();
		}

		private void Update()
		{
		}
	}
}
