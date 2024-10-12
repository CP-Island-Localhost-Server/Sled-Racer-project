using UnityEngine;

namespace Disney.ObjectPool
{
	public abstract class ObjectPool : MonoBehaviour, IPool
	{
		protected static int UID;

		protected int instanceID;

		public GameObject container
		{
			get;
			set;
		}

		public abstract bool Ready
		{
			get;
		}

		public ObjectPool()
		{
			instanceID = UID++;
		}

		private void Awake()
		{
			InitializeObjectPool();
		}

		protected virtual void InitializeObjectPool()
		{
			CreatePoolContainer();
		}

		protected void CreatePoolContainer()
		{
			container = new GameObject();
			container.name = GetType().ToString() + instanceID;
			container.transform.parent = base.transform;
		}

		public override string ToString()
		{
			return "[ObjectPool #" + instanceID + "]";
		}

		public abstract void CheckIn(IPoolable poolable);

		public abstract IPoolable CheckOut();

		public abstract void Shrink();
	}
}
