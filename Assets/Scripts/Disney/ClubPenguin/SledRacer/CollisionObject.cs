using System;
using System.Reflection;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class CollisionObject : MonoBehaviour
	{
		private static int UID;

		private int myUID;

		public bool CrashColliderOnly = true;

		public Collider[] CrashColliders;

		public Collider[] BounceColliders;

		private int playerLayerID;

		public static event Action ResetAllColliders;

		private static event Action<GameObject> PlayerCrashed;

		public CollisionObject()
		{
			myUID = UID++;
		}

		public static void DispatchReset()
		{
			if (CollisionObject.ResetAllColliders != null)
			{
				CollisionObject.ResetAllColliders();
			}
		}

		public static void DispatchPlayerCrashed(GameObject collidedObject)
		{
			if (CollisionObject.PlayerCrashed != null)
			{
				CollisionObject.PlayerCrashed(collidedObject);
			}
		}

		private void Start()
		{
			SledRacerGameManager instanceAs = GameManager.GetInstanceAs<SledRacerGameManager>();
			playerLayerID = LayerMask.NameToLayer("Player");
			if (instanceAs != null && !instanceAs.CanBounceOnObstacles)
			{
				EnableColliderEvents();
			}
		}

		private void OnDestroy()
		{
			DisableColliderEvents();
		}

		private void EnableColliderEvents()
		{
			CollisionObject.ResetAllColliders = (Action)Delegate.Combine(CollisionObject.ResetAllColliders, new Action(ResetColliders));
			CollisionObject.PlayerCrashed = (Action<GameObject>)Delegate.Combine(CollisionObject.PlayerCrashed, new Action<GameObject>(OnPlayerCrashed));
		}

		private void DisableColliderEvents()
		{
			CollisionObject.ResetAllColliders = (Action)Delegate.Remove(CollisionObject.ResetAllColliders, new Action(ResetColliders));
			CollisionObject.PlayerCrashed = (Action<GameObject>)Delegate.Remove(CollisionObject.PlayerCrashed, new Action<GameObject>(OnPlayerCrashed));
		}

		private void OnPlayerCrashed(GameObject collidedObject)
		{
			if (collidedObject == null || (collidedObject != base.gameObject && collidedObject.transform.parent.gameObject != base.gameObject))
			{
				EnableColliders(CrashColliders);
			}
		}

		private void ResetColliders()
		{
			DisableColliders(BounceColliders);
			EnableColliders(CrashColliders, asTrigger: true);
		}

		public void DisableColliders(Collider[] _colliders)
		{
			foreach (Collider collider in _colliders)
			{
				if (collider != null)
				{
					collider.enabled = false;
				}
			}
		}

		public void EnableColliders(Collider[] _colliders, bool asTrigger = false)
		{
			for (int i = 0; i < _colliders.Length; i++)
			{
				Collider collider = _colliders[i];
				if (collider != null)
				{
					collider.enabled = true;
					_colliders[i].isTrigger = asTrigger;
				}
			}
		}

		private void DevTrace(string _msg)
		{
			UnityEngine.Debug.Log("[" + MethodBase.GetCurrentMethod().ReflectedType.Name + ":" + myUID + "] " + _msg);
		}
	}
}
