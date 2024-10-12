using System.Collections.Generic;
using UnityEngine;

namespace Disney.ObjectPool
{
	public class ObjectPoolQuantity : ObjectPool
	{
		private bool initialPoolReady;

		public int Priority;

		public int MinPoolSize = 10;

		public int MaxPoolSize = 100;

		public GameObject Prefab;

		private Queue<IPoolable> pooledObjects = new Queue<IPoolable>();

		public override bool Ready => Prefab != null && base.container != null && initialPoolReady;

		protected override void InitializeObjectPool()
		{
			CreatePoolContainer();
			if (!(Prefab == null))
			{
				string arg = string.Empty;
				for (int i = 0; i < MinPoolSize; i++)
				{
					IPoolable poolable = CreateNew();
					poolable.Despawn();
					arg = arg + pooledObjects.Count + "... ";
				}
				initialPoolReady = true;
			}
		}

		public override void CheckIn(IPoolable poolable)
		{
			pooledObjects.Enqueue(poolable);
			poolable.ReparentTo(base.container.transform);
		}

		public override IPoolable CheckOut()
		{
			if (pooledObjects.Count > 0)
			{
				return pooledObjects.Dequeue();
			}
			return CreateNew();
		}

		private IPoolable CreateNew()
		{
			GameObject gameObject = (GameObject)Object.Instantiate(Prefab);
			gameObject.gameObject.transform.parent = base.container.transform;
			IPoolable component = gameObject.GetComponent<PoolableObject>();
			component.SourcePool = this;
			return component;
		}

		public override void Shrink()
		{
			while (pooledObjects.Count > MinPoolSize)
			{
				GameObject gameObject = pooledObjects.Dequeue().GameObject;
				UnityEngine.Object.Destroy(gameObject);
			}
		}
	}
}
