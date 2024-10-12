using System.Collections.Generic;

namespace Disney.ObjectPool
{
	public class ObjectPoolMulti : ObjectPoolQuantity
	{
		public List<ObjectPoolQuantity> Pools;

		private bool poolsParented;

		private ObjectPoolQuantity selectedPool;

		public override bool Ready => base.container != null && poolsParented;

		private void Start()
		{
			foreach (ObjectPoolQuantity pool in Pools)
			{
				pool.container.transform.parent = base.container.transform;
			}
			poolsParented = true;
		}

		public override void CheckIn(IPoolable poolable)
		{
			poolable.SourcePool.CheckIn(poolable);
		}

		public override IPoolable CheckOut()
		{
			return selectedPool.CheckOut();
		}

		public IPoolable CheckOut(int PoolIndex)
		{
			selectedPool = Pools[PoolIndex];
			return selectedPool.CheckOut();
		}

		public override void Shrink()
		{
			foreach (ObjectPoolQuantity pool in Pools)
			{
				((IPool)pool).Shrink();
			}
		}
	}
}
