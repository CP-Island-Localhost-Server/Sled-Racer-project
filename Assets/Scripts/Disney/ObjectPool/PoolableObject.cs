using UnityEngine;

namespace Disney.ObjectPool
{
	public class PoolableObject : MonoBehaviour, IPoolable
	{
		public IPool SourcePool
		{
			get;
			set;
		}

		public GameObject GameObject => base.gameObject;

		public virtual void Spawn(Transform newParent, Vector3 position, Quaternion rotation)
		{
			ReparentTo(newParent);
			base.transform.position = position;
			base.transform.rotation = rotation;
			base.gameObject.SetActive(value: true);
		}

		public virtual void Spawn(Vector3 position, Quaternion rotation)
		{
			base.transform.position = position;
			base.transform.rotation = rotation;
			base.gameObject.SetActive(value: true);
		}

		public virtual void Spawn(Vector3 position, Quaternion rotation, Vector3 trans)
		{
			base.transform.position = position;
			base.transform.rotation = rotation;
			base.transform.Translate(trans);
			base.gameObject.SetActive(value: true);
		}

		public virtual void Despawn()
		{
			base.gameObject.SetActive(value: false);
			SourcePool.CheckIn(this);
		}

		public void ReparentTo(Transform newParent)
		{
			base.transform.parent = newParent;
		}
	}
}
