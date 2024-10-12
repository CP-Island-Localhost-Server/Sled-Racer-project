using Disney.ClubPenguin.Common.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer
{
	public class FriendMarker : SpawnableTrackObject
	{
		public LayerMask CollisionLayerMask;

		public Image AvatarImage;

		private Vector3 distanceFromPlayer;

		private bool triggered;

		private bool launched;

		private PlayerController player;

		private LayerMask sledTrackMask;

		private ConfigController config;

		private bool topFriend;

		private void Start()
		{
			sledTrackMask = LayerMask.GetMask("SledTrack");
			config = Service.Get<ConfigController>();
		}

		public void SetIsTopFriend(bool isTopFriend)
		{
			topFriend = isTopFriend;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!triggered && CollisionLayerMask.IsSet(other.gameObject.layer))
			{
				triggered = true;
				Vector3 position = base.GetComponent<Rigidbody>().position;
				player = SledRacerGameManager.Instance.playerScript;
				distanceFromPlayer = position - player.GetComponent<Rigidbody>().position;
				base.transform.SetParent(SledRacerGameManager.Instance.GameController.transform, worldPositionStays: true);
				player.TriggerAnimation("RiderCelebrate");
				Invoke("PlaySound", config.FriendMarkerSoundDelaySeconds);
				Invoke("StartLaunch", config.FriendMarkerLaunchDelaySeconds);
				Invoke("DoDestroy", config.FriendMarkerDestroyDelaySeconds);
				Service.Get<IBILogging>().BeatFriendsHighScore(topFriend);
			}
		}

		private void PlaySound()
		{
			Service.Get<IAudio>().SFX.Play(SFXEvent.RecordMarkerPassed, SledRacerGameManager.Instance.PlayerManager);
		}

		private void StartLaunch()
		{
			launched = true;
		}

		private void DoDestroy()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		private void FixedUpdate()
		{
			if (triggered)
			{
				RaycastHit hitInfo;
				if (launched)
				{
					base.GetComponent<Rigidbody>().AddForce(config.FriendMarkerLaunchForce);
				}
				else if (Physics.Raycast(new Ray(base.GetComponent<Rigidbody>().position, Vector3.down), out hitInfo, 5f, sledTrackMask))
				{
					base.GetComponent<Rigidbody>().MovePosition(hitInfo.point + Vector3.up * 5f);
				}
				distanceFromPlayer.z -= distanceFromPlayer.z * (config.FriendMarkerDistanceFromPlayerFadePercent * Time.fixedDeltaTime);
				Vector3 position = base.GetComponent<Rigidbody>().position;
				Vector3 position2 = player.GetComponent<Rigidbody>().position;
				position.z = position2.z + distanceFromPlayer.z;
				base.GetComponent<Rigidbody>().MovePosition(position);
			}
		}
	}
}
