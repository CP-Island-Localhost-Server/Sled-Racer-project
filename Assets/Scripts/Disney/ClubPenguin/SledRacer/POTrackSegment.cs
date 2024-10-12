using Disney.ClubPenguin.Common.Utils;
using Disney.ObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer
{
	public class POTrackSegment : PoolableObject
	{
		public Transform endSocket;

		private Vector3 targetPosition;

		private Vector3 offsetPosition;

		private float tweenTimer;

		private bool TweenTrack;

		public int useCount;

		private GameObject ActiveTrackDecoration;

		public GameObject[] TrackDecorations;

		private LayerMask sledTrackMask;

		private ConfigController config;

		public Vector3 EndPoint => targetPosition + endSocket.transform.localPosition;

		public Vector3 LocalEndPoint => endSocket.transform.localPosition;

		private void Awake()
		{
			config = Service.Get<ConfigController>();
			sledTrackMask = LayerMask.GetMask("SledTrack");
			for (int i = 0; i < TrackDecorations.Length; i++)
			{
				if (TrackDecorations[i] != null)
				{
					TrackDecorations[i].SetActive(value: false);
				}
			}
			GameObject[] array = GameObject.FindGameObjectsWithTag("AttachToTrack");
			GameObject[] array2 = array;
			foreach (GameObject obj in array2)
			{
				PhysicsUtil.AttachObjectToLayer(obj, sledTrackMask, 100f, Vector3.down);
			}
			List<GameObject> list = new List<GameObject>();
			FindChildrenByTag("tree", base.transform, list);
			StaticBatchingUtility.Combine(list.ToArray(), base.gameObject);
			List<GameObject> list2 = new List<GameObject>();
			FindChildrenByTag("rock", base.transform, list2);
			StaticBatchingUtility.Combine(list2.ToArray(), base.gameObject);
			List<GameObject> list3 = new List<GameObject>();
			FindChildrenByTag("rockpile", base.transform, list3);
			StaticBatchingUtility.Combine(list3.ToArray(), base.gameObject);
			List<GameObject> list4 = new List<GameObject>();
			FindChildrenByTag("icepatch", base.transform, list4);
			StaticBatchingUtility.Combine(list4.ToArray(), base.gameObject);
			List<GameObject> list5 = new List<GameObject>();
			FindChildrenByTag("log", base.transform, list5);
			StaticBatchingUtility.Combine(list5.ToArray(), base.gameObject);
		}

		private void FindChildrenByTag(string tag, Transform parent, List<GameObject> list)
		{
			foreach (Transform item in parent)
			{
				if (item.tag == tag)
				{
					list.Add(item.gameObject);
				}
				FindChildrenByTag(tag, item, list);
			}
		}

		public void Spawn(Transform newParent, Vector3 position, Quaternion rotation, int difficulty = 0, bool _immediate = false)
		{
			base.Spawn(newParent, position, rotation);
			TweenTrack = !_immediate;
			if (TrackDecorations.Length > 0 && TrackDecorations[difficulty] != null)
			{
				ActiveTrackDecoration = TrackDecorations[difficulty];
				ActiveTrackDecoration.SetActive(value: true);
			}
			Prepare();
		}

		public void SpawnHighScoreMarker(HighScoreMarkerType type, float localZ, string nameLabel, int colorID, bool isTopFriend)
		{
			GameObject gameObject = null;
			switch (type)
			{
			case HighScoreMarkerType.LocalAllTime:
				gameObject = Resources.Load<GameObject>("Prefabs/recordMarker");
				break;
			case HighScoreMarkerType.LocalWeekly:
				gameObject = Resources.Load<GameObject>("Prefabs/recordMarker");
				break;
			case HighScoreMarkerType.Friend:
				gameObject = Resources.Load<GameObject>("Prefabs/friendMarker");
				break;
			default:
				throw new Exception("HighScoreMarkerType of type '" + type + "' is not supported by SpawnHighScoreMarker.");
			}
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject) as GameObject;
			RecordMarker component = gameObject2.GetComponent<RecordMarker>();
			if (component == null)
			{
				Text componentInChildren = gameObject2.GetComponentInChildren<Text>();
				if (componentInChildren != null)
				{
					componentInChildren.text = ((nameLabel == null) ? string.Empty : nameLabel);
				}
			}
			else
			{
				component.SetMode(type);
			}
			if (type == HighScoreMarkerType.Friend)
			{
				FriendMarker component2 = gameObject2.GetComponent<FriendMarker>();
				component2.SetIsTopFriend(isTopFriend);
				if (component2.AvatarImage != null)
				{
					component2.AvatarImage.sprite = AvatarUtil.GetSmallAvatar(colorID);
				}
			}
			gameObject2.transform.SetParent(base.transform);
			gameObject2.transform.localPosition = new Vector3(0f, 30f, localZ);
			Vector3? offset = null;
			if (type == HighScoreMarkerType.Friend)
			{
				offset = new Vector3(0f, Service.Get<ConfigController>().FriendMarkerDistanceFromGround, 0f);
			}
			PhysicsUtil.AttachObjectToLayer(gameObject2, sledTrackMask, 130f, Vector3.down, offset);
		}

		private void Prepare()
		{
			useCount++;
			targetPosition = base.transform.position;
			if (TweenTrack)
			{
				tweenTimer = 0f;
				offsetPosition = base.transform.position + Vector3.down * config.TrackTweenDistance;
				base.transform.position = offsetPosition;
			}
		}

		public void ShiftTrack(Vector3 _shiftVector)
		{
			base.transform.position += _shiftVector;
			targetPosition += _shiftVector;
			offsetPosition += _shiftVector;
		}

		private void Update()
		{
			if (TweenTrack)
			{
				Vector3 position = base.transform.position;
				tweenTimer += Time.deltaTime;
				float num = tweenTimer / config.TrackTweenTime;
				position = Vector3.Lerp(offsetPosition, targetPosition, num);
				base.transform.position = position;
				if (num >= 1f)
				{
					TweenTrack = false;
				}
			}
		}

		public override void Despawn()
		{
			CleanupTrackDecorations();
			base.Despawn();
		}

		private void CleanupTrackDecorations()
		{
			if (ActiveTrackDecoration != null)
			{
				ActiveTrackDecoration.SetActive(value: false);
				ActiveTrackDecoration = null;
			}
			SpawnableTrackObject[] componentsInChildren = GetComponentsInChildren<SpawnableTrackObject>(includeInactive: true);
			SpawnableTrackObject[] array = componentsInChildren;
			foreach (SpawnableTrackObject spawnableTrackObject in array)
			{
				UnityEngine.Object.Destroy(spawnableTrackObject.gameObject);
			}
		}
	}
}
